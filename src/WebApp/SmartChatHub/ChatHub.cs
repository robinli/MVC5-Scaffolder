using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApp.SmartChatHub
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private static List<Users> connectedUsers = new List<Users>();
        private static List<Messages> currentMessage = new List<Messages>();
        //ConnClass ConnC = new ConnClass();

        public void Connect(string userName)
        {
            var id = this.Context.ConnectionId;


            if (connectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                var UserImg = GetUserImage(userName);
                var logintime = DateTime.Now.ToString();

                connectedUsers.Add(new Users { ConnectionId = id, UserName = userName, UserImage = UserImg, LoginTime = logintime });
                // send to caller
                Clients.Caller.onConnected(id, userName, connectedUsers, currentMessage);

                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected(id, userName, UserImg, logintime);
            }
        }

        public void SendMessageToAll(string userName, string message, string time)
        {
            var UserImg = GetUserImage(userName);
            // store last 100 messages in cache
            AddMessageinCache(userName, message, time, UserImg);

            // Broad cast message
            Clients.All.messageReceived(userName, message, time, UserImg);

        }

        private void AddMessageinCache(string userName, string message, string time, string UserImg)
        {
            currentMessage.Add(new Messages { UserName = userName, Message = message, Time = time, UserImage = UserImg });

            if (currentMessage.Count > 100)
            {
                currentMessage.RemoveAt(0);
            }

        }

        // Clear Chat History
        public void ClearTimeout() => currentMessage.Clear();

        public string GetUserImage(string username)
        {
            var RetimgName = "/content/img/avatars/male.png";
            try
            {
                //string query = "select Photo from tbl_Users where UserName='" + username + "'";
                var ImageName = Auth.GetAvatarsByName(username);

                if (ImageName != "")
                {
                    RetimgName = ImageName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RetimgName;
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = connectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                connectedUsers.Remove(item);

                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);

            }
            return base.OnDisconnected(stopCalled);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {

            var fromUserId = Context.ConnectionId;

            var toUser = connectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = connectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                var CurrentDateTime = DateTime.Now.ToString();
                var UserImg = GetUserImage(fromUser.UserName);
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName,toUserId, toUser.UserName, message, UserImg, CurrentDateTime);

                // send to caller user
                Clients.Caller.sendPrivateMessage(fromUserId, fromUser.UserName,toUserId, toUser.UserName, message, UserImg, CurrentDateTime);
            }

        }
    }


    public class Users
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string LoginTime { get; set; }
    }

    public class Messages
    {

        public string UserName { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }

        public string UserImage { get; set; }

    }
}