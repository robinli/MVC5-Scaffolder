using System.Web.Mvc;

namespace WebApplication1.Areas.GRP
{
    public class GRPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GRP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GRP_default",
                "GRP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}