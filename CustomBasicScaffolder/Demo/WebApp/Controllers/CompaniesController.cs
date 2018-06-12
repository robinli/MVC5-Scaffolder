/// <summary>
/// Provides functionality to the /Company/ route.
/// <date> 5/22/2018 8:33:10 AM </date>
/// Create By SmartCode MVC5 Scaffolder for Visual Studio
/// TODO: RegisterType UnityConfig.cs
/// container.RegisterType<IRepositoryAsync<Company>, Repository<Company>>();
/// container.RegisterType<ICompanyService, CompanyService>();
/// 
/// Copyright (c) 2012-2018 neo.zhu
/// Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
/// and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
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
    //[Authorize]
	public class CompaniesController : Controller
	{
		//private StoreContext db = new StoreContext();
		private readonly ICompanyService  _companyService;
		private readonly IUnitOfWorkAsync _unitOfWork;
		public CompaniesController (ICompanyService  companyService, IUnitOfWorkAsync unitOfWork)
		{
			_companyService  = companyService;
			_unitOfWork = unitOfWork;
		}
        		// GET: Companies/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public ActionResult Index()
		{
            
			 return View();
		}
		// Get :Companies/PageList
		// For Index View Boostrap-Table load  data 
		[HttpGet]
				 public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
				{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var totalCount = 0;
			//int pagenum = offset / limit +1;
											var companies  = await  _companyService
						               .Query(new CompanyQuery().Withfilter(filters))
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out totalCount);
      									var datarows = companies .Select(  n => new { 

    Id = n.Id,
    Name = n.Name,
    Address = n.Address,
    City = n.City,
    Province = n.Province,
    RegisterDate = n.RegisterDate,
    Employees = n.Employees,
    CreatedDate = n.CreatedDate,
    CreatedBy = n.CreatedBy,
    LastModifiedDate = n.LastModifiedDate,
    LastModifiedBy = n.LastModifiedBy
}).ToList();
			var pagelist = new { total = totalCount, rows = datarows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
         		[HttpPost]
				public async Task<JsonResult> SaveData(CompanyChangeViewModel companies)
		{
			if (companies.updated != null)
			{
				foreach (var item in companies.updated)
				{
					_companyService.Update(item);
				}
			}
			if (companies.deleted != null)
			{
				foreach (var item in companies.deleted)
				{
					_companyService.Delete(item);
				}
			}
			if (companies.inserted != null)
			{
				foreach (var item in companies.inserted)
				{
					_companyService.Insert(item);
				}
			}
			await _unitOfWork.SaveChangesAsync();
			return Json(new {Success=true}, JsonRequestBehavior.AllowGet);
		}
								        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public async Task<JsonResult> GetCompanies(string q="")
		{
			var companyRepository = _unitOfWork.RepositoryAsync<Company>();
			var data = await companyRepository.Queryable().Where(n=>n.Name.Contains(q)).ToListAsync();
			var rows = data.Select(n => new { Id = n.Id, Name = n.Name });
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
						        //[OutputCache(Duration = 360, VaryByParam = "none")]
		 
						// GET: Companies/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var  company = await _companyService.FindAsync(id);
			if (company == null)
			{
				return HttpNotFound();
			}
			return View(company);
		}
		// GET: Companies/Create
        		public ActionResult Create()
				{
			var company = new Company();
			//set default value
			return View(company);
		}
		// POST: Companies/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "Departments,Employee,Id,Name,Address,City,Province,RegisterDate,Employees,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Company company)
		{
			if (ModelState.IsValid)
			{
			 				company.TrackingState = TrackingState.Added;   
								foreach (var item in company.Departments)
				{
					item.CompanyId = company.Id ;
					item.TrackingState = TrackingState.Added;
				}
								foreach (var item in company.Employee)
				{
					item.CompanyId = company.Id ;
					item.TrackingState = TrackingState.Added;
				}
								_companyService.ApplyChanges(company);
							await _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has append a Company record");
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
						return View(company);
		}
        // GET: Companies/PopupEdit/5
        //[OutputCache(Duration = 360, VaryByParam = "id")]
		public async Task<JsonResult> PopupEdit(int? id)
		{
			
			var company = await _companyService.FindAsync(id);
			return Json(company,JsonRequestBehavior.AllowGet);
		}

		// GET: Companies/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var company = await _companyService.FindAsync(id);
			if (company == null)
			{
				return HttpNotFound();
			}
			return View(company);
		}
		// POST: Companies/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "Departments,Employee,Id,Name,Address,City,Province,RegisterDate,Employees,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Company company)
		{
			if (ModelState.IsValid)
			{
				company.TrackingState = TrackingState.Modified;
												foreach (var item in company.Departments)
				{
					item.CompanyId = company.Id ;
					//set ObjectState with conditions
					if(item.Id <= 0)
						item.TrackingState = TrackingState.Added;
					else
						item.TrackingState = TrackingState.Modified;
				}
								foreach (var item in company.Employee)
				{
					item.CompanyId = company.Id ;
					//set ObjectState with conditions
					if(item.Id <= 0)
						item.TrackingState = TrackingState.Added;
					else
						item.TrackingState = TrackingState.Modified;
				}
				      
				_companyService.ApplyChanges(company);
								await   _unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has update a Company record");
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
						return View(company);
		}
		// GET: Companies/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var company = await _companyService.FindAsync(id);
			if (company == null)
			{
				return HttpNotFound();
			}
			return View(company);
		}
		// POST: Companies/Delete/5
		[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var company = await  _companyService.FindAsync(id);
			 _companyService.Delete(company);
			await _unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a Company record");
			return RedirectToAction("Index");
		}
		// Get Detail Row By Id For Edit
		// Get : Companies/EditDepartment/:id
		[HttpGet]
				public async Task<ActionResult> EditDepartment(int? id)
				{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var departmentRepository = _unitOfWork.RepositoryAsync<Department>();
						var department = await departmentRepository.FindAsync(id);
									var companyRepository = _unitOfWork.RepositoryAsync<Company>();             
						if (department == null)
			{
											ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name" );
											//return HttpNotFound();
				return PartialView("_DepartmentEditForm", new Department());
			}
			else
			{
											 ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name" , department.CompanyId );  
										}
			return PartialView("_DepartmentEditForm",  department);
		}
		// Get Create Row By Id For Edit
		// Get : Companies/CreateDepartment
		[HttpGet]
				public async Task<ActionResult> CreateDepartment()
				{
		  			  var companyRepository = _unitOfWork.RepositoryAsync<Company>();    
			  			  ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name" );
			  		  			return PartialView("_DepartmentEditForm");
		}
		// Post Delete Detail Row By Id
		// Get : Companies/DeleteDepartment/:id
		[HttpPost,ActionName("DeleteDepartment")]
				public async Task<ActionResult> DeleteDepartmentConfirmed(int  id)
				{
			var departmentRepository = _unitOfWork.RepositoryAsync<Department>();
			departmentRepository.Delete(id);
						await _unitOfWork.SaveChangesAsync();
						if (Request.IsAjaxRequest())
			{
				return Json(new { success = true }, JsonRequestBehavior.AllowGet);
			}
			DisplaySuccessMessage("Has delete a Order record");
			return RedirectToAction("Index");
		}
		// Get Detail Row By Id For Edit
		// Get : Companies/EditEmployee/:id
		[HttpGet]
				public async Task<ActionResult> EditEmployee(int? id)
				{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var employeeRepository = _unitOfWork.RepositoryAsync<Employee>();
						var employee = await employeeRepository.FindAsync(id);
									var companyRepository = _unitOfWork.RepositoryAsync<Company>();             
						if (employee == null)
			{
											ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name" );
											//return HttpNotFound();
				return PartialView("_EmployeeEditForm", new Employee());
			}
			else
			{
											 ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name" , employee.CompanyId );  
										}
			return PartialView("_EmployeeEditForm",  employee);
		}
		// Get Create Row By Id For Edit
		// Get : Companies/CreateEmployee
		[HttpGet]
				public async Task<ActionResult> CreateEmployee()
				{
		  			  var companyRepository = _unitOfWork.RepositoryAsync<Company>();    
			  			  ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name" );
			  		  			return PartialView("_EmployeeEditForm");
		}
		// Post Delete Detail Row By Id
		// Get : Companies/DeleteEmployee/:id
		[HttpPost,ActionName("DeleteEmployee")]
				public async Task<ActionResult> DeleteEmployeeConfirmed(int  id)
				{
			var employeeRepository = _unitOfWork.RepositoryAsync<Employee>();
			employeeRepository.Delete(id);
						await _unitOfWork.SaveChangesAsync();
						if (Request.IsAjaxRequest())
			{
				return Json(new { success = true }, JsonRequestBehavior.AllowGet);
			}
			DisplaySuccessMessage("Has delete a Order record");
			return RedirectToAction("Index");
		}
       
		// Get : Companies/GetDepartmentsByCompanyId/:id
		[HttpGet]
				public async Task<ActionResult> GetDepartmentsByCompanyId(int id)
				{
			var departments = _companyService.GetDepartmentsByCompanyId(id);
			if (Request.IsAjaxRequest())
			{
								var data = await departments.AsQueryable().ToListAsync();
								var rows = data.Select( n => new { 

    CompanyName = (n.Company==null?"": n.Company.Name) ,
    Id = n.Id,
    Name = n.Name,
    Manager = n.Manager,
    CompanyId = n.CompanyId,
    CreatedDate = n.CreatedDate,
    CreatedBy = n.CreatedBy,
    LastModifiedDate = n.LastModifiedDate,
    LastModifiedBy = n.LastModifiedBy
});
				return Json(rows, JsonRequestBehavior.AllowGet);
			}  
			return View(departments); 
		}
		// Get : Companies/GetEmployeeByCompanyId/:id
		[HttpGet]
				public async Task<ActionResult> GetEmployeeByCompanyId(int id)
				{
			var employee = _companyService.GetEmployeeByCompanyId(id);
			if (Request.IsAjaxRequest())
			{
								var data = await employee.AsQueryable().ToListAsync();
								var rows = data.Select( n => new { 

    CompanyName = (n.Company==null?"": n.Company.Name) ,
    Id = n.Id,
    Name = n.Name,
    Title = n.Title,
    Sex = n.Sex,
    Age = n.Age,
    Brithday = n.Brithday,
    IsDeleted = n.IsDeleted,
    CompanyId = n.CompanyId,
    CreatedDate = n.CreatedDate,
    CreatedBy = n.CreatedBy,
    LastModifiedDate = n.LastModifiedDate,
    LastModifiedBy = n.LastModifiedBy
});
				return Json(rows, JsonRequestBehavior.AllowGet);
			}  
			return View(employee); 
		}
 
		//导出Excel
		[HttpPost]
		public ActionResult ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "companies_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  _companyService.ExportExcel(filterRules,sort, order );
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
