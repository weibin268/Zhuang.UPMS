(function ($) {

    var _setup = function (self, options, defaults) {

        var $self = $(self);
        var settings = $.extend(true,{}, defaults, options);
        settings.getTotalColWidth = function () {

            var result = 0;

            if (settings.usePercent) {

                baseElements.$base.css("width", settings.width);
                result = baseElements.$base.width() - settings.colDiffWdith;
                

            } else {

                for (var i = 0; i < this.colModel.length; i++) {
                    result += this.colModel[i]["width"];
                }

            }

            return result;
        };

        settings.resetColWdith = function () {

            for (var i = 0; i < settings.colModel.length; i++)
            {
                var currentCol = settings.colModel[i]["width"];
                settings.colModel[i]["width"] = settings.getTotalColWidth() * (parseInt(currentCol.toString().replace(/%/g, "")) / 100);
                settings.colModel[i]["width"] = settings.colModel[i]["width"] - 12;//相减的值与css文件设置的单元格padding的值相关
            }

        }

        /*********************************
        定义基础元素标签
        $base
            $divHead
                $table.clone
            $divBody
                $table
                    $tr
                        $td
        **********************************/
        var baseElements = {

            $base: $(),
            $divHead: $("<div></div>"),
            $divColumnResizer: $("<div class='divColumnResizer'></div>"),    //用来控制拖拽列的div
            $divBody: $("<div></div>"),
            $table: $("<table></table>"),
            $tr: $("<tr></tr>"),
            $td: $("<td></td>"),
            $divcell: $("<div></div>"),

        };

        //事响应处理方法
        var eventMethods = {

            onTrMouseOver: function () {
                $(this).addClass("zwbgrid-tr-over");
            },
            onTrMouseOut: function () {
                $(this).removeClass("zwbgrid-tr-over");
            },
            onTrClick: function () {

                $(this).parent().find(".zwbgrid-tr-selected").removeClass("zwbgrid-tr-selected");
                $(this).addClass("zwbgrid-tr-selected");

            },
            onTdMouseOver: function () {
                var $this = $(this);
                $this.attr("title", $this.text());
            },
            onDivBodyScroll: function (e) {
                var $this = $(this);
                var scrollLeft = $this.scrollLeft();
                var scrollTop = $this.scrollTop();
                baseElements.$divHead.scrollLeft(scrollLeft);
                baseElements.$divHead.scrollTop(scrollTop);

            },
            onDivHeadMouseDown: function (e) {
                var $this = $(this);
                var td = $this.parent();
                var tdSn = td.data("sn");
                var bodyTd = baseElements.$divBody.find("tr:first td:eq(" + tdSn + ")");
                var bodyTable = baseElements.$divBody.find("table");
                var startX = e.clientX;
                $(document).on("mousemove", function (e) {
                    td.width(td.width() + (e.clientX - startX));
                    bodyTd.width(bodyTd.width() + (e.clientX - startX));
                    bodyTable.width(bodyTable.width() + (e.clientX - startX));
                    startX = e.clientX;
                });

                $(document).one("mouseup", function () {
                    $(document).off("mousemove");
                });

            }

        };

        var baseMethods = {

            init: function () {
                baseElements.$base = $self;
                baseElements.$base.width(options.width || settings.getTotalColWidth() + 40);//确定整个grid的宽度
                baseElements.$divColumnResizer.width(10).height(20)
                .css({ "background-color": "red", float: "right", "margin-right": -6, "cursor": "e-resize" });
                baseElements.$divBody.height(settings.height)
                .css("overflow-y", "scroll");
                baseElements.$table.width(settings.getTotalColWidth());


                baseElements.$base.append(baseElements.$divHead);
                baseElements.$base.append(baseElements.$divBody);
                baseElements.$divBody.append(baseElements.$table);

                settings.resetColWdith();
                
            },

            //创建Grid的Head
            createHead: function () {

                var headTable = baseElements.$table.clone();
                var headTr = baseElements.$tr.clone();
                headTr.css("height", settings.headerHeight);

                baseElements.$divHead.append(headTable);
                headTable.append(headTr);

                for (var j = 0; j < settings.colModel.length; j++) {
                    var headTd = baseElements.$td.clone();
                    headTr.append(headTd);
                    headTd.data("sn", j);
                    headTd.width(settings.colModel[j]["width"]);//设置宽度
                    headTd.html(settings.colModel[j]["title"]);//设置列名

                    //if(settings.colModel[j]["text-align"])
                    //{
                    //    headTd.css("text-align", settings.colModel[j]["text-align"]);//设置列对方式
                    //}

                    headTd.css("border-top", "0px");

                    if (j == 0)//如果是第一列
                    {
                        headTd.css("border-left", "0px");
                    }

                    if (settings.useColumnResizer)
                    {
                        headTd.append(baseElements.$divColumnResizer.clone());
                    }
                    
                }

                //增加最后一个单元格用于替代滚动条的空间
                var lastTd = baseElements.$td.clone();
                headTr.append(lastTd.width(999).css("border", "0px"));

            },


            //创建Grid的Body
            createBody: function () {

                for (var i = 0; i < settings.data.length; i++) {
                    //创建行
                    var bodyTr = baseElements.$tr.clone();
                    baseElements.$table.append(bodyTr);

                    for (var j = 0; j < settings.colModel.length; j++) {
                        //创建单元格
                        var bodyTd = baseElements.$td.clone();
                        bodyTr.append(bodyTd);

                        //填充单元格值
                        //var tddiv = $divcell.clone();
                        //tddiv.html(settings.data[i][settings.colModel[j]["name"]]);
                        //bodyTd.append(tddiv);
                        bodyTd.html(settings.data[i][settings.colModel[j]["name"]]);//设置单元格值
                        if (settings.colModel[j]["text-align"]) {
                            bodyTd.css("text-align", settings.colModel[j]["text-align"]);//设置列对方式
                        }
                        if (i == 0)//如果是第一行
                        {
                            bodyTd.width(settings.colModel[j]["width"]);
                            bodyTd.css("border-top", "0px");

                        } else if (i == settings.data.length - 1)//如果是最后一行
                        {
                            bodyTd.css("border-bottom", "0px");
                        }

                        if (j == 0)//如果是第一列
                        {
                            bodyTd.css("border-left", "0px");
                        }

                    }

                }

            },

            //应用css样式
            applyCss: function () {

                baseElements.$base.addClass("zwbgrid-base");
                baseElements.$divHead.addClass("zwbgrid-divhead");
                baseElements.$divBody.addClass("zwbgrid-divbody");
                baseElements.$divBody.find("table tr:even").addClass("zwbgrid-tr-even");
                baseElements.$divBody.find("table tr:odd").addClass("zwbgrid-tr-odd");

            },

            //应用event事件
            applyEvent: function () {

                baseElements.$divBody.find("table tr").mouseover(eventMethods.onTrMouseOver)
                .mouseout(eventMethods.onTrMouseOut)
                .click(eventMethods.onTrClick);
                baseElements.$divBody.find("table td").mouseover(eventMethods.onTdMouseOver);

                //表头横向滚动条移动处理
                baseElements.$divBody.scroll(eventMethods.onDivBodyScroll);

                baseElements.$divHead.find(".divColumnResizer").on("mousedown", eventMethods.onDivHeadMouseDown);

            }

        };


        //执行基本方法
        baseMethods.init();
        baseMethods.createHead();
        baseMethods.createBody();
        baseMethods.applyCss();
        baseMethods.applyEvent();

    };




    $.fn.zwbGrid = function (options) {

        //设置参数
        var defaults = {
            height: 100,
            width: 500,
            datatype: "local",//
            colNames: ["列1", "列2"],//列标题名称
            colModel: [{ name: "col1", title: "列1", width: 50 }],//列的各种属性
            data: [],//数据源
            usePercent: false,
            useColumnResizer: false,
            headerHeight: 24,
            colDiffWdith:24
        };

        //支持应于于多个jquery对象
        return this.each(function () {

            return _setup(this, options, defaults);

        });
    }



})(jQuery);