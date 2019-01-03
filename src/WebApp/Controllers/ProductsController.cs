/// <summary>
/// File: ProductsController.cs
/// Purpose:
/// Date: 2018/12/20 10:19:50
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: RegisterType UnityConfig.cs
///    container.RegisterType<IRepositoryAsync<Product>, Repository<Product>>();
///    container.RegisterType<IProductService, ProductService>();
///
/// Copyright (c) 2012-2018 neo.zhu and Contributors
/// License: GNU General Public License v3.See license.txt
/// </summary>
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
using TrackableEntities;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
namespace WebApp.Controllers
{
    [Authorize]
	public class ProductsController : Controller
	{
		private readonly IProductService  productService;
		private readonly IUnitOfWorkAsync unitOfWork;
		public ProductsController (IProductService  productService, IUnitOfWorkAsync unitOfWork)
		{
			this.productService  = productService;
			this.unitOfWork = unitOfWork;
		}
        		//GET: Products/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public ActionResult Index() => this.View();

		//Get :Products/GetData
		//For Index View datagrid datasource url
		[HttpGet]
		 public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var pagerows  = (await this.productService
						               .Query(new ProductQuery().Withfilter(filters)).Include(p => p.Category)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    CategoryName = n.Category?.Name,
    Id = n.Id,
    Name = n.Name,
    Unit = n.Unit,
    UnitPrice = n.UnitPrice,
    StockQty = n.StockQty,
    IsRequiredQc = n.IsRequiredQc,
    ConfirmDateTime = n.ConfirmDateTime.ToString("yyyy/MM/dd HH:mm:ss"),
    CategoryId = n.CategoryId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
        [HttpGet]
        public async Task<JsonResult> GetDataByCategoryIdAsync (int  categoryid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			    var pagerows = (await this.productService
						               .Query(new ProductQuery().ByCategoryIdWithfilter(categoryid,filters)).Include(p => p.Category)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    CategoryName = n.Category?.Name,
    Id = n.Id,
    Name = n.Name,
    Unit = n.Unit,
    UnitPrice = n.UnitPrice,
    StockQty = n.StockQty,
    IsRequiredQc = n.IsRequiredQc,
    ConfirmDateTime = n.ConfirmDateTime.ToString("yyyy/MM/dd HH:mm:ss"),
    CategoryId = n.CategoryId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        //easyui datagrid post acceptChanges 
		[HttpPost]
		public async Task<JsonResult> SaveDataAsync(ProductChangeViewModel products)
		{
            if (products == null)
            {
                throw new ArgumentNullException(nameof(products));
            }
            if (ModelState.IsValid)
            {
			   if (products.updated != null)
			   {
				foreach (var item in products.updated)
				{
					this.productService.Update(item);
				}
			   }
			if (products.deleted != null)
			{
				foreach (var item in products.deleted)
				{
					this.productService.Delete(item);
				}
			}
			if (products.inserted != null)
			{
				foreach (var item in products.inserted)
				{
					this.productService.Insert(item);
				}
			}
            try{
			   var result = await this.unitOfWork.SaveChangesAsync();
			   return Json(new {success=true,result=result}, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                 return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
		    }
            else
            {
                var modelStateErrors = string.Join(",", ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
        
        }
				        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public async Task<JsonResult> GetCategoriesAsync(string q="")
		{
			var categoryRepository = this.unitOfWork.RepositoryAsync<Category>();
			var rows = await categoryRepository
                            .Queryable()
                            .Where(n=>n.Name.Contains(q))
                            .OrderBy(n=>n.Name)
                            .Select(n => new { Id = n.Id, Name = n.Name })
                            .ToListAsync();
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
								//GET: Products/Details/:id
		public ActionResult Details(int id)
		{
			
			var product = this.productService.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}
        //GET: Products/GetItemAsync/:id
        [HttpGet]
        public async Task<JsonResult> GetItemAsync(int id) {
            var  product = await this.productService.FindAsync(id);
            return Json(product,JsonRequestBehavior.AllowGet);
        }
		//GET: Products/Create
        		public ActionResult Create()
				{
			var product = new Product();
			//set default value
			var categoryRepository = this.unitOfWork.RepositoryAsync<Category>();
		   			ViewBag.CategoryId = new SelectList(categoryRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name");
		   			return View(product);
		}
		//POST: Products/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateAsync(Product product)
		{
			if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            } 
            if (ModelState.IsValid)
			{
				productService.Insert(product);
                try{ 
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result=result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                   var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                   return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
			    //DisplaySuccessMessage("Has update a product record");
			}
			else {
			   var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			   //DisplayErrorMessage(modelStateErrors);
			}
			//var categoryRepository = this.unitOfWork.RepositoryAsync<Category>();
			//ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", product.CategoryId);
			//return View(product);
		}

        //新增对象初始化
        [HttpGet]
        public JsonResult PopupCreate() {
            var product = new Product();
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        //GET: Products/PopupEdit/:id
        //[OutputCache(Duration = 360, VaryByParam = "id")]
        [HttpGet]
		public async Task<JsonResult> PopupEditAsync(int id)
		{
			
			var product = await this.productService.FindAsync(id);
			return Json(product,JsonRequestBehavior.AllowGet);
		}

		//GET: Products/Edit/:id
		public ActionResult Edit(int id)
		{
			var product = this.productService.Find(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			var categoryRepository = this.unitOfWork.RepositoryAsync<Category>();
			ViewBag.CategoryId = new SelectList(categoryRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name", product.CategoryId);
			return View(product);
		}
		//POST: Products/Edit/:id
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditAsync(Product product)
		{
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
			if (ModelState.IsValid)
			{
				product.TrackingState = TrackingState.Modified;
								productService.Update(product);
				                try{
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
				
				//DisplaySuccessMessage("Has update a Product record");
				//return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			//DisplayErrorMessage(modelStateErrors);
			}
						//var categoryRepository = this.unitOfWork.RepositoryAsync<Category>();
												//return View(product);
		}
		//GET: Products/Delete/:id
		public async Task<ActionResult> DeleteAsync(int id)
		{
			var product = await this.productService.FindAsync(id);
			if (product == null)
			{
				return HttpNotFound();
			}
			return View(product);
		}
		//POST: Products/Delete/:id
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var product = await  this.productService.FindAsync(id);
			 this.productService.Delete(product);
			var result = await this.unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true,result=result }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a Product record");
			return RedirectToAction("Index");
		}
       
 

        //删除选中的记录
        [HttpPost]
        public async Task<JsonResult> DeleteCheckedAsync(int[] id) {
           if (id == null)
           {
                throw new ArgumentNullException(nameof(id));
           }
           try{
               await this.productService.Queryable().Where(x => id.Contains(x.Id)).DeleteAsync();
               return Json(new { success = true }, JsonRequestBehavior.AllowGet);
           }
           catch (System.Data.Entity.Validation.DbEntityValidationException e)
           {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
           }
           catch (Exception e)
           {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
           }
        }
		//导出Excel
		[HttpPost]
		public ActionResult ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "products_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  this.productService.ExportExcel(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;
		 
	}
}
