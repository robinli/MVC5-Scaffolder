


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
using WebApp.Extensions;
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
        public ActionResult Index()
        {
            
            var products  = _productService.Queryable().Include(p => p.Category).AsQueryable();
            
             return View(products);
        }

        // Get :Products/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit +1;
                        var product  = _productService.Query(new ProductQuery().WithAnySearch(search)).Include(p => p.Category).OrderBy(n=>n.OrderBy(sort,order)).SelectPage(pagenum, limit, out totalCount);
            
                        var rows = product .Select( n => new { CategoryName = n.Category.Name , Id = n.Id , Name = n.Name , Unit = n.Unit , UnitPrice = n.UnitPrice , StockQty = n.StockQty , ConfirmDateTime = n.ConfirmDateTime , CategoryId = n.CategoryId }).ToList();
            var pagelist = new { total = totalCount, rows = rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
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
                				_productService.Update(product);
                                
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
