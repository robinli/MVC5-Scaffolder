using System.Web.Mvc;

namespace WebApplication1.Areas.NEW
{
    public class NEWAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NEW";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NEW_default",
                "NEW/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}