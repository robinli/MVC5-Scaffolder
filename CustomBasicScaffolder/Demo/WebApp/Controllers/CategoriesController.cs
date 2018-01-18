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
 


namespace WebApp.Controllers
{
	public class CategoriesController : Controller
	{
		
		//Please RegisterType UnityConfig.cs
		//container.RegisterType<IRepositoryAsync<Category>, Repository<Category>>();
		//container.RegisterType<ICategoryService, CategoryService>();
		
		//private StoreContext db = new StoreContext();
		private readonly ICategoryService  _categoryService;
		private readonly IUnitOfWorkAsync _unitOfWork;

		public CategoriesController (ICategoryService  categoryService, IUnitOfWorkAsync unitOfWork)
		{
			_categoryService  = categoryService;
			_unitOfWork = unitOfWork;
		}

		// GET: Categories/Index
		public  ActionResult Index()
		{

            var list = new List<Category>();
            list.Add(new Category() { Name = "Name1", CreatedBy = "", CreatedDate = DateTime.Now });
            list.Add(new Category() { Name = "Name2", CreatedBy = "", CreatedDate = DateTime.Now });
            list.Add(new Category() { Name = "Name3", CreatedBy = "", CreatedDate = DateTime.Now });
            list.Add(new Category() { Name = "Name4", CreatedBy = "", CreatedDate = DateTime.Now });

            _unitOfWork.BulkInsert(list);

            var categories  = _categoryService.Queryable().Where(x=>x.Name.Contains("Name"));
            //foreach (var item in categories) {
            //    item.LastModifiedBy = "test";
            //    item.LastModifiedDate = DateTime.Now;
            //    item.Name = "Test1xx";
            //}
            _unitOfWork.BulkDelete(categories);
            //_unitOfWork.BulkUpdate(categories);

            return View( );
			
		}

		// Get :Categories/PageList
		// For Index View Boostrap-Table load  data 
		[HttpGet]
				 public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
				{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var totalCount = 0;
			//int pagenum = offset / limit +1;
											var categories  = await  _categoryService
						.Query(new CategoryQuery().Withfilter(filters))
							.OrderBy(n=>n.OrderBy(sort,order))
							.SelectPageAsync(page, rows, out totalCount);
							 
				
						var datarows = categories .Select(  n => new {  Id = n.Id , Name = n.Name }).ToList();
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

			return Json(new {Success=true}, JsonRequestBehavior.AllowGet);
		}
		
		
		
	   
		// GET: Categories/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var  category = await _categoryService.FindAsync(id);

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
		public async Task<ActionResult> Create([Bind(Include = "Products,Id,Name")] Category category)
		{
			if (ModelState.IsValid)
			{
			 				_categoryService.Insert(category);
		   				await _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has append a Category record");
				return RedirectToAction("Index");
			}
			else {
			 var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			 if (Request.IsAjaxRequest())
			 {
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			 }
			 DisplayErrorMessage(modelStateErrors);
			}
			
			return View(category);
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
		public async Task<ActionResult> Edit([Bind(Include = "Products,Id,Name")] Category category)
		{
			if (ModelState.IsValid)
			{
				category.ObjectState = ObjectState.Modified;
								_categoryService.Update(category);
								
				await   _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has update a Category record");
				return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
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
			var category = await  _categoryService.FindAsync(id);
			 _categoryService.Delete(category);
			await _unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a Category record");
			return RedirectToAction("Index");
		}


       

 

		//导出Excel
		[HttpPost]
		public ActionResult ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "categories_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  _categoryService.ExportExcel(filterRules,sort, order );
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
