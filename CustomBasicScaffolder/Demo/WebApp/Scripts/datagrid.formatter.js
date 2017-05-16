//日志查询相关
function groupformatter(value, row, index) {
    if (value === null || value === '') {
        return "";
    }
    for (var i = 0; i < MessageGroup.length; i++) {
        if (MessageGroup[i].value.toString() === value.toString()) {
            return MessageGroup[i].text;
        }
    }
    return value;
}

function typeformatter(value, row, index) {
    if (value === null || value === '') {
        return "";
    }
    for (var i = 0; i < MessageType.length; i++) {
        if (MessageType[i].value.toString() === value.toString()) {
            return MessageType[i].text;
        }
    }
    return value;
}

function newformatter(value, row, index) {
    if (value === null || value === '') {
        return "";
    }
    for (var i = 0; i < MessageNew.length; i++) {
        if (MessageNew[i].value.toString() === value.toString()) {
            return MessageNew[i].text;
        }
    }
    return value;
}

function noticeformatter(value, row, index) {
    if (value === null || value === '') {
        return "";
    }
    for (var i = 0; i < MessageNotice.length; i++) {
        if (MessageNotice[i].value.toString() === value.toString()) {
            return MessageNotice[i].text;
        }
    }
    return value;
}
//日志查询相关

function Categoryformatter(value, row, index) {
    if (value === null || value === '') {
        return "";
    }
    for (var i = 0; i < Category.length; i++) {
        if (Category[i].value.toString() === value.toString()) {
            return Category[i].text;
        }
    }
    return "";
}

 function requireStatus (value, row, index) {
     switch (value) {
         case "0":
             return '待审核';
         case "1":
             return '已审核';
         case "2":
             return '审核不通过';
         default:
             return value;
     }
}
function planStatus(value, row, index) {
    switch (value) {
        case "0":
            return '待审核';
        case "1":
            return '已审核';
        case "2":
            return '审核不通过';
        default:
            return value;
    }
}

function requireType(value, row, index) {
    switch (value) {
        case "1":
            return '需求';
        case "2":
            return '累计';
        default:
            return value;
    }
}
function planType(value, row, index) {
    switch (value) {
        case "1":
            return '投入';
        case "2":
            return '投入累计';
        case "3":
            return '产出';
        case "4":
            return '产出累计';
        case "5":
            return '需求';
        case "6":
            return '需求累计';
        default:
            return value;
    }
}
function CompareType(value, row, index) {
    switch (value) {
        case "1":
            return '计划产出';
        case "2":
            return '计划累计';
        case "3":
            return '实际产出';
        case "4":
            return '产出累计';
        case "5":
            return '日差异';
        case "6":
            return '累计差异';
        default:
            return value;
    }
}

function CompareShipQtyType(value, row, index) {
    switch (value) {
        case "1":
            return '计划出货';
        case "2":
            return '出货累计';
        case "3":
            return '实际出货';
        case "4":
            return '出货累计';
        case "5":
            return '日差异';
        case "6":
            return '累计差异';
        default:
            return value;
    }
}



function Formatter(value, row, index) {
    if (value == undefined)
        return null;
    if (value === null || value === '') {
        return value;
    }
    var enumdata = {};
    //console.log(this.field);
    if (this.field == "ComponentMakeFlag")
        enumdata = eval("MakeFlag");
    else if (this.field == "ComponentCategory")
        enumdata = eval("Category");
    else
      enumdata = eval(this.field);
    var strvalue = value.toString();
    for (var i = 0; i < enumdata.length; i++) {
        if (enumdata[i].value.toString() === strvalue) {
            return enumdata[i].text;
        }
    }
    return value;

}






function dateformatter(value, row, index) {
    //console.log(value);
    if (value == typeof (undefined))
        return null;
    if (value == null)
        return null;
    else {

        if (moment(value).isValid()) {

            return moment(value).format('MM/DD/YYYY');

        }
    }
}
function datetimeformatter(value, row, index) {


    if (value == typeof (undefined))
        return null;
    if (value == null)
        return null;
    else
        return moment(value).format('MM/DD/YYYY HH:mm:ss');
}


jQuery.extend({
    dateNow: function () {
        //console.log(new Date());
        return moment(new Date()).format('MM/DD/YYYY');
    }
});