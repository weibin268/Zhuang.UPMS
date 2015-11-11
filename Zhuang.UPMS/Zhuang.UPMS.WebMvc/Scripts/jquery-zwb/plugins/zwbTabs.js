/*

    <div id="zwbtabs1" >
        <p><span id="Span1">图纸目录</span> <span id="Span2">附件</span></p>
        <ul class="Span1" >
            aaa
        </ul>
        <ul class="Span2">
            bbbb
        </ul>
    </div>

*/


(function ($) {

    $.fn.zwbTabs = function (options) {

        //设置参数
        var defaults = {
            isSingle:false
        };

        var settings = $.extend(true,{}, defaults, options);

        return this.each(function () {

            $this = $(this);

            $this.attr("class", "zwbtabs");

            $this.find("span:first").addClass("current"); //为第一个SPAN添加当前效果样式

            if (!settings.isSingle) {

                $this.find("li:not(:first)").hide(); //隐藏其它的UL
                $this.find("span").click(function () {
                    $this2=$(this);
                    $this.find("span").removeClass("current"); //去掉所有SPAN的样式
                    $this2.addClass("current");
                    $this.find("li").hide();
                    $("." + $this2.attr("id")).show();
                    //$(this).show();
                });
            }

        });

    };

})(jQuery)


