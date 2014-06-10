using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace WebApplication1.Areas.Groupage.Controllers
{
    public class GroupageAdminController : Controller
    {
        private SchoolDBEntities1 db = new SchoolDBEntities1();

        // GET: GroupageAdmin
        public ActionResult CustomergradeIndex()
        {
            return View(db.GRP_BS_CUSTGRADE.ToList());
        }

        /*
        // GET: GroupageAdmin/CustomergradeDetails/5
        public ActionResult CustomergradeDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GRP_BS_CUSTGRADE gRP_BS_CUSTGRADE = db.GRP_BS_CUSTGRADE.Find(id);
            if (gRP_BS_CUSTGRADE == null)
            {
                return HttpNotFound();
            }
            return View(gRP_BS_CUSTGRADE);
        }
        */

        // GET: GroupageAdmin/CustomergradeCreate
        public ActionResult CustomergradeCreate()
        {
            return View();
        }

        // POST: GroupageAdmin/CustomergradeCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomergradeCreate([Bind(Include = "GRADECO,GRADENA,DISCOUNT_PRICE")] GRP_BS_CUSTGRADE gRP_BS_CUSTGRADE)
        {
            if (ModelState.IsValid)
            {
                db.GRP_BS_CUSTGRADE.Add(gRP_BS_CUSTGRADE);
                db.SaveChanges();
                DisplaySuccessMessage("Has append a GRP_BS_CUSTGRADE record");
                return RedirectToAction("CustomergradeIndex");
            }

            DisplayErrorMessage();
            return View(gRP_BS_CUSTGRADE);
        }

        // GET: GroupageAdmin/CustomergradeEdit/5
        public ActionResult CustomergradeEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GRP_BS_CUSTGRADE gRP_BS_CUSTGRADE = db.GRP_BS_CUSTGRADE.Find(id);
            if (gRP_BS_CUSTGRADE == null)
            {
                return HttpNotFound();
            }
            return View(gRP_BS_CUSTGRADE);
        }

        // POST: GroupageAdminCustomergrade/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomergradeEdit([Bind(Include = "GRADECO,GRADENA,DISCOUNT_PRICE")] GRP_BS_CUSTGRADE gRP_BS_CUSTGRADE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gRP_BS_CUSTGRADE).State = EntityState.Modified;
                db.SaveChanges();
                DisplaySuccessMessage("Has update a GRP_BS_CUSTGRADE record");
                return RedirectToAction("CustomergradeIndex");
            }
            DisplayErrorMessage();
            return View(gRP_BS_CUSTGRADE);
        }

        // GET: GroupageAdmin/CustomergradeDelete/5
        public ActionResult CustomergradeDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GRP_BS_CUSTGRADE gRP_BS_CUSTGRADE = db.GRP_BS_CUSTGRADE.Find(id);
            if (gRP_BS_CUSTGRADE == null)
            {
                return HttpNotFound();
            }
            return View(gRP_BS_CUSTGRADE);
        }

        // POST: GroupageAdmin/CustomergradeDelete/5
        [HttpPost, ActionName("CustomergradeDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult CustomergradeDeleteConfirmed(string id)
        {
            GRP_BS_CUSTGRADE gRP_BS_CUSTGRADE = db.GRP_BS_CUSTGRADE.Find(id);
            db.GRP_BS_CUSTGRADE.Remove(gRP_BS_CUSTGRADE);
            db.SaveChanges();
            DisplaySuccessMessage("Has delete a GRP_BS_CUSTGRADE record");
            return RedirectToAction("CustomergradeIndex");
        }

        private void DisplaySuccessMessage(string msgText)
        {
            TempData["SuccessMessage"] = msgText;
        }

        private void DisplayErrorMessage()
        {
            TempData["ErrorMessage"] = "Save changes was unsuccessful.";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
