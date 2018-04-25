jQuery.extend({
    datetimeNow: function () {
        //console.log(new Date());
        return moment(new Date()).format('MM/DD/YYYY');
    },
    isDateVaild: function (value) {
       
        var regex = new RegExp("^\\d{1,2}\\/\\d{1,2}\\/\\d{4}$");
        return regex.test(value);
    }
});


//(function ($) {
//    $.fn.serializeObject = function () {
//        "use strict";

//        var result = {};
//        var extend = function (i, element) {
//            var node = result[element.name];
            
//            // If node with same name exists already, need to convert it to an array as it
//            // is a multi-value field (i.e., checkboxes)

//            if ('undefined' !== typeof node && node !== null) {
//                if ($.isArray(node)) {
//                    node.push(element.value);
//                } else {
//                    result[element.name] = [node, element.value];
//                }
//            } else {
//                result[element.name] = element.value;
//            }
//        };
//        var formproparray = this.serializeArray();
//        var checkboxes = $('input[type=radio],input[type=checkbox]', this);
//        var temp = {};
//        $.each(checkboxes, function () {
//            if (!temp.hasOwnProperty(this.name)) {
//                if ($("input[name='" + this.name + "']:checked").length == 0) {
//                    temp[this.name] = "";
//                    formproparray.push({ name: this.name, value: "False" });
//                }
//            }
//        })
//        $.each(formproparray, extend);
//        return result;
//    };
//})(jQuery);


////$.dateNow()