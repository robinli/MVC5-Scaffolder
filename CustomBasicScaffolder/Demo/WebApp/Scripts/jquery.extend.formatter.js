
 // Test
 var statusfiltersource = [{ value: '', text: 'All'}] ; 
 var statusdatasource = [] ; 
   statusfiltersource.push({ value: '0', text: '新增' });
   statusdatasource.push({ value: '0', text: '新增' });
   statusfiltersource.push({ value: '1', text: '修改' });
   statusdatasource.push({ value: '1', text: '修改' });
//for datagrid Status field  formatter
function statusformatter(value, row, index) {
    if (value === null || value === '' || value === undefined ) {
        return "";
    }
    $.each(statusdatasource, function (index, item) {
        if (item.value === value.toString()) {
            return item.text;
        }
    });
    return value;
   
}

//for datagrid   Status   field filter 
$.extend($.fn.datagrid.defaults.filters, {
    statuscombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: statusfiltersource
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

 // 启用标志
 var isenabledfiltersource = [{ value: '', text: 'All'}] ; 
 var isenableddatasource = [] ; 
    isenabledfiltersource.push({ value: '0', text: '未启用' });
   isenableddatasource.push({ value: '0', text: '未启用' });
   isenabledfiltersource.push({ value: '1', text: '已启用' });
   isenableddatasource.push({ value: '1', text: '已启用' });
//for datagrid IsEnabled field  formatter
function isenabledformatter(value, row, index) {
    if (value === null || value === '' || value === undefined ) {
        return "";
    }
    $.each(isenableddatasource, function (index, item) {
        if (item.value === value.toString()) {
            return item.text;
        }
    });
    return value;
   
}

//for datagrid   IsEnabled   field filter 
$.extend($.fn.datagrid.defaults.filters, {
    isenabledcombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: isenabledfiltersource
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

 // 禁用标志
 var isdisabledfiltersource = [{ value: '', text: 'All'}] ; 
 var isdisableddatasource = [] ; 
    isdisabledfiltersource.push({ value: '0', text: '未禁用' });
   isdisableddatasource.push({ value: '0', text: '未禁用' });
   isdisabledfiltersource.push({ value: '1', text: '已禁用' });
   isdisableddatasource.push({ value: '1', text: '已禁用' });
//for datagrid IsDisabled field  formatter
function isdisabledformatter(value, row, index) {
    if (value === null || value === '' || value === undefined ) {
        return "";
    }
    $.each(isdisableddatasource, function (index, item) {
        if (item.value === value.toString()) {
            return item.text;
        }
    });
    return value;
   
}

//for datagrid   IsDisabled   field filter 
$.extend($.fn.datagrid.defaults.filters, {
    isdisabledcombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: isdisabledfiltersource
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




