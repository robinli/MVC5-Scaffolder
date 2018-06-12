
$.extend($.fn.datebox.defaults, {
    formatter: function (value) {
        
   
        if (moment(value).isValid()) {
            var date = moment(value).format('MM/DD/YYYY');
            return date;
        } 
    },
    parser: function (value) {
         
        if (value != '1/1/0001 12:00:00 AM' && moment(value).isValid()) {
           
            return moment(value).toDate();
        } else {
            return moment().toDate();
        }
    }
});
$.extend($.fn.datetimebox.defaults, {
    formatter: function (value) {
        //console.log(value, 'formatter');
        if (moment(value).isValid()) {
            var date = moment(value).format('MM/DD/YYYY HH:mm:ss');
            return date;
        }
    },
    parser: function (value) {

        if (value != '1/1/0001 12:00:00 AM' && moment(value).isValid()) {
           
            return moment(value).toDate();
        } else {
            return moment().toDate();
        }
    }
});

//dateRange filter
$.extend($.fn.datagrid.defaults.filters, {
    dateRange: {
        init: function (container, options) {
            var cc = $('<span class="datagrid-editable-input textbox combo datebox"><span class="textbox-addon textbox-addon-right" style="right: 0px; top: 0px;"><a href="javascript:" class="textbox-icon combo-arrow" icon-index="0" tabindex="-1" style="width: 18px; height: 31px;"></a></span></span>').appendTo(container);
            var input = $('<input type="text" style="border:0px ;height: 31px;">').appendTo(cc);
            var myoptions = {
                applyClass: 'btn-sm btn-success',
                cancelClass: 'btn-sm btn-default',
                locale: {
                    applyLabel: '确认',
                    cancelLabel: '清空',
                    fromLabel: '起始时间',
                    toLabel: '结束时间',
                    customRangeLabel: '自定义',
                    firstDay: 1,
                    daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                    monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                        '七月', '八月', '九月', '十月', '十一月', '十二月'],
                },
                ranges: {

                    //'最近1小时': [moment().subtract('hours',1), moment()],
                    '今日': [moment(), moment()],
                    '昨日': [moment().subtract(1, 'days').startOf('day'), moment().subtract(1, 'days').endOf('day')],
                    '最近7日': [moment().subtract(6, 'days'), moment()],
                    '最近30日': [moment().subtract(29, 'days'), moment()],
                    '本月': [moment().startOf("month"), moment().endOf("month")],
                    '上个月': [moment().subtract(1, "month").startOf("month"), moment().subtract(1, "month").endOf("month")]
                },
                opens: 'right',    // 日期选择框的弹出位置
                separator: '-',
                showWeekNumbers: false,     // 是否显示第几周
                format: 'MM/DD/YYYY'
            }
            input.daterangepicker(myoptions);
            container.find('.textbox-icon').on('click', function () {
                container.find('input').trigger('click.daterangepicker');
            });
            if (options.onChange == undefined) {
                //console.log('Can not find function:onChange for', input[0]);
            }
            else {
                input.on('cancel.daterangepicker', function (ev, picker) {
                    $(this).val('');
                    options.onChange('');
                });
                input.on('apply.daterangepicker', function (ev, picker) {
                    options.onChange(picker.startDate.format('MM/DD/YYYY') + '-' + picker.endDate.format('MM/DD/YYYY'));
                });
            }

            //console.log($(target));
            return input;
        },
        destroy: function (target) {
            $(target).daterangepicker('destroy');
        },
        getValue: function (target) {
           
           
            return $(target).data('daterangepicker').getStartDate() + '-' + $(target).data('daterangepicker').getEndDate();
        },
        setValue: function (target, value) {
            //console.log($(target), value);
    
            var daterange = value.split('-');
            $(target).data('daterangepicker').setStartDate(daterange[0]);
            $(target).data('daterangepicker').setEndDate(daterange[1]);

        },
        resize: function (target, width) {
            $(target)._outerWidth(width)._outerHeight(25);
            // $(target).daterangepicker('resize', width / 2);
        }
    }
});

 

//datebox editor
$.extend($.fn.datagrid.defaults.editors, {
    datebox: {
        init: function (container, options) {
            var input = $('<input>').appendTo(container);
            input.datebox(options);
            return input;
        },
        destroy: function (target) {
            $(target).datebox('destroy');
        },
        getValue: function (target) {

            return $(target).datebox('getValue');
        },
        setValue: function (target, value) {

            var date = {};
            if (moment(value).isValid()) {
                date = moment(value).format('MM/DD/YYYY');
                $(target).datebox('setValue', date);
            } else {
                $(target).datebox('setValue', '');
            }
            //console.log(value, date)


        },
        resize: function (target, width) {
            $(target).datebox('resize', width);
        }
    }
});
//CheckBox Editor
$.extend($.fn.datagrid.defaults.editors, {
    booleaneditor: {
        

        init: function (container, options) {
            var checked = '<div class="datagrid-cell"><div class="smart-form"><label class="checkbox ">' +
                '<input type="checkbox" name="checkbox"   >' +
                '<i></i>&nbsp; </label></div></div>';
            var input = $(checked).appendTo(container);
            
            return input;
        },
        destroy: function (target) {
            
        },
        getValue: function (target) {
            //console.log('getValue', $(target[0]).find(':checkbox').prop('checked'));
            return $(target[0]).find(':checkbox').prop('checked');
        },
        setValue: function (target, value) {
        
                $(target[0]).find(':checkbox').prop('checked', value);
           


        },
        resize: function (target, width) {
             
        }
    }
});



function dateformatter(value, row, index) {
   
    if (value == typeof (undefined))
        return null;
    if (value == null)
        return null;

  
    if (moment(value).isValid()) {

        return moment(value).format('MM/DD/YYYY');

    }

}
function datetimeformatter(value, row, index) {


    if (value == typeof (undefined))
        return null;
    if (value == null)
        return null;
    if (moment(value).isValid())
        return moment(value).format('MM/DD/YYYY HH:mm:ss');
}

function isTrue(value) {
    if (typeof (value) === 'string') {
        value = value.trim().toLowerCase();
    }
    switch (value) {
        case true:
        case "true":
        case 1:
        case "1":
        case "on":
        case "yes":
            return true;
        default:
            return false;
    }
}
function booleanformatter(value, row, index) {
 
  


    if (isTrue(value)) {
        var checked = '<div class="smart-form"><label class="checkbox state-disabled">' +
            '<input type="checkbox" name="checkbox" checked="checked" disabled="disabled">' +
            '<i></i>&nbsp; </label></div>';

        return checked
    } else {
        var unchecked = '<div class="smart-form"><label class="checkbox state-disabled">' +
            '<input type="checkbox" name="checkbox"   disabled="disabled">' +
            '<i></i>&nbsp; </label></div>';

        return unchecked
    }


}
