using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Helpers;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{
   
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureHangfire(app);

            app.MapSignalR();






        }
    }
}
