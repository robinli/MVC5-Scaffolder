#region Using

using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace SmartAdminMvc
{
    public class DashRouteHandler : MvcRouteHandler
    {
        /// <summary>
        ///     Custom route handler that removes any dashes from the route before handling it.
        /// </summary>
        /// <param name="requestContext">The context of the given (current) request.</param>
        /// <returns></returns>
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var routeValues = requestContext.RouteData.Values;

            routeValues["action"] = routeValues["action"].UnDash();
            routeValues["controller"] = routeValues["controller"].UnDash();

            return base.GetHttpHandler(requestContext);
        }
    }
}