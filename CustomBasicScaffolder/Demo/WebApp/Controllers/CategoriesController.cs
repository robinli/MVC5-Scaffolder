// <copyright file="CategoriesController.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2/2/2018 12:01:16 PM </date>
// <summary>
// Create By Custom MVC5 Scaffolder for Visual Studio
// TODO: RegisterType UnityConfig.cs
// container.RegisterType<IRepositoryAsync<Category>, Repository<Category>>();
// container.RegisterType<ICategoryService, CategoryService>();
// </summary>
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using Z.EntityFramework.Plus;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using TrackableEntities;

namespace WebApp.Controllers
{
    public class CategoriesController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public CategoriesController(ICategoryService categoryService, IUnitOfWorkAsync unitOfWork)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
        }
        // GET: Categories/Index
        [OutputCache(Duration = 360, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }
        // Get :Categories/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            //int pagenum = offset / limit +1;
            var categories = await _categoryService
       .Query(new CategoryQuery().Withfilter(filters))
       .OrderBy(n => n.OrderBy(sort, order))
       .SelectPageAsync(page, rows, out totalCount);
            var datarows = categories.Select(n => new { Id = n.Id, Name = n.Name }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> SaveData(CategoryChangeViewModel categories)
        {
            if (categories.updated != null)
            {
                foreach (var item in categories.updated)
                {
                    _categoryService.Update(item);
                }
            }
            if (categories.deleted != null)
            {
                foreach (var item in categories.deleted)
                {
                    _categoryService.Delete(item);
                }
            }
            if (categories.inserted != null)
            {
                foreach (var item in categories.inserted)
                {
                    _categoryService.Insert(item);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
        [OutputCache(Duration = 360, VaryByParam = "none")]
        public async Task<ActionResult> GetCategories()
        {
            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();
            var data = await categoryRepository.Queryable().ToListAsync();
            var rows = data.Select(n => new { Id = n.Id, Name = n.Name });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = await _categoryService.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        // GET: Categories/Create
        public ActionResult Create()
        {
            var category = new Category();
            //set default value
            return View(category);
        }
        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Products,Id,Name,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.TrackingState = TrackingState.Added;
                foreach (var item in category.Products)
                {
                    item.CategoryId = category.Id;
                    item.TrackingState = TrackingState.Added;
                }
                _categoryService.ApplyChanges(category);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Category record");
                return RedirectToAction("Index");
            }
            else
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
                }
                DisplayErrorMessage(modelStateErrors);
            }
            return View(category);
        }
        // GET: Categories/PopupEdit/5
        [OutputCache(Duration = 360, VaryByParam = "id")]
        public async Task<ActionResult> PopupEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = await _categoryService.FindAsync(id);
            return Json(category, JsonRequestBehavior.AllowGet);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = await _categoryService.FindAsync(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "Products,Id,Name,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.TrackingState = TrackingState.Modified;
                foreach (var item in category.Products)
                {
                    item.CategoryId = category.Id;
                    //set ObjectState with conditions
                    if (item.Id <= 0)
                        item.TrackingState = TrackingState.Added;
                    else
                        item.TrackingState = TrackingState.Modified;
                }

                _categoryService.ApplyChanges(category);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Category record");
                return RedirectToAction("Index");
            }
            else
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
                }
                DisplayErrorMessage(modelStateErrors);
            }
            return View(category);
        }
        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = await _categoryService.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryService.FindAsync(id);
            _categoryService.Delete(category);
            await _unitOfWork.SaveChangesAsync();
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
        public async Task<ActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productRepository = _unitOfWork.RepositoryAsync<Product>();
            var product = await productRepository.FindAsync(id);
            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();
            if (product == null)
            {
                ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().ToListAsync(), "Id", "Name");
                //return HttpNotFound();
                return PartialView("_ProductEditForm", new Product());
            }
            else
            {
                ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().ToListAsync(), "Id", "Name", product.CategoryId);
            }
            return PartialView("_ProductEditForm", product);
        }
        // Get Create Row By Id For Edit
        // Get : Categories/CreateProduct
        [HttpGet]
        public async Task<ActionResult> CreateProduct()
        {
            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();
            ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().ToListAsync(), "Id", "Name");
            return PartialView("_ProductEditForm");
        }
        // Post Delete Detail Row By Id
        // Get : Categories/DeleteProduct/:id
        [HttpPost, ActionName("DeleteProduct")]
        public async Task<ActionResult> DeleteProductConfirmed(int id)
        {
            var productRepository = _unitOfWork.RepositoryAsync<Product>();
            productRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Order record");
            return RedirectToAction("Index");
        }

        // Get : Categories/GetProductsByCategoryId/:id
        [HttpGet]
        public async Task<ActionResult> GetProductsByCategoryId(int id)
        {
            var products = _categoryService.GetProductsByCategoryId(id);
            if (Request.IsAjaxRequest())
            {
                var data = await products.AsQueryable().ToListAsync();
                var rows = data.Select(n => new { CategoryName = (n.Category == null ? "" : n.Category.Name), Id = n.Id, Name = n.Name, Unit = n.Unit, UnitPrice = n.UnitPrice, StockQty = n.StockQty, ConfirmDateTime = n.ConfirmDateTime, IsRequiredQc = n.IsRequiredQc, CategoryId = n.CategoryId });
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            return View(products);
        }

        //导出Excel
        [HttpPost]
        public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "categories_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = _categoryService.ExportExcel(filterRules, sort, order);
            return File(stream, "application/vnd.ms-excel", fileName);
        }
        private void DisplaySuccessMessage(string msgText)
        {
            TempData["SuccessMessage"] = msgText;
        }
        private void DisplayErrorMessage(string msgText)
        {
            TempData["ErrorMessage"] = msgText;
        }
         
    }
}
