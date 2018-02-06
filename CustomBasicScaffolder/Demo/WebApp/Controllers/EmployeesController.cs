// <copyright file="EmployeesController.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2/6/2018 10:11:13 AM </date>
// <summary>
// Create By Custom MVC5 Scaffolder for Visual Studio
// TODO: RegisterType UnityConfig.cs
// container.RegisterType<IRepositoryAsync<Employee>, Repository<Employee>>();
// container.RegisterType<IEmployeeService, EmployeeService>();
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
namespace WebApp.Controllers
{
	public class EmployeesController : Controller
	{
		//private StoreContext db = new StoreContext();
		private readonly IEmployeeService  _employeeService;
		private readonly IUnitOfWorkAsync _unitOfWork;
		public EmployeesController (IEmployeeService  employeeService, IUnitOfWorkAsync unitOfWork)
		{
			_employeeService  = employeeService;
			_unitOfWork = unitOfWork;
		}
        		// GET: Employees/Index
        [OutputCache(Duration = 360, VaryByParam = "none")]
		public ActionResult Index()
		{
			 return View();
		}
		// Get :Employees/PageList
		// For Index View Boostrap-Table load  data 
		[HttpGet]
				 public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
				{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var totalCount = 0;
			//int pagenum = offset / limit +1;
											var employees  = await _employeeService
						               .Query(new EmployeeQuery().Withfilter(filters)).Include(e => e.Company)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out totalCount);
										var datarows = employees .Select(  n => new { CompanyName = (n.Company==null?"": n.Company.Name) , Id = n.Id , Name = n.Name , Title = n.Title , Sex = n.Sex , Age = n.Age , Brithday = n.Brithday , IsDeleted = n.IsDeleted , CompanyId = n.CompanyId }).ToList();
			var pagelist = new { total = totalCount, rows = datarows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
                 [HttpGet]
        public async Task<ActionResult> GetDataByCompanyId (int  companyid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var totalCount = 0;
            			    var employees  = await _employeeService
						               .Query(new EmployeeQuery().ByCompanyIdWithfilter(companyid,filters)).Include(e => e.Company)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out totalCount);
				            var datarows = employees .Select(  n => new { CompanyName = (n.Company==null?"": n.Company.Name) , Id = n.Id , Name = n.Name , Title = n.Title , Sex = n.Sex , Age = n.Age , Brithday = n.Brithday , IsDeleted = n.IsDeleted , CompanyId = n.CompanyId }).ToList();
			var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        		[HttpPost]
				public async Task<ActionResult> SaveData(EmployeeChangeViewModel employees)
		{
			if (employees.updated != null)
			{
				foreach (var item in employees.updated)
				{
					_employeeService.Update(item);
				}
			}
			if (employees.deleted != null)
			{
				foreach (var item in employees.deleted)
				{
					_employeeService.Delete(item);
				}
			}
			if (employees.inserted != null)
			{
				foreach (var item in employees.inserted)
				{
					_employeeService.Insert(item);
				}
			}
			await _unitOfWork.SaveChangesAsync();
			return Json(new {Success=true}, JsonRequestBehavior.AllowGet);
		}
						        [OutputCache(Duration = 360, VaryByParam = "none")]
		public async Task<ActionResult> GetCompanies()
		{
			var companyRepository = _unitOfWork.RepositoryAsync<Company>();
			var data = await companyRepository.Queryable().ToListAsync();
			var rows = data.Select(n => new { Id = n.Id, Name = n.Name });
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
								// GET: Employees/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var  employee = await _employeeService.FindAsync(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			return View(employee);
		}
		// GET: Employees/Create
        		public ActionResult Create()
				{
			var employee = new Employee();
			//set default value
			var companyRepository = _unitOfWork.RepositoryAsync<Company>();
		   			ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name");
		   			return View(employee);
		}
		// POST: Employees/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Company,Id,Name,Title,Sex,Age,Brithday,IsDeleted,CompanyId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Employee employee)
		{
			if (ModelState.IsValid)
			{
			 				_employeeService.Insert(employee);
		   				await _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has append a Employee record");
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
						var companyRepository = _unitOfWork.RepositoryAsync<Company>();
						ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name", employee.CompanyId);
									return View(employee);
		}
        // GET: Employees/PopupEdit/5
        [OutputCache(Duration = 360, VaryByParam = "id")]
		public async Task<ActionResult> PopupEdit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var employee = await _employeeService.FindAsync(id);
			return Json(employee,JsonRequestBehavior.AllowGet);
		}

		// GET: Employees/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var employee = await _employeeService.FindAsync(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			var companyRepository = _unitOfWork.RepositoryAsync<Company>();
			ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name", employee.CompanyId);
			return View(employee);
		}
		// POST: Employees/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Company,Id,Name,Title,Sex,Age,Brithday,IsDeleted,CompanyId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Employee employee)
		{
			if (ModelState.IsValid)
			{
				employee.ObjectState = ObjectState.Modified;
								_employeeService.Update(employee);
								await   _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has update a Employee record");
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
						var companyRepository = _unitOfWork.RepositoryAsync<Company>();
						ViewBag.CompanyId = new SelectList( await companyRepository.Queryable().ToListAsync(), "Id", "Name", employee.CompanyId);
									return View(employee);
		}
		// GET: Employees/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var employee = await _employeeService.FindAsync(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			return View(employee);
		}
		// POST: Employees/Delete/5
		[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var employee = await  _employeeService.FindAsync(id);
			 _employeeService.Delete(employee);
			await _unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a Employee record");
			return RedirectToAction("Index");
		}
       
 
		//导出Excel
		[HttpPost]
		public ActionResult ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "employees_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  _employeeService.ExportExcel(filterRules,sort, order );
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
