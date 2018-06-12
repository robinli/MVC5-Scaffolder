using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Test_WebClient.Startup))]
namespace Test_WebClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
