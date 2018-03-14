
namespace WebApp.Controllers
{
    using Microsoft.AspNet.Identity.Owin;
    using NB.Apps.EFMigrationsManager.CustomAttributes;
    using NB.Apps.EFMigrationsManager.Models;
    using NB.Apps.EFMigrationsManager.Services;
    using System.Net;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    [VerifyIsEFMigrationUpToDate(true)]
    public class EFMigrationsManagerController : Controller
    {
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        public ActionResult Publish(bool isRollback = false)
        {
            var _service = new EFMigrationService();
            //if(!_service.IsAuthorizedUser())
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            //}
            var vm = _service.LoadMigrationDetails(isRollback);
            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Publish(EFMigrationDetails entity)
        {
            var _service = new EFMigrationService();
            //if (!_service.IsAuthorizedUser())
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            //}
            if (entity == null || string.IsNullOrWhiteSpace(entity.TargetMigration))
                throw new System.ArgumentException("Invalid Parameters...");

            _service.Update(entity.TargetMigration);

            TempData["StatusMessage"] = entity.IsRollback ? "Database restored successfully." : "Database updated successfully.";
            return RedirectToAction("Publish", new {isRollback = entity.IsRollback});
        }

        public ActionResult DbMaintenance()
        {
            //if (Request.IsAuthenticated) {
            //    FormsAuthentication.SignOut();
            //    this.SignInManager.SignOut();

            //    // Second we clear the principal to ensure the user does not retain any authentication
            //    HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            //    // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
            //    // this clears the Request.IsAuthenticated flag since this triggers a new request
                
            //}
                return View();
        }
    }
}