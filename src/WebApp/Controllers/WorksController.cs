/// <summary>
/// File: WorksController.cs
/// Purpose:
/// Date: 2018/12/19 14:51:28
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: RegisterType UnityConfig.cs
///    container.RegisterType<IRepositoryAsync<Work>, Repository<Work>>();
///    container.RegisterType<IWorkService, WorkService>();
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
	public class WorksController : Controller
	{
		private readonly IWorkService  workService;
		private readonly IUnitOfWorkAsync unitOfWork;
		public WorksController (IWorkService  workService, IUnitOfWorkAsync unitOfWork)
		{
			this.workService  = workService;
			this.unitOfWork = unitOfWork;
		}
        		//GET: Works/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public ActionResult Index() => this.View();

		//Get :Works/GetData
		//For Index View datagrid datasource url
		[HttpGet]
		 public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var pagerows  = (await this.workService
						               .Query(new WorkQuery().Withfilter(filters))
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    Id = n.Id,
    Name = n.Name,
    Status = n.Status,
    StartDate = n.StartDate.ToString("yyyy/MM/dd HH:mm:ss"),
    EndDate = n.EndDate?.ToString("yyyy/MM/dd HH:mm:ss"),
    ToDoDateTime = n.ToDoDateTime?.ToString("yyyy/MM/dd HH:mm:ss"),
    Enableed = n.Enableed,
    Completed = n.Completed,
    Hour = n.Hour,
    Priority = n.Priority,
    Score = n.Score
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
        //easyui datagrid post acceptChanges 
		[HttpPost]
		public async Task<JsonResult> SaveDataAsync(WorkChangeViewModel works)
		{
            if (works == null)
            {
                throw new ArgumentNullException(nameof(works));
            }
            if (ModelState.IsValid)
            {
			   if (works.updated != null)
			   {
				foreach (var item in works.updated)
				{
					this.workService.Update(item);
				}
			   }
			if (works.deleted != null)
			{
				foreach (var item in works.deleted)
				{
					this.workService.Delete(item);
				}
			}
			if (works.inserted != null)
			{
				foreach (var item in works.inserted)
				{
					this.workService.Insert(item);
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
						//GET: Works/Details/:id
		public ActionResult Details(int id)
		{
			
			var work = this.workService.Find(id);
			if (work == null)
			{
				return HttpNotFound();
			}
			return View(work);
		}
        //GET: Works/GetItemAsync/:id
        [HttpGet]
        public async Task<JsonResult> GetItemAsync(int id) {
            var  work = await this.workService.FindAsync(id);
            return Json(work,JsonRequestBehavior.AllowGet);
        }
		//GET: Works/Create
        		public ActionResult Create()
				{
			var work = new Work();
			//set default value
			return View(work);
		}
		//POST: Works/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateAsync(Work work)
		{
			if (work == null)
            {
                throw new ArgumentNullException(nameof(work));
            } 
            if (ModelState.IsValid)
			{
				workService.Insert(work);
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
			    //DisplaySuccessMessage("Has update a work record");
			}
			else {
			   var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			   //DisplayErrorMessage(modelStateErrors);
			}
			//return View(work);
		}

        //新增对象初始化
        [HttpGet]
        public JsonResult PopupCreate() {
            var work = new Work();
            return Json(work, JsonRequestBehavior.AllowGet);
        }

        //GET: Works/PopupEdit/:id
        //[OutputCache(Duration = 360, VaryByParam = "id")]
        [HttpGet]
		public async Task<JsonResult> PopupEditAsync(int id)
		{
			
			var work = await this.workService.FindAsync(id);
			return Json(work,JsonRequestBehavior.AllowGet);
		}

		//GET: Works/Edit/:id
		public ActionResult Edit(int id)
		{
			var work = this.workService.Find(id);
			if (work == null)
			{
				return HttpNotFound();
			}
			return View(work);
		}
		//POST: Works/Edit/:id
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditAsync(Work work)
		{
            if (work == null)
            {
                throw new ArgumentNullException(nameof(work));
            }
			if (ModelState.IsValid)
			{
				work.TrackingState = TrackingState.Modified;
								workService.Update(work);
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
				
				//DisplaySuccessMessage("Has update a Work record");
				//return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			//DisplayErrorMessage(modelStateErrors);
			}
						//return View(work);
		}
		//GET: Works/Delete/:id
		public async Task<ActionResult> DeleteAsync(int id)
		{
			var work = await this.workService.FindAsync(id);
			if (work == null)
			{
				return HttpNotFound();
			}
			return View(work);
		}
		//POST: Works/Delete/:id
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var work = await  this.workService.FindAsync(id);
			 this.workService.Delete(work);
			var result = await this.unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true,result=result }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a Work record");
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
               await this.workService.Queryable().Where(x => id.Contains(x.Id)).DeleteAsync();
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
			var fileName = "works_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  this.workService.ExportExcel(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;
		 
	}
}
