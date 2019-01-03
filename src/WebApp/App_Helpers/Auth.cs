using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LazyCache;
using WebApp.Models;

namespace WebApp
{
    public static class Auth
    {
        private static IAppCache cache = new CachingService();
        /// <summary>
        /// 获取当前登录用户名
        /// </summary>
        public static string CurrentUserName {

            get {
                var fullName = string.Empty;
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
        public static ApplicationUser CurrentApplicationUser
        {
            get
            {
                var username = HttpContext.Current.User.Identity.Name;
                var db = SqlHelper2.DatabaseFactory.CreateDatabase();
                var user = db.ExecuteDataReader<ApplicationUser>("select * from [dbo].[AspNetUsers] where [username]=@username", new { username },
                    dr =>
                    {
                        return new ApplicationUser()
                        {
                            AvatarsX120 = dr["AvatarsX120"].ToString(),
                            AvatarsX50 = dr["AvatarsX50"].ToString(),
                            AccountType = Convert.ToInt32(dr["AccountType"]),
                            CompanyCode = dr["CompanyCode"].ToString(),
                            CompanyName = dr["CompanyName"].ToString(),
                            Email = dr["Email"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            Gender = Convert.ToInt32(dr["Gender"].ToString()),
                            PhoneNumber = dr["PhoneNumber"].ToString(),
                            UserName = dr["UserName"].ToString()

                        };
                    });
                return user.FirstOrDefault();
            }
        }



        public static string GetUserIdByName(string username) => cache.GetOrAdd(username, () =>
             {
                 var db = SqlHelper2.DatabaseFactory.CreateDatabase();
                 var userid = db.ExecuteScalar<string>("select [id] from [dbo].[AspNetUsers] where [username]=@username", new { username });
                 return userid;
             });

        public static string GetAvatarsByName(string username, int size = 50) => cache.GetOrAdd(username, () =>
            {
                var db = SqlHelper2.DatabaseFactory.CreateDatabase();
                var photopath = "";
                if (size == 50)
                {
                    photopath = db.ExecuteScalar<string>("select [AvatarsX50] from [dbo].[AspNetUsers] where [username]=@username", new { username });
                }
                else
                {
                    photopath = db.ExecuteScalar<string>("select [AvatarsX120] from [dbo].[AspNetUsers] where [username]=@username", new { username });
                }

                return "/content/img/avatars/" + ( string.IsNullOrEmpty(photopath) ? "sunny" : photopath ) + ".png";
            });
    }
}