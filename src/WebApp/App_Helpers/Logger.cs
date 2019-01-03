using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlHelper2;
using WebApp.App_Start;
using WebApp.Models;

namespace WebApp
{
    public static class Logger
    {
        public static void Message(string extensionKey, string method, string content, string tags = "")
        {
            DatabaseFactory.CreateDatabase().ExecuteSPNonQuery("[dbo].[SP_InsertMessages]",
                new
                {
                    Group = MessageGroup.Operator,
                    ExtensionKey1 = extensionKey,
                    Type = MessageType.Error,
                    Content = content,
                    Tags = tags,
                    Method = method,
                    User = Auth.CurrentUserName

                }
                );
        }

        public static void Alert(string extensionKey, string method, string content, string tags = "")
        {
            DatabaseFactory.CreateDatabase().ExecuteSPNonQuery("[dbo].[SP_InsertMessages]",
                new
                {
                    Group = MessageGroup.Operator,
                    ExtensionKey1 = extensionKey,
                    Type = MessageType.Alert,
                    Content = content,
                    Tags = tags,
                    Method = method,
                    User = Auth.CurrentUserName
                }
                );
        }


        public static void Error(string extensionKey, string method, string exceptionMessage, string extensionKey1 = "", string tags = "", string stackTrace = "")
        {
            DatabaseFactory.CreateDatabase().ExecuteSPNonQuery("[dbo].[SP_InsertMessages]",
            new
            {
                Group = MessageGroup.Operator,
                ExtensionKey1 = extensionKey,
                Type = MessageType.Error,
                Content = exceptionMessage,
                Tags = tags,
                Method = method,
                StackTrace = stackTrace,
                ExtensionKey2= extensionKey1,
                User = Auth.CurrentUserName

            }
            );
        }

    }
}