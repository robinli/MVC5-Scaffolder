/*
 * SMART CHAT ENGINE (EXTENTION)
 * Copyright (c) 2013 Wen Pu
 * Modified by MyOrange
 * All modifications made are hereby copyright (c) 2014-2015 MyOrange
 */

// clears the variable if left blank
// Need this to make IE happy
// see http://soledadpenades.com/2007/05/17/arrayindexof-in-internet-explorer/
/*if (!Array.indexOf) {
    Array.prototype.indexOf = function (obj) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == obj) {
                return i;
            }
        }
        return -1;
    }
}*/

var chatboxManager = function() {

    var init = function(options) {
        $.extend(chatbox_config, options);
    };


    var delBox = function(id) {
        // TODO
    };

    var getNextOffset = function() {
        return (chatbox_config.width + chatbox_config.gap) * showList.length;
    };

    var boxClosedCallback = function(id) {
        // close button in the titlebar is clicked
        var idx = showList.indexOf(id);
        if (idx != -1) {
            showList.splice(idx, 1);
            diff = chatbox_config.width + chatbox_config.gap;
            for (var i = idx; i < showList.length; i++) {
                offset = $("#" + showList[i]).chatbox("option", "offset");
                $("#" + showList[i]).chatbox("option", "offset", offset - diff);
            }
        } else {
            alert("NOTE: Id missing from array: " + id);
        }
    };

    // caller should guarantee the uniqueness of id
    var addBox = function (id, options) {
       
        var idx1 = showList.indexOf(id);
        var idx2 = boxList.indexOf(id);
        if (idx1 != -1) {
            // found one in show box, do nothing
        } else if (idx2 != -1) {
            // exists, but hidden
            // show it and put it back to showList
            $("#" + id).chatbox("option", "offset", getNextOffset());
            var manager = $("#" + id).chatbox("option", "boxManager");
            manager.toggleBox();
            showList.push(id);
        } else {
            var el = document.createElement("div");
            el.setAttribute("id", id);
            $(el).chatbox({
                id: id,
                fromid:options.fromid,
                user: options,
                title: '<i title="' + options.status + '"></i>' + options.first_name + " " + options.last_name,
                hidden: false,
                offset: getNextOffset(),
                width: chatbox_config.width,
                status: options.status,
                alertmsg: options.alertmsg,
                alertshow: options.alertshow,
                messageSent: dispatch,
                boxClosed: boxClosedCallback
            });
            boxList.push(id);
            showList.push(id);
            nameList.push(options.first_name);
        }
    };

    var messageSentCallback = function(id, user, msg,fromid) {
        var idx = boxList.indexOf(id);
        chatbox_config.messageSent(nameList[idx], msg,fromid);
    };

    // not used in demo
    var dispatch = function (id, user, msg,fromid) {
        //if ($("#chatlog").doesExist()) {
        //    $("#chatlog").append("You said to <b>" + user.first_name + " " + user.last_name + ":</b> " + msg + "<br/>")
        //        .effect("highlight", {}, 500);;
        //}
        var userid = $('#hdId').val(),
            username = $('#hdUserName').val();
        var currentuser = { id: userid, username: username };
        $("#" + id).chatbox("option", "boxManager").addMsg("Me", msg);
        //send private message to server side signlar chat.client.js subscribe this event
     
        $(document).trigger('dispatchprivatemessage', [id,user, msg])
        HideNewMessageAlert();
    };
    //subscribe for chat.client.js receiving private message to me
    $(document).on('receivingprivatemessage', function (e, msg) {
        //console.log('subscribe receivingprivatemessage', e, msg);
        var userid = $('#hdId').val(),
            username = $('#hdUserName').val();
        if (userid != msg.fromUserId) {
            
            addBox(msg.fromUserId, {
                id: msg.fromUserId,
                fromid: msg.toUserId,
                title: "username" + msg.fromUserId,
                first_name: msg.fromUserName,
                last_name: '',
                status: 'online',
                alertmsg: '',
                alertshow: false
                //you can add your own options too
            });
            ShowNewMessageAlert();
            $("#" + msg.fromUserId).chatbox("option", "boxManager").addMsg(msg.fromUserName, msg.message);
        }
    })

    // Show new message Alert li.chat-users top-menu-invisible > a
    function ShowNewMessageAlert() {
        var tag = $('li.chat-users > a > i')
        $(tag).append('<em class="bg-color-pink flash animated">!</em>');
        //console.log(tag);
    }
    // hide new message Alert li.chat-users top-menu-invisible > a
    function HideNewMessageAlert() {
        var tag = $('li.chat-users > a > i > em')
        if (tag.length > 0) {
            $(tag).remove();
        }
        //console.log(tag);
    }

    return {
        init: init,
        addBox: addBox,
        delBox: delBox,
        dispatch: dispatch
    };
}();


$("a[data-chat-id]:not(.offline)").click(function(event, ui) {

    var $this = $(this),
        temp_chat_id = $this.attr("data-chat-id"),
        fname = $this.attr("data-chat-fname"),
        lname = $this.attr("data-chat-lname"),
        status = $this.attr("data-chat-status") || "online",
        alertmsg = $this.attr("data-chat-alertmsg"),
        alertshow = $this.attr("data-chat-alertshow") || false,
        fromid = $('#hdId').val();
        


    chatboxManager.addBox(temp_chat_id,
        {
            // dest:"dest" + counter, 
            // not used in demo
            id: temp_chat_id,
            fromid: fromid,
            title: "username" + temp_chat_id,
            first_name: fname,
            last_name: lname,
            status: status,
            alertmsg: alertmsg,
            alertshow: alertshow
            //you can add your own options too
        });

    event.preventDefault();

});
