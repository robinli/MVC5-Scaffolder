using System.Web.Mvc;

namespace WebApplication1.Areas.CODGN
{
    public class CODGNAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CODGN";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CODGN_default",
                "CODGN/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}