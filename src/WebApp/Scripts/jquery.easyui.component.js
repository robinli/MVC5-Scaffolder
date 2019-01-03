//dateRange filter
$.extend($.fn.datagrid.defaults.filters, {
    dateRange: {
        init: function (container, options) {
          var cc = $('<span class="textbox combo datebox"><span class="textbox-addon textbox-addon-right" style="right: 4px; top: -1px;"><a href="javascript:" class="textbox-icon combo-arrow" icon-index="0" tabindex="-1" style="width: 18px; height: 31px;"></a></span></span>').appendTo(container);
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
          };
          input.on('blur', function () {
            $(this).parent().removeClass('textbox-focused'); 
          }).on('focus', function () {
            $(this).parent().addClass('textbox-focused');
          });
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
            $(target)._outerWidth(width - 10)._outerHeight(25);
            // $(target).daterangepicker('resize', width / 2);
        }
    }
});
//booleanfilter
$.extend($.fn.datagrid.defaults.filters, {
    booleanfilter: {
        init: function (container, options) {
            var input = $('<select class="easyui-combobox" >').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: [{ value: '', text: 'All' }, { value: 'true', text: 'True' }, { value: 'false', text: 'False' }],
                onChange: function () {
                    input.trigger('combobox.filter');
                }
            }
            $.extend(options, myoptions);
            input.combobox(options);
            return input;
        },
        destroy: function (target) {
            $(target).combobox('destroy');
        },
        getValue: function (target) {
            return $(target).combobox('getValue');
        },
        setValue: function (target, value) {
            $(target).combobox('setValue', value);
        },
        resize: function (target, width) {
            $(target).combobox('resize', width);
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
