$.extend($.fn.datagrid.defaults.filters, {
    dateRange1: {
        init: function (container, options) {
            var input = $('<input>').appendTo(container);
            input.datebox({
                panelWidth: 300,
                panelHeight: 209,
                onShowPanel: function () {
                    var dd = input.combo('getText').split(':');
                    var d1 = $.fn.datebox.defaults.parser(dd[0]);
                    var d2 = $.fn.datebox.defaults.parser(dd[1]);
                    var p = input.combo('panel');
                    p.find('.c1').calendar('moveTo', d1);
                    p.find('.c2').calendar('moveTo', d2);
                }
            });
            var p = input.datebox('panel');
            $('<div><div class="c1" style="width:50%;float:left"></div><div class="c2" style="width:50%;float:right"></div></div>').appendTo(p);
            var c1 = p.find('.c1').calendar();
            var c2 = p.find('.c2').calendar();

            var footer = $('<div class="easyui-panel"></div>').appendTo(p);
            var btn = $('<a class="easyui-linkbutton" data-options="plain:true,iconCls:"icon- save" href="javascript:void(0)">Done</a>').appendTo(footer);
            btn.bind('click', function () {
                var v1 = $.fn.datebox.defaults.formatter(c1.calendar('options').current);
                var v2 = $.fn.datebox.defaults.formatter(c2.calendar('options').current);
                var v = v1 + ':' + v2;
                input.combo('setValue', v).combo('setText', v);
                input.combo('hidePanel');
            })
            return input;
        },
        destroy: function (target) {
            $(target).combo('destroy');
        },
        getValue: function (target) {
            var p = $(target).combo('panel');
            var v1 = $.fn.datebox.defaults.formatter(p.find('.c1').calendar('options').current);
            var v2 = $.fn.datebox.defaults.formatter(p.find('.c2').calendar('options').current);
            return v1 + ':' + v2;
        },
        setValue: function (target, value) {
            var dd = value.split(':');
            var d1 = $.fn.datebox.defaults.parser(dd[0]);
            var d2 = $.fn.datebox.defaults.parser(dd[1]);
            var p = $(target).combo('panel');
            p.find('.c1').calendar('moveTo', d1);
            p.find('.c2').calendar('moveTo', d2);
            $(target).combo('setValue', value).combo('setText', value);
        },
        resize: function (target, width) {
            $(target).combo('resize', width);
        }
    }
});

$.extend($.fn.datagrid.defaults.filters, {
    dateRange: {
        init: function (container, options) {
            var cc = $('<span class="textbox combo datebox"><span class="textbox-addon textbox-addon-right" style="right: 0px; top: 1px;"><a href="javascript:" class="textbox-icon combo-arrow" icon-index="0" tabindex="-1" style="width: 18px; height: 22px;"></a></span></span>').appendTo(container);
            var input = $('<input type="text" style="border:0px">').appendTo(cc);
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
                    '最近7日': [moment().subtract(6,'days'), moment()],
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
                console.log('Can not find function:onChange for', input[0]);
            }
            else {
                input.on('cancel.daterangepicker', function (ev, picker) {
                    $(this).val('');
                    options.onChange('');
                });
                input.on('apply.daterangepicker', function (ev, picker) {
                    options.onChange(picker.startDate.format('MM/DD/YYYY') +'-'+ picker.endDate.format('MM/DD/YYYY'));
                });
            }
            
            //console.log($(target));
            return input;
        },
        destroy: function (target) {
            $(target).daterangepicker('destroy');
        },
        getValue: function (target) {
            //console.log($(target));
            return $(target).data('daterangepicker').getStartDate() + '-' + $(target).data('daterangepicker').getEndDate();
        },
        setValue: function (target, value) {
            //console.log($(target), value);
            var daterange = value.split('-');
            $(target).data('daterangepicker').setStartDate(daterange[0]);
            $(target).data('daterangepicker').setEndDate(daterange[1]);
           
        },
        resize: function (target, width) {
            $(target)._outerWidth(width)._outerHeight(24);
           // $(target).daterangepicker('resize', width / 2);
        }
    }
});