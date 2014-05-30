HappyMessager = (function () {
    var templates = {
        success: "<div class='alert alert-success' style='position: absolute; z-index: 10000;display:none;top:0 ;width:100%'>" +
            " <a href='#' class='close' data-dismiss='alert'>&times;</a>" +
              "<strong>Success </strong><span id='ntxAlertMessage'></span>" +
                "</div>"
        ,
        alert: "<div class='alert alert-danger' style='position: absolute; z-index: 10000;display:none;top:0 ;width:100%'>" +
            " <a href='#' class='close' data-dismiss='alert'>&times;</a>" +
              "<strong>Warning! </strong><span id='ntxAlertMessage'></span>" +
                "</div>"
    };
    function success() {
        $(".alert").alert("close");
        var alertObj = $(templates.success);
        $("body").append(alertObj);
        var options = sysCoreMapArguments(arguments, ["message", "callback"]);
        $("#ntxAlertMessage").html(options.message);
        $('.alert').fadeIn(1000).fadeOut(15000);
        $(".alert").on("closed.bs.alert", function () {
            if (options.callback)
                options.callback();
        });
    };
    function alert() {
        $(".alert").alert("close");
        var alertObj = $(templates.alert);
        $("body").append(alertObj);
        var options = sysCoreMapArguments(arguments, ["message", "callback"]);
        $("#ntxAlertMessage").html(options.message);
        $('.alert').fadeIn(1000).fadeOut(15000);
        $(".alert").on("closed.bs.alert", function () {
            if (options.callback)
                options.callback();
        });
    };
    function sysCoreMapArguments(args, properties) {
        var argn = Math.min(args.length, properties.length);
        var options = {};
        for (var i = 0; i < argn; i++) {
            options[properties[i]] = args[i];
        }
        return options;
    }
    var exports = {};
    exports.success = success;
    exports.alert = alert;
    return exports;
}());