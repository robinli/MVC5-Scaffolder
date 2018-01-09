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
	public class BaseCodesController : Controller
	{
		
		//Please RegisterType UnityConfig.cs
		//container.RegisterType<IRepositoryAsync<BaseCode>, Repository<BaseCode>>();
		//container.RegisterType<IBaseCodeService, BaseCodeService>();
		
		//private StoreContext db = new StoreContext();
		private readonly IBaseCodeService  _baseCodeService;
		private readonly IUnitOfWorkAsync _unitOfWork;

		public BaseCodesController (IBaseCodeService  baseCodeService, IUnitOfWorkAsync unitOfWork)
		{
			_baseCodeService  = baseCodeService;
			_unitOfWork = unitOfWork;
		}

		// GET: BaseCodes/Index
		public async Task<ActionResult> Index()
		{
			
			var basecodes  = _baseCodeService.Queryable();
			return View(await basecodes.ToListAsync()  );
			
		}

		// Get :BaseCodes/PageList
		// For Index View Boostrap-Table load  data 
		[HttpGet]
				 public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
				{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var totalCount = 0;
			//int pagenum = offset / limit +1;
											var basecodes  = await  _baseCodeService
						               .Query(new BaseCodeQuery().Withfilter(filters))
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out totalCount);
      			
						var datarows = basecodes .Select(  n => new {  Id = n.Id , CodeType = n.CodeType , Description = n.Description }).ToList();
			var pagelist = new { total = totalCount, rows = datarows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}

         


		[HttpPost]
				public async Task<ActionResult> SaveData(BaseCodeChangeViewModel basecodes)
		{
			if (basecodes.updated != null)
			{
				foreach (var item in basecodes.updated)
				{
					_baseCodeService.Update(item);
				}
			}
			if (basecodes.deleted != null)
			{
				foreach (var item in basecodes.deleted)
				{
					_baseCodeService.Delete(item);
				}
			}
			if (basecodes.inserted != null)
			{
				foreach (var item in basecodes.inserted)
				{
					_baseCodeService.Insert(item);
				}
			}
			await _unitOfWork.SaveChangesAsync();

			return Json(new {Success=true}, JsonRequestBehavior.AllowGet);
		}
		
		
		
	   
		// GET: BaseCodes/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var  baseCode = await _baseCodeService.FindAsync(id);

			if (baseCode == null)
			{
				return HttpNotFound();
			}
			return View(baseCode);
		}
		

		// GET: BaseCodes/Create
				public ActionResult Create()
				{
			var baseCode = new BaseCode();
			//set default value
			return View(baseCode);
		}

		// POST: BaseCodes/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "CodeItems,Id,CodeType,Description,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] BaseCode baseCode)
		{
			if (ModelState.IsValid)
			{
			 				_baseCodeService.Insert(baseCode);
		   				await _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has append a BaseCode record");
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
			
			return View(baseCode);
		}

		// GET: BaseCodes/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var baseCode = await _baseCodeService.FindAsync(id);
			if (baseCode == null)
			{
				return HttpNotFound();
			}
			return View(baseCode);
		}

		// POST: BaseCodes/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "CodeItems,Id,CodeType,Description,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] BaseCode baseCode)
		{
			if (ModelState.IsValid)
			{
				baseCode.ObjectState = ObjectState.Modified;
								_baseCodeService.Update(baseCode);
								
				await   _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has update a BaseCode record");
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
			
			return View(baseCode);
		}

		// GET: BaseCodes/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var baseCode = await _baseCodeService.FindAsync(id);
			if (baseCode == null)
			{
				return HttpNotFound();
			}
			return View(baseCode);
		}

		// POST: BaseCodes/Delete/5
		[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var baseCode = await  _baseCodeService.FindAsync(id);
			 _baseCodeService.Delete(baseCode);
			await _unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a BaseCode record");
			return RedirectToAction("Index");
		}


       

 

		//导出Excel
		[HttpPost]
		public ActionResult ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "basecodes_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  _baseCodeService.ExportExcel(filterRules,sort, order );
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
