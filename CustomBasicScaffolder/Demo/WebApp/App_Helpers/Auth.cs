using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public static class Auth
    {
        /// <summary>
        /// 获取当前登录用户名
        /// </summary>
        public static string CurrentUserName {

            get {
                string fullName = string.Empty;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    fullName = HttpContext.Current.User.Identity.Name;
                }
                else {
                    fullName = "无名氏";
                }


                return fullName;
            }
        }

        public static string GetUserIdByName(string username) {
          var db=  SqlHelper2.DatabaseFactory.CreateDatabase();
            var userid = db.ExecuteScalar<string>("select [id] from [dbo].[AspNetUsers] where [username]=@username", new { username });
            return userid;
        }
        public static string GetAvatarsByName(string username, int size = 50) {
            var db = SqlHelper2.DatabaseFactory.CreateDatabase();
            var photopath = "";
            if (size==50)
                photopath = db.ExecuteScalar<string>("select [AvatarsX50] from [dbo].[AspNetUsers] where [username]=@username", new { username });
            else
                photopath = db.ExecuteScalar<string>("select [AvatarsX120] from [dbo].[AspNetUsers] where [username]=@username", new { username });
            return photopath;
        }
    }
}