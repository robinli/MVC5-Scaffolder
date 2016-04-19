using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebApp.Extensions
{

    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
            {
                controller = currentController;
            }

            if (String.IsNullOrEmpty(action))
                action = currentAction;
            var ctrs = controller.Split(new char[] { ',' },  StringSplitOptions.RemoveEmptyEntries);
            if (ctrs.Length > 1)
            {
                return ctrs.Contains( currentController)  ? cssClass : String.Empty;
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