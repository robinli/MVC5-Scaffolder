using System.Web.Mvc;

namespace WebApplication1.Areas.kim
{
    public class kimAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "kim";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "kim_default",
                "kim/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}