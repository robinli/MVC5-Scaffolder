//easyui datebox default options
$.extend($.fn.datebox.defaults, {
    formatter: function (value) {
        if (value !== null && typeof value !== "undefined"  && moment(value).isValid()) {
            var date = moment(value).format('MM/DD/YYYY');
            return date;
        } 
    },
    parser: function (value) {
        if (value !== null && typeof value !== "undefined"  && moment(value).isValid() && !moment(value).isSame(moment("1/1/0001 12:00:00 AM"))) {
            return moment(value).toDate();
        } else {
            return moment().toDate();
        }
    }
});
$.extend($.fn.datetimebox.defaults, {
    formatter: function (value) {
        if (value !== null && typeof value !== "undefined" && moment(value).isValid()) {
            var date = moment(value).format('MM/DD/YYYY HH:mm:ss');
            return date;
        } else {
            return null;
        }
    },
    parser: function (value) {
        if (value !== null && typeof value !== "undefined"  &&  moment(value).isValid() && !moment(value).isSame(moment("1/1/0001 12:00:00 AM"))) {
            return moment(value).toDate();
        } else {
            return moment().toDate();
        }
    }
});
//extend validatebox regex
$.extend($.fn.validatebox.defaults.rules, {
    regex: {
        validator: function (value, param) {
            var re = new RegExp(param[0]);
            return re.test(value);
        },
        message: '{1}'
    },
    carNo: {
        validator: function (value) {
          return /^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9挂学警港澳]{1}$/.test(value);
        },
        message: '车牌号码无效（例：苏E1235X）'
    },
    ip: {// 验证IP地址  
        validator: function (value) {
            return /d+.d+.d+.d+/i.test(value);
        },
        message: 'IP地址格式不正确'
    },
    zip: {// 验证邮政编码  
        validator: function (value) {
            return /^[1-9]\d{5}$/i.test(value);
        },
        message: '邮政编码格式不正确'
    },
    chinese: {// 验证中文  
        validator: function (value) {
            return /^[\u0391-\uFFE5]+$/i.test(value);
        },
        message: '请输入中文'
    },
    english: {// 验证英语  
        validator: function (value) {
            return /^[A-Za-z]+$/i.test(value);
        },
        message: '请输入英文'
    },
    mobile: {// 验证手机号码  
        validator: function (value) {
            return /^(13|15|18)\d{9}$/i.test(value);
        },
        message: '手机号码格式不正确'
    },
    idcard: {// 验证身份证  
        validator: function (value) {
            return /^\d{15}(\d{2}[A-Za-z0-9])?$/i.test(value) || /^\d{18}(\d{2}[A-Za-z0-9])?$/i.test(value);
        },
        message: '身份证号码格式不正确'
  },
  tel: {//验证电话号码
    validator: function (value, param) {
      this._invalidMessage = '电话号码输入规则:</br>手机号:13/15/18xxxxxxxxx</br>电话号:区号-电话号-分机号</br>(分机号/区号可选填)';
      return /(^(\d{3}-|\d{4}-)?(\d{8}|\d{7})?(-\d{1,6})?$)|(^(?:13\d|15\d|18\d)-?\d{5}(\d{3}|\*{3})$)/.test(value);
    },
    message: '电话号码输入规则:</br>手机号:13/15/18xxxxxxxxx</br>电话号:区号-电话号-分机号</br>(分机号/区号可选填)'
  },
  number: {//验证数字
    validator: function (value, param) {
      this._invalidMessage = '请输入数字';
      return /^[0-9]+(.[0-9]{1,10})?$/.test(value);
    },
    message: '请输入数字'
  },
  money: {//验证金额
    validator: function (value, param) {
      this._invalidMessage = '请输入正确的金额';
      return (/^(0|-?[1-9])+(.[0-9]{1,2})?$/).test(value);
    },
    message: '请输入正确的金额'

  },
  mone: {//只允许整数或小数
    validator: function (value, param) {
      this._invalidMessage = '请输入整数或小数';
      return (/^(([1-9]\d*)|\d)(\.\d{1,5})?$/).test(value);
    },
    message: '请输入整数或小数'

  },
  integer: {//不允许小数或0
    validator: function (value, param) {
      this._invalidMessage = '请输入最小为1的整数';
      return /^[+]?[1-9]\d*$/.test(value);
    },
    message: '请输入最小为1的整数'
  },
  decimal: {
    validator: function (value) {
      this._invalidMessage = '不能小于等于[0]';
      return /^\s*(?=.*[1-9])\d*(?:\.\d{1,5})?\s*$/.test(value);
    },
    message: '不能小于等于[0]'
  },
  minLength: {//最小字符数
    validator: function (value, param) {
      this._invalidMessage = '至少输入' + param[0] + '个字';
      return value.length >= param[0];
    },
    message: '至少输入{0}个字'
  },
  maxLength: {//最多字符数
    validator: function (value, param) {
      this._invalidMessage = '最多' + param[0] + '个字';
      return value.length <= param[0];
    },
    message: '最多{0}个字'
  },
  noZero: {//判断非零的正数
    validator: function (value) {
      this._invalidMessage = '输入值不能为[0]';
      return /(^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$)|([1-9]\d*)/.test(value);
    },
    message: '输入值不能为[0]'
  },
  password: {
    validator: function (value) {
      this._invalidMessage = '只允许8/16/32位的字母数字组合';
      return /^[a-zA-Z0-9]{8}$|^[a-zA-Z0-9]{16}$|^[a-zA-Z0-9]{32}$/.test(value);
    },
    message: '只允许8/16/32位的字母数字组合！'
  },
  select: {
    validator: function (value, param) {
      if (param[0] == '0') { return false; }
      if (param[0] == '1') {
        param[1] = '输入项无效,请重新选择';
        this._invalidMessage = param[1];
        return false;
      }
      if ((value == '请选择' || value == '') && param[0] == 2) {
        param[1] = '该选项为必选项';
        this._invalidMessage = param[1];
        return false;
      } else {
        return true;
      }
    },
    message: '{1}'
  }
});
//date formatter
function dateformatter(value, row, index) {
   
    if (typeof value === "undefined" || value === null) {
        return null;
    }
    else if (moment(value).isValid()) {
        return moment(value).format('MM/DD/YYYY');
    } else {
        return value;
    }

}
//datebox datagrid editor
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
    },
    resize: function (target, width) {
      $(target).datebox('resize', width);
    }
  }
});
//datetimebox datagrid editor
$.extend($.fn.datagrid.defaults.editors, {
  datetimebox: {
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
        date = moment(value).format('YYYY-MM-DD HH:mm:00');
        $(target).datebox('setValue', date);
      } else {
        $(target).datebox('setValue', '');
      }
    },
    resize: function (target, width) {
      $(target).datebox('resize', width);
    }
  }
});
//datetime formatter
function datetimeformatter(value, row, index) {
    if (typeof value === "undefined" || value === null) {
        return null;
    }
    else if (moment(value).isValid()) {
        return moment(value).format('MM/DD/YYYY HH:mm:ss');
    } else {
        return value;
    }
}
//number formmater
function numberformatter(value) {
    return numeral(value).format('0,0.00');
}
//int formatter
function intformatter(value) {
    return numeral(value).format('0,0');
}
function isTrue(value) {
    if (typeof (value) === 'string') {
        value = value.trim().toLowerCase();
    }
    switch (true) {
        case true == value:
        case "true" == value:
        case value > 0:
        case "ok" == value:
        case "yes" == value:
        case "是" == value:
            return true;
        case "on" == value:
        case false == value:
        case "cancel" == value:
        case "null" == value:
        case "" == value:
        case "否" == value:
        case value <= 0:
            return false;
        default:
            return false;
    }
}
function booleanformatter(value, row, index) {
    if (isTrue(value)) {
        var checked = '<div class="smart-form"><label class="checkbox state-disabled">' +
            '<input type="checkbox" name="checkbox" checked="checked" disabled="disabled">' +
            '<i></i>&nbsp; </label></div>';

        return checked;
    } else {
        var unchecked = '<div class="smart-form"><label class="checkbox state-disabled">' +
            '<input type="checkbox" name="checkbox"   disabled="disabled">' +
            '<i></i>&nbsp; </label></div>';

        return unchecked;
    }


}


// easyui datagrid clientPaging
(function ($) {
    function pagerFilter(data) {
        if ($.isArray(data)) {    // is array
            data = {
                total: data.length,
                rows: data
            };
        }
        var target = this;
        var dg = $(target);
        var state = dg.data('datagrid');
        var opts = dg.datagrid('options');
        if (!state.allRows) {
            state.allRows = (data.rows);
        }
        if (!opts.remoteSort && opts.sortName) {
            var names = opts.sortName.split(',');
            var orders = opts.sortOrder.split(',');
            state.allRows.sort(function (r1, r2) {
                var r = 0;
                for (var i = 0; i < names.length; i++) {
                    var sn = names[i];
                    var so = orders[i];
                    var col = $(target).datagrid('getColumnOption', sn);
                    var sortFunc = col.sorter || function (a, b) {
                        return a == b ? 0 : (a > b ? 1 : -1);
                    };
                    r = sortFunc(r1[sn], r2[sn]) * (so == 'asc' ? 1 : -1);
                    if (r != 0) {
                        return r;
                    }
                }
                return r;
            });
        }
        var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
        var end = start + parseInt(opts.pageSize);
        data.rows = state.allRows.slice(start, end);
        return data;
    }

    var loadDataMethod = $.fn.datagrid.methods.loadData;
    var deleteRowMethod = $.fn.datagrid.methods.deleteRow;
    $.extend($.fn.datagrid.methods, {
        clientPaging: function (jq) {
            return jq.each(function () {
                var dg = $(this);
                var state = dg.data('datagrid');
                var opts = state.options;
                opts.loadFilter = pagerFilter;
                var onBeforeLoad = opts.onBeforeLoad;
                opts.onBeforeLoad = function (param) {
                    state.allRows = null;
                    return onBeforeLoad.call(this, param);
                };
                var pager = dg.datagrid('getPager');
                pager.pagination({
                    onSelectPage: function (pageNum, pageSize) {
                        opts.pageNumber = pageNum;
                        opts.pageSize = pageSize;
                        pager.pagination('refresh', {
                            pageNumber: pageNum,
                            pageSize: pageSize
                        });
                        dg.datagrid('loadData', state.allRows);
                    }
                });
                $(this).datagrid('loadData', state.data);
                if (opts.url) {
                    $(this).datagrid('reload');
                }
            });
        },
        loadData: function (jq, data) {
            jq.each(function () {
                $(this).data('datagrid').allRows = null;
            });
            return loadDataMethod.call($.fn.datagrid.methods, jq, data);
        },
        deleteRow: function (jq, index) {
            return jq.each(function () {
                var row = $(this).datagrid('getRows')[index];
                deleteRowMethod.call($.fn.datagrid.methods, $(this), index);
                var state = $(this).data('datagrid');
                if (state.options.loadFilter == pagerFilter) {
                    for (var i = 0; i < state.allRows.length; i++) {
                        if (state.allRows[i] == row) {
                            state.allRows.splice(i, 1);
                            break;
                        }
                    }
                    $(this).datagrid('loadData', state.allRows);
                }
            });
        },
        getAllRows: function (jq) {
            return jq.data('datagrid').allRows;
        }
    });
})(jQuery);
