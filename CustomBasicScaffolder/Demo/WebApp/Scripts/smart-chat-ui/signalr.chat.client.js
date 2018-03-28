 var IntervalVal;
        $(function () {

            // Declare a proxy to reference the hub.
            //$('#textarea-expand').emojioneArea({

            //});
          
            var chatHub = $.connection.chatHub;
       
            registerClientMethods(chatHub);
            // Start Hub
            $.connection.hub.start().done(function () {

                registerEvents(chatHub)

            });

            

            // Diplay Image Preview on File Upload 
            $(document).on('change', '#uploadfilebutton', function (e) {

                var tmppath = URL.createObjectURL(e.target.files[0]);
                $("#ImgDisp").attr('src', tmppath);

            });
            //
            $(document).on('dispatchprivatemessage',  function (e,id,user,msg) {
                //send private message server
                
                chatHub.server.sendPrivateMessage(id, msg);
              
            });


       

        });

        // Show new message Alert li.chat-users top-menu-invisible > a
        function ShowNewMessageAlert( ) {
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

        function registerEvents(chatHub) {

            var name = $('#show-shortcut  span').html();

            if (name.length > 0) {
                chatHub.server.connect(name);

            }


            // Clear Chat
            $('#btnClearChat').click(function () {

                var msg = $("#chat-body > ul").html();
              
                if (msg.length > 0) {
                    chatHub.server.clearTimeout();
                   
                    $("#chat-body > ul").animateCss('slideOutRight', function () {

                        $("#chat-body > ul > li").remove();
                    })
                        
                     

                }
            });

            // Send Button Click Event
            $('#btnSendMsg').click(function () {

                var msg = $("#textarea-expand").val();

                if (msg.length > 0) {

                    var userName = $('#show-shortcut  span').html();
                    //console.log(msg, userName);
                    var date = GetCurrentDateTime(new Date());

                    chatHub.server.sendMessageToAll(userName, msg, date);
                    $("#textarea-expand").val('');
                }
            });

            // Send Message on Enter Button
            $("#textarea-expand").keypress(function (e) {
                if (e.which == 13 && $("#subscription").prop("checked")) {
                    $('#btnSendMsg').click();
                    e.preventDefault();
                    return false;
                }
            });

        }

        function registerClientMethods(chatHub) {


            // Calls when user successfully logged in
            chatHub.client.onConnected = function (id, userName, allUsers, messages, times) {

                $('#hdId').val(id);
                $('#hdUserName').val(userName);
                $('#spanUser').html(userName);

                // Add All Users
                for (i = 0; i < allUsers.length; i++) {

                    AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName, allUsers[i].UserImage, allUsers[i].LoginTime);
                }

                // Add Existing Messages
                for (i = 0; i < messages.length; i++) {
                    AddMessage(messages[i].UserName, messages[i].Message, messages[i].Time, messages[i].UserImage);

                }
            }

            // On New User Connected
            chatHub.client.onNewUserConnected = function (id, name, UserImage, loginDate) {
                AddUser(chatHub, id, name, UserImage, loginDate);
            }

            // On User Disconnected
            chatHub.client.onUserDisconnected = function (id, userName) {
                //console.log('onUserDisconnected', id, userName);
                var dt = $('.display-users dl').find('a[data-chat-id="' + id + '"]').parent();

                $(dt).animate({ opacity: '0' }, 200, function () {
                    $(dt).animate({ height: '0px' }, 300, function () {
                        $(dt).remove();
                    });
                });

               
                

            }

            chatHub.client.messageReceived = function (userName, message, time, userimg) {

                AddMessage(userName, message, time, userimg);

                // Display Message Count and Notification
                var CurrUser1 = $('#hdUserName').val();
                if (CurrUser1 != userName) {
                    var diff = Math.round( moment().diff(moment(time, "YYYY-MM-DD HH:mm:ss")) / 1000);
                    console.log('Display Message Count and Notification',time,diff);
                    $.smallBox({
                        title: "有新消息",
                        content: "" + message + " 来至 【" + userName + "】 <i class='fa fa-clock-o'></i> <i>" + diff + " 秒前 .</i>",
                        color: "#739E73",
                        iconSmall: "fa fa-bell shake animated",
                        number: "1",
                        timeout: 6000
                    });
                     

                }
            }


            chatHub.client.sendPrivateMessage = function (fromUserId, fromUserName, toUserId, toUserName, message, UserImg, CurrentDateTime) {
                //console.log('receive sendPrivateMessage', {
                //    fromUserId: fromUserId,
                //    fromUserName: fromUserName,
                //    toUserId: toUserId,
                //    toUserName: toUserName,
                //    message: message,
                //    UserImg: UserImg,
                //    CurrentDateTime:CurrentDateTime
                //});
                $(document).trigger('receivingprivatemessage', [{
                    fromUserId: fromUserId,
                    fromUserName: fromUserName,
                    toUserId: toUserId,
                    toUserName: toUserName,
                    message: message,
                    UserImg: UserImg,
                    CurrentDateTime: CurrentDateTime
                }]);
                //ShowNewMessageAlert();
            }

        }

        function GetCurrentDateTime(now) {

            var localdate = moment(now).format("YYYY-MM-DD HH:mm:ss");

            return localdate;
        }

        function AddUser(chatHub, id, name, UserImage, date) {

            var userId = $('#hdId').val();
            var dl = $(".display-users dl");
            var role = "user";
            var me = "ME";
         
            
            var code, Clist;
            if (userId == id) {

                code = $('<dt class="animated fadeIn">' +
                    '  <a href="#" class="usr"' +
                    '     data-chat-id="' + id + '"' +
                    '     data-chat-fname="' + name + '"' +
                    '     data-chat-lname=" "' +
                    '     data-chat-status="online"' +
                    '     data-chat-alertmsg=" "' +
                    '     data-chat-alertshow="false"' +
                    '     rel="popover-hover"' +
                    '     data-placement="right"' +
                    '     data-html="true"' +
                    '     data-content="' +
                    '            <div class=\'usr-card\'>' +
                    '              <img src=\'' + UserImage + '\' alt=\'' + me +'\'>' +
                    '             <div class=\'usr-card-content\'>' +
                    '             <h3>' + me +'</h3 > ' +
                    '             <p>' + role + '</p>' +
                    '             </div>' +
                    '             </div>"' +
                    '             >' +
                    '         <i></i>' + me + '' +
                    '         </a>' +
                    '     </dt>' +
                    '     ');

                $(dl).prepend(code);
                 
            }
            else {

                code = $('<dt class="animated fadeIn">' +
                    '  <a href="#" class="usr"' +
                    '     data-chat-id="' + id + '"' +
                    '     data-chat-fname="' + name + '"' +
                    '     data-chat-lname=" "' +
                    '     data-chat-status="online"' +
                    '     data-chat-alertmsg=" "' +
                    '     data-chat-alertshow="false"' +
                    '     rel="popover-hover"' +
                    '     data-placement="right"' +
                    '     data-html="true"' +
                    '     data-content="' +
                    '            <div class=\'usr-card\'>' +
                    '              <img src=\'' + UserImage + '\' alt=\'' + name + '\'>' +
                    '             <div class=\'usr-card-content\'>' +
                    '             <h3>' + name + '</h3 > ' +
                    '             <p>' + role + '</p>' +
                    '             </div>' +
                    '             </div>"' +
                    '             >' +
                    '         <i></i>' + name + '' +
                    '         </a>' +
                    '     </dt>' +
                    '     ');


                $(dl).append(code);
                 
                $("a[data-chat-id='" + id + "']:not(.offline)").click(function (event, ui) {

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

            }

           
            $('a[rel=popover-hover]')
                .popover({
                    placement: 'right',
                    trigger: 'hover'
                })
            
               
      
            

        }

        function AddMessage(userName, message, time, userimg) {

            var CurrUser = $('#hdUserName').val();
            var Side = 'right';
            var TimeSide = 'left';

            if (CurrUser == userName) {
                Side = 'left';
                TimeSide = 'right';

            }

            var divChat = '<li class="message" >' +
                            '<img   src="' + userimg + '" class="online" alt="">' +
                            '<div class="message-text">' +
                                '<time>' +
                                  time   +
                                '</time>' +
                                '<a href="javascript:void(0);" class="username">' + userName + '</a > ' +
                                    message +
                              '</div>' +
                            '</li>';

            $('#chat-body > ul').append(divChat);
            //console.log(divChat);
            var height = $('#chat-body')[0].scrollHeight;
        
            //Apply Slim Scroll Bar in Group Chat Box
            $('#chat-body').slimScroll({
                scrollTo : height
            });

            ParseEmoji($('#chat-body'));

        }

         

function ParseEmoji(div) {
       $(div).emoticonize();
           
        }

function uploadComplete(sender, args) {
    var imgDisplay = $get("imgDisplay");
    imgDisplay.src = "images/loading.gif";
    imgDisplay.style.cssText = "";
    var img = new Image();
    img.onload = function () {
        imgDisplay.style.cssText = "Display:none;";
        imgDisplay.src = img.src;
    };

    imgDisplay.src = "<%# ResolveUrl(UploadFolderPath) %>" + args.get_fileName();
var chatHub = $.connection.chatHub;
var userName = $('#hdUserName').val();
var date = GetCurrentDateTime(new Date());
var sizeKB = (args.get_length() / 1024).toFixed(2);

var msg1;

if (IsValidateFile(args.get_fileName())) {
    if (IsImageFile(args.get_fileName())) {
        msg1 =
            '<div class="box-body">' +
            '<div class="attachment-block clearfix">' +
            '<a><img id="imgC" style="width:100px;" class="attachment-img" src="' + imgDisplay.src + '" alt="Attachment Image"></a>' +
            '<div class="attachment-pushed"> ' +
            '<h4 class="attachment-heading"><i class="fa fa-image">  ' + args.get_fileName() + ' </i></h4> <br />' +
            '<div id="at" class="attachment-text"> Dimensions : ' + imgDisplay.height + 'x' + imgDisplay.width + ', Type: ' + args.get_contentType() +

            '</div>' +
            '</div>' +
            '</div>' +
            '<a id="btnDownload" href="' + imgDisplay.src + '" class="btn btn-default btn-xs" download="' + args.get_fileName() + '"><i class="fa fa fa-download"></i> Download</a>' +
            '<button type="button" id="ShowModelImg"  value="' + imgDisplay.src + '"  class="btn btn-default btn-xs"><i class="fa fa-camera"></i> View</button>' +
            '<span class="pull-right text-muted">File Size : ' + sizeKB + ' Kb</span>' +
            '</div>';
    }
    else {

        msg1 =
            '<div class="box-body">' +
            '<div class="attachment-block clearfix">' +
            '<a><img id="imgC" style="width:100px;" class="attachment-img" src="images/file-icon.png" alt="Attachment Image"></a>' +
            '<div class="attachment-pushed"> ' +
            '<h4 class="attachment-heading"><i class="fa fa-file-o">  ' + args.get_fileName() + ' </i></h4> <br />' +
            '<div id="at" class="attachment-text"> Type: ' + args.get_contentType() +

            '</div>' +
            '</div>' +
            '</div>' +
            '<a id="btnDownload" href="' + imgDisplay.src + '" class="btn btn-default btn-xs" download="' + args.get_fileName() + '"><i class="fa fa fa-download"></i> Download</a>' +
            '<a href="' + imgDisplay.src + '" target="_blank" class="btn btn-default btn-xs"><i class="fa fa-camera"></i> View</a>' +
            '<span class="pull-right text-muted">File Size : ' + sizeKB + ' Kb</span>' +
            '</div>';
    }
    chatHub.server.sendMessageToAll(userName, msg1, date);

}


imgDisplay.src = '';
}

        function uploadStarted() {
            $get("imgDisplay").style.display = "none";
        }

$(document).on('click', '#ShowModelImg', function () {
    $get("ImgModal").src = this.value;
    $('#ShowPictureModal').modal('show');
});

function IsValidateFile(fileF) {
    var allowedFiles = [".doc", ".docx", ".pdf", ".txt", ".xlsx", ".xls", ".png", ".jpg", ".gif"];
    var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
    if (!regex.test(fileF.toLowerCase())) {
        alert("Please upload files having extensions: " + allowedFiles.join(', ') + " only.");
        return false;
    }
    return true;
}

function IsImageFile(fileF) {
    var ImageFiles = [".png", ".jpg", ".gif"];
    var regex = new RegExp("(" + ImageFiles.join('|') + ")$");
    if (!regex.test(fileF.toLowerCase())) {
        return false;
    }
    return true;
}
