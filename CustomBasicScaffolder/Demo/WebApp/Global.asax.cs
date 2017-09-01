using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApp.App_Start;
using WebApp.Models;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //BundleTable.EnableOptimizations = false; 
            //ModelBinders.Binders.DefaultBinder = new EnumModelBinder();


    


            
           
        }



        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var message = exception.Message;
            var stackTrace = exception.StackTrace;
            var Source = exception.Source;

            HttpContext httpContext = HttpContext.Current;
            var controllerName = string.Empty;
            var actionName = string.Empty;
            var messageTag = string.Empty;
            var code = string.Empty;
            HttpException httpException = exception as HttpException;
            if (httpException != null)
            {
                code = $"{httpException.ErrorCode}:{httpException.WebEventCode}";
            }
            if (httpContext != null)
            {
                var requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                controllerName = requestContext.RouteData.GetRequiredString("controller");
                actionName = requestContext.RouteData.GetRequiredString("action");



                /* when the request is ajax the system can automatically handle a mistake with a JSON response. then overwrites the default response */
                if (requestContext.HttpContext.Request.IsAjaxRequest())
                {
                    messageTag = "Ajax";

                    var factory = ControllerBuilder.Current.GetControllerFactory();
                    var controller = factory.CreateController(requestContext, controllerName);
                    var controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);


                }
                else
                {
                    messageTag = "Mvc";

                }

            }

            DatabaseFactory.CreateDatabase().ExecuteSPNonQuery("[dbo].[SP_InsertMessages]",
                new
                {
                    Group = MessageGroup.System,
                    ExtensionKey1 = Source,
                    Type = MessageType.Error,
                    Content = message,
                    Tags = messageTag,
                    Code = code,
                    Method = $"{controllerName}/{actionName}",
                    StackTrace = stackTrace,
                    User = Auth.CurrentUserName
                }
                );
        }
    }
}
