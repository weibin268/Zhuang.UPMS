(function ($) {

    $.fn.DropdownTree = function (options) {

        var defaults = {
            setting: {
                check: {
                    enable: true,//是否启用多选
                    chkboxType: { "Y": "", "N": "" }
                },
                view: {
                    dblClickExpand: false,
                    selectedMulti: false
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                },
                edit: { enable: false }

            },
            zNodes: [],//数据源
            autoWidth:true,
            width:100,
            height: 300,
            autoHide: true,//当鼠标点击其他区域时是否隐藏树
            isParentNoSelect: true,//父节点是否可选
            enableSearch: true,//是否启用检索功能
            compatibility: true//是否为ie的兼容模式显示
        };

        var settings = $.extend(true, {}, defaults, options);
        var $data = this;
        var outerMethods = {
            getSelectedIds: function () {
                return $data.data[$data.attr("id") + "-" + "ids"];
            },
            show: function () {
                var th = this[0];
                th.methods.showMenu();
            },
            hide: function () {
                var th = this[0];
                th.methods.hideMenu();
            }
        };

        if (typeof options == "string") {
            if (outerMethods[options] !== undefined) {
                return outerMethods[options].apply(this);
            } else { alert("未找到方法‘" + options + "’！"); }

        }

        return this.each(function () {

            var $input = $(this);
            $input.attr("readonly", "readonly");

            var methods = {
                showMenu: function () {

                    var inputOffset = $input.offset();
                    $divTreePanel.css({ left: inputOffset.left + "px", top: inputOffset.top + $input.outerHeight() + "px" }).show();

                    $("html").bind("mousedown", methods.onBodyDown);
                },
                onBodyDown: function (event) {
                    if (!(event.target.id == $input.attr("id") || event.target.id == $divTreePanel.attr("id") || $(event.target).parents("#" + $divTreePanel.attr("id")).length > 0)) {
                        if (settings.autoHide) {
                            methods.hideMenu();
                        }
                    }
                },
                hideMenu: function () {

                    $divTreePanel.hide();
                    $("html").unbind("mousedown", methods.onBodyDown);

                },
                beforeClick: function (treeId, treeNode) {
                    //if (settings.isParentNoSelect && treeNode.isParent) return;
                    var zTree = $.fn.zTree.getZTreeObj($ulTree.attr("id"));
                    zTree.checkNode(treeNode, !treeNode.checked, null, true);
                    //return false;
                },
                onCheck: function (e, treeId, treeNode) {

                    var zTree = $.fn.zTree.getZTreeObj(treeId),
                    nodes = zTree.getCheckedNodes(true);
                    methods.saveData(nodes);

                },
                setAllNocheck: function () {
                    var zTree = $.fn.zTree.getZTreeObj($ulTree.attr("id"));
                    nodes = zTree.getCheckedNodes(true);
                    $.each(nodes, function (i, n) {
                        zTree.checkNode(n, !n.checked, null, true);
                    })
                },
                emptyData: function () {
                    $data.data[methods.getSaveDataKey()] = "";
                    $input.val("");
                },
                getSaveDataKey: function () {

                    return $data.attr("id") + "-" + "ids";

                },
                saveData: function (nodes) {

                    var names = "", ids = "";
                    for (var i = 0, l = nodes.length; i < l; i++) {
                        names += nodes[i].name + ",";
                        ids += nodes[i].id + ",";
                    }
                    if (names.length > 0) names = names.substring(0, names.length - 1);
                    if (ids.length > 0) ids = ids.substring(0, ids.length - 1);

                    $input.attr("value", names);
                    $data.data[methods.getSaveDataKey()] = ids;

                },
                onClick: function (e, treeId, treeNode, clickFlag) {
                    if (!settings.setting.check.enable && !(treeNode.isParent && settings.isParentNoSelect)) {//单选
                        var zTree = $.fn.zTree.getZTreeObj(treeId),
                        nodes = zTree.getSelectedNodes(true);
                        methods.saveData(nodes);
                    }
                },
                onDblClick: function (e, treeId, treeNode) {
                    if (!treeNode) return;
                    if (!settings.setting.check.enable && !(treeNode.isParent && settings.isParentNoSelect)) {//单选
                        methods.hideMenu();
                    }
                },
                searchBoxFocus: function () {
                    var $this = $(this);
                    if ($this.val() == inputDefaultValue) {
                        $this.val(''); $this.css({ "color": defaultColor });
                    }
                },
                searchBoxBlur: function () {
                    var $this = $(this);
                    if (!$this.val()) {
                        $this.val(inputDefaultValue); $this.css({ "color": tipsColor });
                    }
                },
                searchBoxKeyDown: function (event) {
                    if (event.keyCode != 13) return;//不是回车键直接返回

                    var $serachInpt = $(this);
                    var strFilter = $.trim($serachInpt.val());
                    var zTree = $.fn.zTree.getZTreeObj(treeId);
                    var nodes = zTree.getNodesByParamFuzzy("name", strFilter);

                    if (typeof preStrFilter == "undefined") preStrFilter = "";
                    if (strFilter == preStrFilter) {
                        count = count + 1;
                    } else {
                        count = 0;
                    }
                    if (count >= nodes.length) count = 0;

                    zTree.selectNode(nodes[count]);

                    $serachInpt[0].focus();

                    preStrFilter = strFilter;

                    return false;
                },
                init: function () {

                    $data.data[methods.getSaveDataKey()] = "";

                    settings.setting.callback = {
                        beforeClick: methods.beforeClick,
                        onClick: methods.onClick,
                        onDblClick: methods.onDblClick,
                        onCheck: methods.onCheck
                    }

                    if (!(settings.setting.check.enable) && settings.isParentNoSelect) {//单选
                        settings.setting.view.dblClickExpand = true;
                    }

                    if (settings.autoWidth) {
                        settings.width = $input.outerWidth(true);
                    }

                    $input.click(function () {

                        if ($divTreePanel.css("display") == "none") {
                            methods.showMenu();
                        } else {
                            methods.hideMenu();
                        }

                    });

                    $input.keydown(function (event) {
                        switch (event.keyCode) {
                            case 8://back
                                methods.emptyData();
                                methods.setAllNocheck();
                                return false;
                                break;
                            case 46://delete
                                methods.emptyData();
                                methods.setAllNocheck();
                                return false;
                                break;
                        }
                        //return false;
                    });

                }
            };

            this.methods = methods;

            methods.init();

            var idCount = 0;
            var treePanelId = "divTreePanel";
            while ($("#" + treePanelId).length > 0) {
                treePanelId = treePanelId + (++idCount);
            }

            var $divTreePanel = jQuery("<div/>").attr({
                "id": treePanelId, "class": "tree-panel",
                "style": "display: none; position: absolute;width:" + settings.width + "px"
            });


            if (!settings.autoWidth) {
                $divTreePanel.css("overflow", "auto");
            }
            
            var treeId = "ulTree";
            while ($("#" + treeId).length > 0) {
                treeId = treeId + (++idCount);
            }
            var $ulTree = jQuery("<ul/>").attr({
                "id": treeId, "class": "ztree", "style": "margin-top: 0;width:auto;"
            });

            $ulTree.height(settings.height);

            if (settings.enableSearch) {

                $divSearchBox = $("<div style='margin-top:-2px;'><input type='text' style='margin:0;width:100%;' value=''/></div>");
                $divSearchBox.width($input.width());
                $inputSearchBox = $divSearchBox.find("input");
                $inputSearchBox.keydown(methods.searchBoxKeyDown);

                var inputDefaultValue = "请输入检索关键字，后按回车！";
                var defaultColor = "#000";
                var tipsColor = '#999';
                $inputSearchBox.val(inputDefaultValue);
                $inputSearchBox.css({ "color": tipsColor });
                $inputSearchBox.focus(methods.searchBoxFocus);
                $inputSearchBox.blur(methods.searchBoxBlur);

                $divTreePanel.prepend($divSearchBox);
            }

            $divTreePanel.prepend($ulTree);

            $divTreePanel.appendTo(jQuery("body"));

            if (settings.compatibility) {
                $ulTree[0].style.width = "100%";

                if (settings.enableSearch) {
                    $divSearchBox[0].style.width = "100%";
                }

            }
            
            $.fn.zTree.init($ulTree, settings.setting, settings.zNodes);

        });
    };

})(jQuery);

