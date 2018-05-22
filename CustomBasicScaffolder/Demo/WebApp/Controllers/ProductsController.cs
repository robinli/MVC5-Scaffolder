// <copyright file="ProductsController.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>12/5/2017 3:58:10 PM </date>
// <summary>
// Create By Custom MVC5 Scaffolder for Visual Studio
// TODO: RegisterType UnityConfig.cs
// container.RegisterType<IRepositoryAsync<Product>, Repository<Product>>();
// container.RegisterType<IProductService, ProductService>();
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
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using TrackableEntities;

namespace WebApp.Controllers
{
    public class ProductsController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly IProductService _productService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public ProductsController(IProductService productService, IUnitOfWorkAsync unitOfWork)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
        }
        // GET: Products/Index
        public ActionResult Index()
        {
            return View();
        }
        // Get :Products/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            //int pagenum = offset / limit +1;
            var products = await _productService
       .Query(new ProductQuery().Withfilter(filters)).Include(p => p.Category)
       .OrderBy(n => n.OrderBy(sort, order))
       .SelectPageAsync(page, rows, out totalCount);
            var datarows = products.Select(n => new { CategoryName = (n.Category == null ? "" : n.Category.Name), Id = n.Id, Name = n.Name, Unit = n.Unit, UnitPrice = n.UnitPrice, StockQty = n.StockQty, ConfirmDateTime = n.ConfirmDateTime, IsRequiredQc = n.IsRequiredQc, CategoryId = n.CategoryId }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> GetDataByCategoryId(int categoryid, int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            var products = await _productService
                       .Query(new ProductQuery().ByCategoryIdWithfilter(categoryid, filters)).Include(p => p.Category)
                       .OrderBy(n => n.OrderBy(sort, order))
                       .SelectPageAsync(page, rows, out totalCount);
            var datarows = products.Select(n => new { CategoryName = (n.Category == null ? "" : n.Category.Name), Id = n.Id, Name = n.Name, Unit = n.Unit, UnitPrice = n.UnitPrice, StockQty = n.StockQty, ConfirmDateTime = n.ConfirmDateTime, IsRequiredQc = n.IsRequiredQc, CategoryId = n.CategoryId }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> SaveData(ProductChangeViewModel products)
        {
            if (products.updated != null)
            {
                foreach (var item in products.updated)
                {
                    _productService.Update(item);
                }
            }
            if (products.deleted != null)
            {
                foreach (var item in products.deleted)
                {
                    _productService.Delete(item);
                }
            }
            if (products.inserted != null)
            {
                foreach (var item in products.inserted)
                {
                    _productService.Insert(item);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetCategories()
        {
            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();
            var data = await categoryRepository.Queryable().ToListAsync();
            var rows = data.Select(n => new { Id = n.Id, Name = n.Name });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await _productService.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        // GET: Products/Create
        public ActionResult Create()
        {
            var product = new Product();
            //set default value
            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name");
            return View(product);
        }
        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Category,Id,Name,Unit,UnitPrice,StockQty,ConfirmDateTime,IsRequiredQc,CategoryId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.Insert(product);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Product record");
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
            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();
            ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }
        // GET: Products/PopupEdit/5
        public async Task<ActionResult> PopupEdit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await _productService.FindAsync(id);
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await _productService.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name", product.CategoryId);
            return View(product);
        }
        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Category,Id,Name,Unit,UnitPrice,StockQty,ConfirmDateTime,IsRequiredQc,CategoryId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.TrackingState = TrackingState.Modified;
                _productService.Update(product);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Product record");
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
            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();
            ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }
        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = await _productService.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.FindAsync(id);
            _productService.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Product record");
            return RedirectToAction("Index");
        }


        //导出Excel
        [HttpPost]
        public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "products_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = _productService.ExportExcel(filterRules, sort, order);
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
