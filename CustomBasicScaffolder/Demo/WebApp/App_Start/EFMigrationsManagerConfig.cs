[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApp.EFMigrationsManagerConfig), "Register")]
namespace WebApp
{
	using WebApp.Controllers;
    using NB.Apps.EFMigrationsManager.Settings;
    using System;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NB.Apps.EFMigrationsManager.Services;
    using NB.Apps.EFMigrationsManager.CustomAttributes;

    public static class EFMigrationsManagerConfig
    {
        #region Methods
        public static void Register()
        {
            //Call the SetEFConfiguration method and pass the EF Migration Configuration instance
            //(EFConfiguration is a class which inherits from the DbMigrationsConfiguration class and available in EFData/Models project).
            EFMigrationsManagerSettings.SetEFConfiguration(new Migrations.Configuration());

            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new VerifyIsEFMigrationUpToDateAttribute());
        }

         internal static bool HandleEFMigrationException(Exception ex, HttpServerUtility server, HttpResponse response, HttpContext context)
        {
            var _service = new EFMigrationService();
            var isEfException = (
                     ex.Message.ToLower().Contains("context has changed") ||
                     ex.Message.ToLower().Contains("contains no mapped tables") ||
                     (ex is System.Data.Entity.Core.EntityCommandExecutionException && ex.InnerException?.Message?.ToLower()?.Contains("invalid column name") == true) ||
                      ex is SqlException
                );

            if (isEfException && !_service.IsLatestVersion())
            {
                server.ClearError();

                var routeData = new RouteData();
                routeData.Values.Add("controller", "EFMigrationsManager");

                if (_service.IsAuthorizedUser())
                {
                    routeData.Values.Add("action", "publish");
                }
                else
                {
                    routeData.Values.Add("action", "DbMaintenance");
                }
                HttpContext.Current.Response.RedirectToRoute(routeData.Values);
                return true;
            }
            return false;
        }
        #endregion
    }

    
}