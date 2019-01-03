moment.suppressDeprecationWarnings = true;
jQuery.extend({
    datetimeNow: function () {
        //console.log(new Date());
        return moment(new Date()).format('MM/DD/YYYY');
    },
    isDateVaild: function (value) {
       
        var regex = new RegExp("^\\d{1,2}\\/\\d{1,2}\\/\\d{4}$");
        return regex.test(value);
    },
    isDateTimeVaild: function (value) {

        var regex = new RegExp("/(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2})/");
        return regex.test(value);
    },
    getUserName: function () {
        return $('#currentuser').val();
    }
});
