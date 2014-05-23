using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;




namespace WebApplication1.Controllers
{
    public class GRP_BS_TAGController : Controller
    {
        private Entities db = new Entities();

        // GET: GRP_BS_TAG
        public ActionResult Index()
        {
            return View(db.GRP_BS_TAG.ToList());
        }

        // GET: GRP_BS_TAG/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GRP_BS_TAG gRP_BS_TAG = db.GRP_BS_TAG.Find(id);
            if (gRP_BS_TAG == null)
            {
                return HttpNotFound();
            }
            return View(gRP_BS_TAG);
        }

        // GET: GRP_BS_TAG/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GRP_BS_TAG/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TAG_ID,TAG_TITLE,TAG_COLOR,COMP_NO,DEP_NO,WHE_NO")] GRP_BS_TAG gRP_BS_TAG)
        {
            if (ModelState.IsValid)
            {
                db.GRP_BS_TAG.Add(gRP_BS_TAG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gRP_BS_TAG);
        }

        // GET: GRP_BS_TAG/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GRP_BS_TAG gRP_BS_TAG = db.GRP_BS_TAG.Find(id);
            if (gRP_BS_TAG == null)
            {
                return HttpNotFound();
            }
            return View(gRP_BS_TAG);
        }

        // POST: GRP_BS_TAG/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TAG_ID,TAG_TITLE,TAG_COLOR,COMP_NO,DEP_NO,WHE_NO")] GRP_BS_TAG gRP_BS_TAG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gRP_BS_TAG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gRP_BS_TAG);
        }

        // GET: GRP_BS_TAG/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GRP_BS_TAG gRP_BS_TAG = db.GRP_BS_TAG.Find(id);
            if (gRP_BS_TAG == null)
            {
                return HttpNotFound();
            }
            return View(gRP_BS_TAG);
        }

        // POST: GRP_BS_TAG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GRP_BS_TAG gRP_BS_TAG = db.GRP_BS_TAG.Find(id);
            db.GRP_BS_TAG.Remove(gRP_BS_TAG);
            db.SaveChanges();
            return RedirectToAction("Index");
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
