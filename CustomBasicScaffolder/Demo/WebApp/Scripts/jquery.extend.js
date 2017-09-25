jQuery.extend({
    dateNow: function () {
        //console.log(new Date());
        return moment(new Date()).format('MM/DD/YYYY');
    }
});
//$.dateNow()