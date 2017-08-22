using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using WebApp.Models;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;

namespace WebApp
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureHangfire(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(
                    "DefaultConnection",
                    new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) });

            

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            //每10分钟执行一个方法
            RecurringJob.AddOrUpdate(() => ExecuteProcess(), Cron.MinuteInterval(10));
        }

        public void ExecuteProcess() {
            Console.WriteLine("{ DateTime.Now }:do something......");
        }
    }
}