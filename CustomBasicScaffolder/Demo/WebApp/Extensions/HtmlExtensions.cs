using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using WebApp.Models;
namespace WebApp.Extensions
{
    public static class CacheExtensions
    {
        static object sync = new object();

        public static T Data<T>(this Cache cache, string cacheKey, int expirationSeconds, Func<T> method)
        {
            var data = cache == null ? default(T) : (T)cache[cacheKey];
            if (data == null)
            {
                data = method();

                if (expirationSeconds > 0 && data != null)
                {
                    lock (sync)
                    {
                        cache.Insert(cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), Cache.NoSlidingExpiration);
                    }
                }
            }
            return data;
        }
    }
    public static class HMTLHelperExtensions
    {

        public static bool IsAuthorize(this HtmlHelper html, string menu)
        {
            string userid = html.ViewContext.HttpContext.User.Identity.GetUserId();
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            string key = userid + currentAction + currentController;
            // var data= html.ViewContext.HttpContext.Cache.Data<IList<WebApp.Models.RoleMenu>>(key,10000,()=>{
            var rolemanager = html.ViewContext.HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            var usermanager = html.ViewContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var roles = usermanager.GetRoles(userid);
            StoreContext db = new StoreContext();
            var authorize = db.RoleMenus.Where(x => roles.Contains(x.RoleName) && x.MenuItem.Action == currentAction && x.MenuItem.Controller == currentController).ToList();
            //return authorize;
            //});
            var data = authorize;
            if (menu == "Create")
            {
                return data.Where(x => x.Create == true).Any();
            }
            if (menu == "Edit")
            {
                return data.Where(x => x.Edit == true).Any();
            }
            if (menu == "Delete")
            {
                return data.Where(x => x.Delete == true).Any();
            }
            if (menu == "Import")
            {
                return data.Where(x => x.Import == true).Any();
            }




            return false;
        }
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
            {
                controller = currentController;
            }

            if (String.IsNullOrEmpty(action))
                action = currentAction;
            var ctrs = controller.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ctrs.Length > 1)
            {
                return ctrs.Contains(currentController) ? cssClass : String.Empty;
            }
            return controller == currentController && action == currentAction ? cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

    }

    public static class HtmlExtensions
    {
        public static MvcHtmlString ActionButton(this HtmlHelper html, string linkText, string action, string controllerName, string iconClass)
        {
            //<a href="/@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText"><i class="@lLink.IconClass"></i><span class="">@lLink.LinkText</span></a>
            var lURL = new UrlHelper(html.ViewContext.RequestContext);

            // build the <span class="">@lLink.LinkText</span> tag
            var lSpanBuilder = new TagBuilder("span");
            lSpanBuilder.MergeAttribute("class", "");
            lSpanBuilder.SetInnerText(linkText);
            string lSpanHtml = lSpanBuilder.ToString(TagRenderMode.Normal);

            // build the <i class="@lLink.IconClass"></i> tag
            var lIconBuilder = new TagBuilder("i");
            lIconBuilder.MergeAttribute("class", iconClass);
            string lIconHtml = lIconBuilder.ToString(TagRenderMode.Normal);

            // build the <a href="@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText">...</a> tag
            var lAnchorBuilder = new TagBuilder("a");
            lAnchorBuilder.MergeAttribute("href", lURL.Action(action, controllerName));
            lAnchorBuilder.InnerHtml = lIconHtml + lSpanHtml; // include the <i> and <span> tags inside
            string lAnchorHtml = lAnchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(lAnchorHtml);
        }
    }
}