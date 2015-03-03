

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
    public class CategoriesController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly ICategoryService  _categoriesService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public CategoriesController (ICategoryService  categoriesService, IUnitOfWorkAsync unitOfWork)
        {
            _categoriesService  = categoriesService;
            _unitOfWork = unitOfWork;
        }

        // GET: Categories/Index
        public ActionResult Index()
        {
            
            var categories  = _categoriesService.Queryable().AsQueryable();
            return View(categories  );
        }

        // Get :Categories/PageList
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit +1;
                        var categories  = _categoriesService.Query(new CategoryQuery().WithAnySearch(search)).OrderBy(n=>n.OrderBy(sort,order)).SelectPage(pagenum, limit, out totalCount);
                        var rows = categories .Select( n => new {  Id = n.Id , Name = n.Name }).ToList();
            var pagelist = new { total = totalCount, rows = rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

       
        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category categories = _categoriesService.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }
        

        // GET: Categories/Create
        public ActionResult Create()
        {
		   //Detail Models RelatedProperties 
			var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name");

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Products,Id,Name")] Category categories)
        {
            if (ModelState.IsValid)
            {
               _categoriesService.Insert(categories);
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has append a Category record");
                return RedirectToAction("Index");
            }

            DisplayErrorMessage();
            return View(categories);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category categories = _categoriesService.Find(id);

		   //Detail Models RelatedProperties 
			var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name");

			ViewBag.Products = categories.Products.Select(n => new { Category = n.Category,Id = n.Id,Name = n.Name,Unit = n.Unit,UnitPrice = n.UnitPrice,StockQty = n.StockQty,ConfirmDateTime = n.ConfirmDateTime,CategoryId = n.CategoryId });



            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Products,Id,Name")] Category categories)
        {
            if (ModelState.IsValid)
            {
                categories.ObjectState = ObjectState.Modified;
				_categoriesService.Update(categories);
                
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has update a Category record");
                return RedirectToAction("Index");
            }
            DisplayErrorMessage();
            return View(categories);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category categories = _categoriesService.Find(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category categories =  _categoriesService.Find(id);
             _categoriesService.Delete(categories);
            _unitOfWork.SaveChanges();
            DisplaySuccessMessage("Has delete a Category record");
            return RedirectToAction("Index");
        }


        // Get Detail Row By Id For Edit
        // Get : Categories/EditProduct/:id
        [HttpGet]
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productRepository = _unitOfWork.Repository<Product>();
            var product = productRepository.Find(id);

                        var categoryRepository = _unitOfWork.Repository<Category>();

                        if (product == null)
            {
                            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name" );
                            
                //return HttpNotFound();
                return PartialView("_OrderDetailForm", new OrderDetail());
            }
            else
            {
                            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name" , product.CategoryId );  
                             
            }
                        return PartialView("_OrderDetailForm", product);

        }
        
        // Get Create Row By Id For Edit
        // Get : Categories/CreateProduct
        [HttpGet]
        public ActionResult CreateProduct()
        {
                        var categoryRepository = _unitOfWork.Repository<Category>();    
              ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name" );
                      return PartialView("_OrderDetailForm");

        }

        // Post Delete Detail Row By Id
        // Get : Categories/DeleteProductConfirmed/:id
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteProductConfirmed(int  id)
        {
            var productRepository = _unitOfWork.Repository<Product>();
            productRepository.Delete(id);
            _unitOfWork.SaveChanges();
            DisplaySuccessMessage("Has delete a Order record");
            return RedirectToAction("Index");
        }

       

                 // Get : Categories/GetProductsByCategoryId/:id
        [HttpGet]
        public ActionResult GetProductsByCategoryId(int id)
        {
            var products = _categoriesService.GetProductsByCategoryId(id);
            return Json(products.Select(n => new { Category = n.Category, Id = n.Id, Name = n.Name, Unit = n.Unit, UnitPrice = n.UnitPrice, StockQty = n.StockQty, ConfirmDateTime = n.ConfirmDateTime, CategoryId = n.CategoryId }), JsonRequestBehavior.AllowGet);

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
