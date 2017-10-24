
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
    for (var i = 0; i < statusdatasource.length; i++) {
      var item = statusdatasource[i];
     if (item.value === value.toString())
     {
         return item.text;
     }
     }
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

//for datagrid   Status   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
    statuscombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: statusdatasource,
                valueField: 'value',
                textField: 'text'
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
            //console.info(dateformatter(value));
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
    for (var i = 0; i < isenableddatasource.length; i++) {
      var item = isenableddatasource[i];
     if (item.value === value.toString())
     {
         return item.text;
     }
     }
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

//for datagrid   IsEnabled   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
    isenabledcombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: isenableddatasource,
                valueField: 'value',
                textField: 'text'
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
            //console.info(dateformatter(value));
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
    for (var i = 0; i < isdisableddatasource.length; i++) {
      var item = isdisableddatasource[i];
     if (item.value === value.toString())
     {
         return item.text;
     }
     }
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

//for datagrid   IsDisabled   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
    isdisabledcombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: isdisableddatasource,
                valueField: 'value',
                textField: 'text'
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
            //console.info(dateformatter(value));
            $(target).combobox('setValue', value);
        },
        resize: function (target, width) {
            $(target).combobox('resize', width);
        }
    }
});


 // 账号类型
 var accounttypefiltersource = [{ value: '', text: 'All'}] ; 
 var accounttypedatasource = [] ; 
    accounttypefiltersource.push({ value: '0', text: '公司' });
   accounttypedatasource.push({ value: '0', text: '公司' });
   accounttypefiltersource.push({ value: '1', text: '供应商' });
   accounttypedatasource.push({ value: '1', text: '供应商' });
   accounttypefiltersource.push({ value: '2', text: '客户' });
   accounttypedatasource.push({ value: '2', text: '客户' });
   accounttypefiltersource.push({ value: '3', text: '外协单位' });
   accounttypedatasource.push({ value: '3', text: '外协单位' });
//for datagrid AccountType field  formatter
function accounttypeformatter(value, row, index) {
    if (value === null || value === '' || value === undefined ) {
        return "";
    }
    for (var i = 0; i < accounttypedatasource.length; i++) {
      var item = accounttypedatasource[i];
     if (item.value === value.toString())
     {
         return item.text;
     }
     }
    return value;
   
}

//for datagrid   AccountType   field filter 
$.extend($.fn.datagrid.defaults.filters, {
    accounttypecombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: accounttypefiltersource
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

//for datagrid   AccountType   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
    accounttypecombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: accounttypedatasource,
                valueField: 'value',
                textField: 'text'
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
            //console.info(dateformatter(value));
            $(target).combobox('setValue', value);
        },
        resize: function (target, width) {
            $(target).combobox('resize', width);
        }
    }
});


 // 日志分组
 var messagegroupfiltersource = [{ value: '', text: 'All'}] ; 
 var messagegroupdatasource = [] ; 
    messagegroupfiltersource.push({ value: '0', text: '系统操作' });
   messagegroupdatasource.push({ value: '0', text: '系统操作' });
   messagegroupfiltersource.push({ value: '1', text: '业务操作' });
   messagegroupdatasource.push({ value: '1', text: '业务操作' });
   messagegroupfiltersource.push({ value: '2', text: '接口操作' });
   messagegroupdatasource.push({ value: '2', text: '接口操作' });
//for datagrid MessageGroup field  formatter
function messagegroupformatter(value, row, index) {
    if (value === null || value === '' || value === undefined ) {
        return "";
    }
    for (var i = 0; i < messagegroupdatasource.length; i++) {
      var item = messagegroupdatasource[i];
     if (item.value === value.toString())
     {
         return item.text;
     }
     }
    return value;
   
}

//for datagrid   MessageGroup   field filter 
$.extend($.fn.datagrid.defaults.filters, {
    messagegroupcombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: messagegroupfiltersource
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

//for datagrid   MessageGroup   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
    messagegroupcombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: messagegroupdatasource,
                valueField: 'value',
                textField: 'text'
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
            //console.info(dateformatter(value));
            $(target).combobox('setValue', value);
        },
        resize: function (target, width) {
            $(target).combobox('resize', width);
        }
    }
});


 // 日志类型
 var messagetypefiltersource = [{ value: '', text: 'All'}] ; 
 var messagetypedatasource = [] ; 
    messagetypefiltersource.push({ value: '0', text: 'Information' });
   messagetypedatasource.push({ value: '0', text: 'Information' });
   messagetypefiltersource.push({ value: '1', text: 'Error' });
   messagetypedatasource.push({ value: '1', text: 'Error' });
   messagetypefiltersource.push({ value: '2', text: 'Alert' });
   messagetypedatasource.push({ value: '2', text: 'Alert' });
//for datagrid MessageType field  formatter
function messagetypeformatter(value, row, index) {
    if (value === null || value === '' || value === undefined ) {
        return "";
    }
    for (var i = 0; i < messagetypedatasource.length; i++) {
      var item = messagetypedatasource[i];
     if (item.value === value.toString())
     {
         return item.text;
     }
     }
    return value;
   
}

//for datagrid   MessageType   field filter 
$.extend($.fn.datagrid.defaults.filters, {
    messagetypecombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: messagetypefiltersource
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

//for datagrid   MessageType   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
    messagetypecombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: messagetypedatasource,
                valueField: 'value',
                textField: 'text'
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
            //console.info(dateformatter(value));
            $(target).combobox('setValue', value);
        },
        resize: function (target, width) {
            $(target).combobox('resize', width);
        }
    }
});


 // 已读标志
 var isnewfiltersource = [{ value: '', text: 'All'}] ; 
 var isnewdatasource = [] ; 
    isnewfiltersource.push({ value: '0', text: '未读' });
   isnewdatasource.push({ value: '0', text: '未读' });
   isnewfiltersource.push({ value: '1', text: '已读' });
   isnewdatasource.push({ value: '1', text: '已读' });
//for datagrid IsNew field  formatter
function isnewformatter(value, row, index) {
    if (value === null || value === '' || value === undefined ) {
        return "";
    }
    for (var i = 0; i < isnewdatasource.length; i++) {
      var item = isnewdatasource[i];
     if (item.value === value.toString())
     {
         return item.text;
     }
     }
    return value;
   
}

//for datagrid   IsNew   field filter 
$.extend($.fn.datagrid.defaults.filters, {
    isnewcombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: isnewfiltersource
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

//for datagrid   IsNew   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
    isnewcombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: isnewdatasource,
                valueField: 'value',
                textField: 'text'
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
            //console.info(dateformatter(value));
            $(target).combobox('setValue', value);
        },
        resize: function (target, width) {
            $(target).combobox('resize', width);
        }
    }
});


 // 通知标志
 var isnoticefiltersource = [{ value: '', text: 'All'}] ; 
 var isnoticedatasource = [] ; 
    isnoticefiltersource.push({ value: '0', text: '未发' });
   isnoticedatasource.push({ value: '0', text: '未发' });
   isnoticefiltersource.push({ value: '1', text: '已发' });
   isnoticedatasource.push({ value: '1', text: '已发' });
//for datagrid IsNotice field  formatter
function isnoticeformatter(value, row, index) {
    if (value === null || value === '' || value === undefined ) {
        return "";
    }
    for (var i = 0; i < isnoticedatasource.length; i++) {
      var item = isnoticedatasource[i];
     if (item.value === value.toString())
     {
         return item.text;
     }
     }
    return value;
   
}

//for datagrid   IsNotice   field filter 
$.extend($.fn.datagrid.defaults.filters, {
    isnoticecombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: isnoticefiltersource
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

//for datagrid   IsNotice   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
    isnoticecombobox: {
        init: function (container, options) {
            var input = $('<input type="text">').appendTo(container);
            var myoptions = {
                panelHeight: "auto",
                data: isnoticedatasource,
                valueField: 'value',
                textField: 'text'
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
            //console.info(dateformatter(value));
            $(target).combobox('setValue', value);
        },
        resize: function (target, width) {
            $(target).combobox('resize', width);
        }
    }
});





