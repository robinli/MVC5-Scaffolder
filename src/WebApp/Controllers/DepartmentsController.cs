/// <summary>
/// File: DepartmentsController.cs
/// Purpose:
/// Date: 2018/11/15 10:27:41
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: RegisterType UnityConfig.cs
///    container.RegisterType<IRepositoryAsync<Department>, Repository<Department>>();
///    container.RegisterType<IDepartmentService, DepartmentService>();
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
	public class DepartmentsController : Controller
	{
		private readonly IDepartmentService  departmentService;
		private readonly IUnitOfWorkAsync unitOfWork;
		public DepartmentsController (IDepartmentService  departmentService, IUnitOfWorkAsync unitOfWork)
		{
			this.departmentService  = departmentService;
			this.unitOfWork = unitOfWork;
		}
        		//GET: Departments/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public ActionResult Index() => this.View();

		//Get :Departments/GetData
		//For Index View datagrid datasource url
		[HttpGet]
		 public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var pagerows  = (await this.departmentService
						               .Query(new DepartmentQuery().Withfilter(filters)).Include(d => d.Company)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    CompanyName = n.Company?.Name,
    Id = n.Id,
    Name = n.Name,
    Manager = n.Manager,
    CompanyId = n.CompanyId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
                 [HttpGet]
        public async Task<JsonResult> GetDataByCompanyIdAsync (int  companyid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
		
            			    var pagerows = (await this.departmentService
						               .Query(new DepartmentQuery().ByCompanyIdWithfilter(companyid,filters)).Include(d => d.Company)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    CompanyName = n.Company?.Name,
    Id = n.Id,
    Name = n.Name,
    Manager = n.Manager,
    CompanyId = n.CompanyId
}).ToList();
							var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
                //easyui datagrid post acceptChanges 
		[HttpPost]
		public async Task<JsonResult> SaveDataAsync(DepartmentChangeViewModel departments)
		{
            if (departments == null)
            {
                throw new ArgumentNullException(nameof(departments));
            }
            if (ModelState.IsValid)
            {
			   if (departments.updated != null)
			   {
				foreach (var item in departments.updated)
				{
					this.departmentService.Update(item);
				}
			   }
			if (departments.deleted != null)
			{
				foreach (var item in departments.deleted)
				{
					this.departmentService.Delete(item);
				}
			}
			if (departments.inserted != null)
			{
				foreach (var item in departments.inserted)
				{
					this.departmentService.Insert(item);
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
		public async Task<JsonResult> GetCompaniesAsync(string q="")
		{
			var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
			var rows = await companyRepository
                            .Queryable()
                            .Where(n=>n.Name.Contains(q))
                            .OrderBy(n=>n.Name)
                            .Select(n => new { Id = n.Id, Name = n.Name })
                            .ToListAsync();
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
								//GET: Departments/Details/:id
		public ActionResult Details(int id)
		{
			
			var department = this.departmentService.Find(id);
			if (department == null)
			{
				return HttpNotFound();
			}
			return View(department);
		}
        //GET: Departments/GetItemAsync/:id
        [HttpGet]
        public async Task<JsonResult> GetItemAsync(int id) {
            var  department = await this.departmentService.FindAsync(id);
            return Json(department,JsonRequestBehavior.AllowGet);
        }
		//GET: Departments/Create
        		public ActionResult Create()
				{
			var department = new Department();
			//set default value
			var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
		   			ViewBag.CompanyId = new SelectList(companyRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name");
		   			return View(department);
		}
		//POST: Departments/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateAsync(Department department)
		{
			if (department == null)
            {
                throw new ArgumentNullException(nameof(department));
            } 
            if (ModelState.IsValid)
			{
				departmentService.Insert(department);
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
			    //DisplaySuccessMessage("Has update a department record");
			}
			else {
			   var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			   //DisplayErrorMessage(modelStateErrors);
			}
			//var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
			//ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", department.CompanyId);
			//return View(department);
		}

        //新增对象初始化
        [HttpGet]
        public JsonResult PopupCreate() {
            var department = new Department();
            return Json(department, JsonRequestBehavior.AllowGet);
        }

        //GET: Departments/PopupEdit/:id
        //[OutputCache(Duration = 360, VaryByParam = "id")]
        [HttpGet]
		public async Task<JsonResult> PopupEditAsync(int id)
		{
			
			var department = await this.departmentService.FindAsync(id);
			return Json(department,JsonRequestBehavior.AllowGet);
		}

		//GET: Departments/Edit/:id
		public ActionResult Edit(int id)
		{
			var department = this.departmentService.Find(id);
			if (department == null)
			{
				return HttpNotFound();
			}
			var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
			ViewBag.CompanyId = new SelectList(companyRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name", department.CompanyId);
			return View(department);
		}
		//POST: Departments/Edit/:id
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditAsync(Department department)
		{
            if (department == null)
            {
                throw new ArgumentNullException(nameof(department));
            }
			if (ModelState.IsValid)
			{
				department.TrackingState = TrackingState.Modified;
								departmentService.Update(department);
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
				
				//DisplaySuccessMessage("Has update a Department record");
				//return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			//DisplayErrorMessage(modelStateErrors);
			}
						//var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
												//return View(department);
		}
		//GET: Departments/Delete/:id
		public async Task<ActionResult> DeleteAsync(int id)
		{
			var department = await this.departmentService.FindAsync(id);
			if (department == null)
			{
				return HttpNotFound();
			}
			return View(department);
		}
		//POST: Departments/Delete/:id
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var department = await  this.departmentService.FindAsync(id);
			 this.departmentService.Delete(department);
			var result = await this.unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true,result=result }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a Department record");
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
               await this.departmentService.Queryable().Where(x => id.Contains(x.Id)).DeleteAsync();
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
			var fileName = "departments_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  this.departmentService.ExportExcel(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;
		 
	}
}
