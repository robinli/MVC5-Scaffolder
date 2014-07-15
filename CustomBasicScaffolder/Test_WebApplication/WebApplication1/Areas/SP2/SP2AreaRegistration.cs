using System.Web.Mvc;

namespace WebApplication1.Areas.SP2
{
    public class SP2AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SP2";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SP2_default",
                "SP2/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}