
namespace $rootnamespace$.Controllers
{
    using NB.Apps.EFMigrationsManager.CustomAttributes;
    using NB.Apps.EFMigrationsManager.Models;
    using NB.Apps.EFMigrationsManager.Services;
    using System.Net;
    using System.Web.Mvc;
    

    [VerifyIsEFMigrationUpToDate(true)]
    public class EFMigrationsManagerController : Controller
    {
        public ActionResult Publish(bool isRollback = false)
        {
            var _service = new EFMigrationService();
            if(!_service.IsAuthorizedUser())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            var vm = _service.LoadMigrationDetails(isRollback);
            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Publish(EFMigrationDetails entity)
        {
            var _service = new EFMigrationService();
            if (!_service.IsAuthorizedUser())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (entity == null || string.IsNullOrWhiteSpace(entity.TargetMigration))
                throw new System.ArgumentException("Invalid Parameters...");

            _service.Update(entity.TargetMigration);

            TempData["StatusMessage"] = entity.IsRollback ? "Database restored successfully." : "Database updated successfully.";
            return RedirectToAction("Publish", new {isRollback = entity.IsRollback});
        }

        public ActionResult DbMaintenance()
        {
            return View();
        }
    }
}