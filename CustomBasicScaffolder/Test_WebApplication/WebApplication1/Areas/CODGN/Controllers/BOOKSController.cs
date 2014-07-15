using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;

namespace WebApplication1.Areas.CODGN.Controllers
{
    public class BOOKSController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        // GET: BOOKS/BOOKSIndex
        public ActionResult BOOKSIndex()
        {
            return View(db.BOOKS.ToList());
        }

        /*
        // GET: BOOKS/BOOKSDetails/5
        public ActionResult BOOKSDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOKS bOOKS = db.BOOKS.Find(id);
            if (bOOKS == null)
            {
                return HttpNotFound();
            }
            return View(bOOKS);
        }
        */

        // GET: BOOKS/BOOKSCreate
        public ActionResult BOOKSCreate()
        {
            return View();
        }

        // POST: BOOKS/BOOKSCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BOOKSCreate([Bind(Include = "ID,BOOKNAME,AUTHOR,PUBLISH_UTC,VERSION_NUM,VERSION_NUM2,LIST_PRICE,ISOK")] BOOKS bOOKS)
        {
            if (ModelState.IsValid)
            {
                db.BOOKS.Add(bOOKS);
                db.SaveChanges();
                DisplaySuccessMessage("Has append a BOOKS record");
                return RedirectToAction("BOOKSIndex");
            }

            DisplayErrorMessage();
            return View(bOOKS);
        }

        // GET: BOOKS/BOOKSEdit/5
        public ActionResult BOOKSEdit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOKS bOOKS = db.BOOKS.Find(id);
            if (bOOKS == null)
            {
                return HttpNotFound();
            }
            return View(bOOKS);
        }

        // POST: BOOKSBOOKS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BOOKSEdit([Bind(Include = "ID,BOOKNAME,AUTHOR,PUBLISH_UTC,VERSION_NUM,VERSION_NUM2,LIST_PRICE,ISOK")] BOOKS bOOKS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bOOKS).State = EntityState.Modified;
                db.SaveChanges();
                DisplaySuccessMessage("Has update a BOOKS record");
                return RedirectToAction("BOOKSIndex");
            }
            DisplayErrorMessage();
            return View(bOOKS);
        }

        // GET: BOOKS/BOOKSDelete/5
        public ActionResult BOOKSDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOKS bOOKS = db.BOOKS.Find(id);
            if (bOOKS == null)
            {
                return HttpNotFound();
            }
            return View(bOOKS);
        }

        // POST: BOOKS/BOOKSDelete/5
        [HttpPost, ActionName("BOOKSDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult BOOKSDeleteConfirmed(string id)
        {
            BOOKS bOOKS = db.BOOKS.Find(id);
            db.BOOKS.Remove(bOOKS);
            db.SaveChanges();
            DisplaySuccessMessage("Has delete a BOOKS record");
            return RedirectToAction("BOOKSIndex");
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
