using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.App_Start
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
    }
}