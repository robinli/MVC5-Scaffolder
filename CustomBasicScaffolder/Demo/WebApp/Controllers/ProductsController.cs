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
    public class ProductsController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly IProductService  _productService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public ProductsController (IProductService  productService, IUnitOfWorkAsync unitOfWork)
        {
            _productService  = productService;
            _unitOfWork = unitOfWork;
        }

        // GET: Products/Index
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
            var product  = _productService.Queryable().Include(p => p.Category).OrderBy(n=>n.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                product  = product .Where(n => n.Name.Contains(searchString));
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
             return View(product .ToPagedList(pageNumber, pageSize));
        }

       
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        

        // GET: Products/Create
        public ActionResult Create()
        {
            var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Category,Id,Name,Unit,UnitPrice,StockQty,ConfirmDateTime,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
               _productService.Insert(product);
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has append a Product record");
                return RedirectToAction("Index");
            }

            var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name", product.CategoryId);
            DisplayErrorMessage();
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Category,Id,Name,Unit,UnitPrice,StockQty,ConfirmDateTime,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ObjectState = ObjectState.Modified;
                
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has update a Product record");
                return RedirectToAction("Index");
            }
            var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name", product.CategoryId);
            DisplayErrorMessage();
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product =  _productService.Find(id);
             _productService.Delete(product);
            _unitOfWork.SaveChanges();
            DisplaySuccessMessage("Has delete a Product record");
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
