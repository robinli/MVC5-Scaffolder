using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using PagedList;

namespace WebApp.Controllers
{
    public class CategoriesController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly ICategoryService  _categoryService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public CategoriesController (ICategoryService  categoryService, IUnitOfWorkAsync unitOfWork)
        {
            _categoryService  = categoryService;
            _unitOfWork = unitOfWork;
        }

        // GET: Categories/Index
        public ActionResult Index(string sortOrder, string currentFilter, string search_field, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            if (searchString != null)
            {
                page = 1;
                currentFilter = searchString;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            //string expression = string.Format("{0} = '{1}'", search_field, searchString);
            //ViewBag.SearchField = search_field;
            var category  = _categoryService.Queryable().OrderBy(n=>n.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                category  = category .Where(n => n.Name.Contains(searchString));
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(category .ToPagedList(pageNumber, pageSize));
        }

       
        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryService.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Products,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
               _categoryService.Insert(category);
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has append a Category record");
                return RedirectToAction("Index");
            }

            DisplayErrorMessage();
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryService.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Products,Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.ObjectState = ObjectState.Modified;
                //category = _categoryService.Find(1);
                //category.Name += "abc";
                _categoryService.Update(category);
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has update a Category record");
                return RedirectToAction("Index");
            }
            DisplayErrorMessage();
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryService.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category =  _categoryService.Find(id);
             _categoryService.Delete(category);
            _unitOfWork.SaveChanges();
            DisplaySuccessMessage("Has delete a Category record");
            return RedirectToAction("Index");
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
                //_unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
