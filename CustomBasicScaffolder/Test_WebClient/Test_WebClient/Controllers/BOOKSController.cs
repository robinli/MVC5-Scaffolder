using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Test_WebClient;

namespace Test_WebClient.Controllers
{
    public class BOOKSController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        // GET: BOOKS
        public ActionResult Index()
        {
            return View(db.BOOKS.ToList());
        }

        // GET: BOOKS/Details/5
        public ActionResult Details(string id)
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

        // GET: BOOKS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BOOKS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BOOKNAME,AUTHOR,PUBLISH_UTC,VERSION_NUM,VERSION_NUM2,ISOK,LIST_PRICE")] BOOKS bOOKS)
        {
            if (ModelState.IsValid)
            {
                db.BOOKS.Add(bOOKS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bOOKS);
        }

        // GET: BOOKS/Edit/5
        public ActionResult Edit(string id)
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

        // POST: BOOKS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BOOKNAME,AUTHOR,PUBLISH_UTC,VERSION_NUM,VERSION_NUM2,ISOK,LIST_PRICE")] BOOKS bOOKS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bOOKS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bOOKS);
        }

        // GET: BOOKS/Delete/5
        public ActionResult Delete(string id)
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

        // POST: BOOKS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BOOKS bOOKS = db.BOOKS.Find(id);
            db.BOOKS.Remove(bOOKS);
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
