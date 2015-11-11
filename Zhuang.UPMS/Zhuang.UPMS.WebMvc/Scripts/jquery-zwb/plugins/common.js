(function ($) {

    $.fn.serializeJson = function (prefix) {
        var serializeObj = {};
        $(this.serializeArray()).each(function () {
            var name = this.name;
            if (prefix)
                name = prefix + name;
            serializeObj[name] = this.value;

        });
        return serializeObj;
    };

    $.fn.serializeJsonFilter = function () {
        return this.serializeJson("Filter_");
    };

})(jQuery);

