/// <summary>
/// File: CategoriesController.cs
/// Purpose:
/// Date: 2019/1/2 15:53:09
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: RegisterType UnityConfig.cs
///    container.RegisterType<IRepositoryAsync<Category>, Repository<Category>>();
///    container.RegisterType<ICategoryService, CategoryService>();
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
	public class CategoriesController : Controller
	{
		private readonly ICategoryService  categoryService;
		private readonly IUnitOfWorkAsync unitOfWork;
		public CategoriesController (ICategoryService  categoryService, IUnitOfWorkAsync unitOfWork)
		{
			this.categoryService  = categoryService;
			this.unitOfWork = unitOfWork;
		}
        		//GET: Categories/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public ActionResult Index() => this.View();

		//Get :Categories/GetData
		//For Index View datagrid datasource url
		[HttpGet]
		 public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var pagerows  = (await this.categoryService
						               .Query(new CategoryQuery().Withfilter(filters))
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    Products = n.Products,
    Id = n.Id,
    Name = n.Name
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
        //easyui datagrid post acceptChanges 
		[HttpPost]
		public async Task<JsonResult> SaveDataAsync(CategoryChangeViewModel categories)
		{
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }
            if (ModelState.IsValid)
            {
			   if (categories.updated != null)
			   {
				foreach (var item in categories.updated)
				{
					this.categoryService.Update(item);
				}
			   }
			if (categories.deleted != null)
			{
				foreach (var item in categories.deleted)
				{
					this.categoryService.Delete(item);
				}
			}
			if (categories.inserted != null)
			{
				foreach (var item in categories.inserted)
				{
					this.categoryService.Insert(item);
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
						//GET: Categories/Details/:id
		public ActionResult Details(int id)
		{
			
			var category = this.categoryService.Find(id);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}
        //GET: Categories/GetItemAsync/:id
        [HttpGet]
        public async Task<JsonResult> GetItemAsync(int id) {
            var  category = await this.categoryService.FindAsync(id);
            return Json(category,JsonRequestBehavior.AllowGet);
        }
		//GET: Categories/Create
        		public ActionResult Create()
				{
			var category = new Category();
			//set default value
			return View(category);
		}
		//POST: Categories/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateAsync(Category category)
		{
			if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            } 
            if (ModelState.IsValid)
			{
				category.TrackingState = TrackingState.Added;   
				foreach (var item in category.Products)
				{
					item.CategoryId = category.Id ;
					item.TrackingState = TrackingState.Added;
				}
               try{ 
				categoryService.ApplyChanges(category);
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
			    //DisplaySuccessMessage("Has update a category record");
			}
			else {
			   var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			   //DisplayErrorMessage(modelStateErrors);
			}
			//return View(category);
		}

        //新增对象初始化
        [HttpGet]
        public JsonResult PopupCreate() {
            var category = new Category();
            return Json(category, JsonRequestBehavior.AllowGet);
        }

        //GET: Categories/PopupEdit/:id
        //[OutputCache(Duration = 360, VaryByParam = "id")]
        [HttpGet]
		public async Task<JsonResult> PopupEditAsync(int id)
		{
			
			var category = await this.categoryService.FindAsync(id);
			return Json(category,JsonRequestBehavior.AllowGet);
		}

		//GET: Categories/Edit/:id
		public ActionResult Edit(int id)
		{
			var category = this.categoryService.Find(id);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}
		//POST: Categories/Edit/:id
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditAsync(Category category)
		{
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
			if (ModelState.IsValid)
			{
				category.TrackingState = TrackingState.Modified;
												foreach (var item in category.Products)
				{
					item.CategoryId = category.Id ;
					//set ObjectState with conditions
					if(item.Id <= 0) {
						item.TrackingState = TrackingState.Added;
                    }
					else {
						item.TrackingState = TrackingState.Modified;
                    }
				}
				 
                try{
				categoryService.ApplyChanges(category);
				                
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
				
				//DisplaySuccessMessage("Has update a Category record");
				//return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			//DisplayErrorMessage(modelStateErrors);
			}
						//return View(category);
		}
		//GET: Categories/Delete/:id
		public async Task<ActionResult> DeleteAsync(int id)
		{
			var category = await this.categoryService.FindAsync(id);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}
		//POST: Categories/Delete/:id
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var category = await  this.categoryService.FindAsync(id);
			 this.categoryService.Delete(category);
			var result = await this.unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true,result=result }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a Category record");
			return RedirectToAction("Index");
		}
		//Get Detail Row By Id For Edit
		//Get : Categories/EditProduct/:id
		[HttpGet]
				public async Task<ActionResult> EditProduct(int id)
				{
			var productRepository = this.unitOfWork.RepositoryAsync<Product>();
						var product = await productRepository.FindAsync(id);
									var categoryRepository = this.unitOfWork.RepositoryAsync<Category>();             
						if (product == null)
			{
											ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name" );
											//return HttpNotFound();
				return PartialView("_ProductEditForm", new Product());
			}
			else
			{
											 ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().ToListAsync(), "Id", "Name" , product.CategoryId );  
										}
			return PartialView("_ProductEditForm",  product);
		}
		//Get Create Row By Id For Edit
		//Get : Categories/CreateProduct
		[HttpGet]
				public async Task<ActionResult> CreateProductAsync(int categoryid)
				{
		  			  var categoryRepository = this.unitOfWork.RepositoryAsync<Category>();    
			  			  ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name" );
			  		  			return PartialView("_ProductEditForm");
		}
		//Post Delete Detail Row By Id
		//Get : Categories/DeleteProduct/:id
		[HttpGet]
				public async Task<ActionResult> DeleteProductAsync(int  id)
				{
            try{
			   var productRepository = this.unitOfWork.RepositoryAsync<Product>();
			   productRepository.Delete(id);
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
		}
       
		//Get : Categories/GetProductsByCategoryId/:id
		[HttpGet]
				public async Task<JsonResult> GetProductsByCategoryIdAsync(int id)
				{
			var products = this.categoryService.GetProductsByCategoryId(id);
							var data = await products.AsQueryable().ToListAsync();
							var rows = data.Select( n => new { 

    CategoryName = n.Category?.Name,
    Id = n.Id,
    Name = n.Name,
    Unit = n.Unit,
    UnitPrice = n.UnitPrice,
    StockQty = n.StockQty,
    IsRequiredQc = n.IsRequiredQc,
    ConfirmDateTime = n.ConfirmDateTime.ToString("yyyy/MM/dd HH:mm:ss"),
    CategoryId = n.CategoryId
});
				return Json(rows, JsonRequestBehavior.AllowGet);
			
		}
 

        //删除选中的记录
        [HttpPost]
        public async Task<JsonResult> DeleteCheckedAsync(int[] id) {
           if (id == null)
           {
                throw new ArgumentNullException(nameof(id));
           }
           try{
               await this.categoryService.Queryable().Where(x => id.Contains(x.Id)).DeleteAsync();
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
			var fileName = "categories_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  this.categoryService.ExportExcel(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;
		 
	}
}
