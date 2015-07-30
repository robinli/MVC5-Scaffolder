


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


namespace WebApp.Controllers
{
    public class CategoriesController : Controller
    {

        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<Category>, Repository<Category>>();
        //container.RegisterType<ICategoryService, CategoryService>();

        //private StoreContext db = new StoreContext();
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public CategoriesController(ICategoryService categoryService, IUnitOfWorkAsync unitOfWork)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
        }

        // GET: Categories/Index
        public ActionResult Index()
        {

            var categories = _categoryService.Queryable();
            return View(categories);
        }

        // Get :Categories/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit + 1;
            var categories = _categoryService.Query(new CategoryQuery().WithAnySearch(search)).OrderBy(n => n.OrderBy(sort, order)).SelectPage(pagenum, limit, out totalCount);
            var rows = categories.Select(n => new { Id = n.Id, Name = n.Name }).ToList();
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
            Category category = new Category();
            //set default value
            return View(category);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Products,Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.ObjectState = ObjectState.Added;
                foreach (var item in category.Products)
                {
                    item.CategoryId = category.Id;
                    item.ObjectState = ObjectState.Added;
                }
                _categoryService.InsertOrUpdateGraph(category);
                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Category record");
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
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
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Products,Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.ObjectState = ObjectState.Modified;
                foreach (var item in category.Products)
                {
                    item.CategoryId = category.Id;
                    //set ObjectState with conditions
                    if (item.Id <= 0)
                        item.ObjectState = ObjectState.Added;
                    else
                        item.ObjectState = ObjectState.Modified;
                }

                _categoryService.InsertOrUpdateGraph(category);

                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Category record");
                return RedirectToAction("Index");
            }
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
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
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryService.Find(id);
            _categoryService.Delete(category);
            _unitOfWork.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
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
                ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name");

                //return HttpNotFound();
                return PartialView("_ProductEditForm", new Product());
            }
            else
            {
                ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name", product.CategoryId);

            }
            return PartialView("_ProductEditForm", product);

        }

        // Get Create Row By Id For Edit
        // Get : Categories/CreateProduct
        [HttpGet]
        public ActionResult CreateProduct()
        {
            var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name");
            return PartialView("_ProductEditForm");

        }

        // Post Delete Detail Row By Id
        // Get : Categories/DeleteProduct/:id
        [HttpPost, ActionName("DeleteProduct")]
        public ActionResult DeleteProductConfirmed(int id)
        {
            var productRepository = _unitOfWork.Repository<Product>();
            productRepository.Delete(id);
            _unitOfWork.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Order record");
            return RedirectToAction("Index");
        }



        // Get : Categories/GetProductsByCategoryId/:id
        [HttpGet]
        public ActionResult GetProductsByCategoryId(int id)
        {
            var products = _categoryService.GetProductsByCategoryId(id);
            if (Request.IsAjaxRequest())
            {
                return Json(products.Select(n => new { CategoryName = (n.Category == null ? "" : n.Category.Name), Id = n.Id, Name = n.Name, Unit = n.Unit, UnitPrice = n.UnitPrice, StockQty = n.StockQty, ConfirmDateTime = n.ConfirmDateTime, CategoryId = n.CategoryId }), JsonRequestBehavior.AllowGet);
            }
            return View(products);

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
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
