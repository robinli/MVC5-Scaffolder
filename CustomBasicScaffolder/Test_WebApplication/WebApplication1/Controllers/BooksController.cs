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
    public class BooksController : Controller
    {
        private SchoolDBEntities1 db = new SchoolDBEntities1();

        // GET: Books
        public ActionResult Index()
        {
            return View(db.BOOKS.ToList());
        }

        // GET: Books/Details/5
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

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BOOKNAME,AUTHOR,PUBLISH_UTC")] BOOKS bOOKS)
        {
            if (ModelState.IsValid)
            {
                db.BOOKS.Add(bOOKS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bOOKS);
        }

        // GET: Books/Edit/5
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

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BOOKNAME,AUTHOR,PUBLISH_UTC")] BOOKS bOOKS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bOOKS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bOOKS);
        }

        // GET: Books/Delete/5
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

        // POST: Books/Delete/5
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
