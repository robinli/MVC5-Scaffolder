/// <summary>
/// File: CompaniesController.cs
/// Purpose:
/// Date: 2018/11/15 10:23:31
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: RegisterType UnityConfig.cs
///    container.RegisterType<IRepositoryAsync<Company>, Repository<Company>>();
///    container.RegisterType<ICompanyService, CompanyService>();
///
/// Copyright (c) 2012-2018 neo.zhu and Contributors
/// License: GNU General Public License v3.See license.txt
/// </summary>
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using TrackableEntities;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;
using Z.EntityFramework.Plus;
namespace WebApp.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private readonly ICompanyService companyService;
        private readonly IUnitOfWorkAsync unitOfWork;
        public CompaniesController(ICompanyService companyService, IUnitOfWorkAsync unitOfWork)
        {
            this.companyService = companyService;
            this.unitOfWork = unitOfWork;
        }
        //GET: Companies/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
        public ActionResult Index() => this.View();

        //Get :Companies/GetData
        //For Index View datagrid datasource url
        [HttpGet]
        public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var pagerows = ( await this.companyService
                                       .Query(new CompanyQuery().Withfilter(filters))
                                       .OrderBy(n => n.OrderBy(sort, order))
                                       .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                           Id = n.Id,
                                           Name = n.Name,
                                           Code = n.Code,
                                           Address = n.Address,
                                           Contect = n.Contect,
                                           PhoneNumber = n.PhoneNumber,
                                           RegisterDate = n.RegisterDate
                                       }).ToList();
            var pagelist = new { total = totalCount, rows = pagerows };
            return this.Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        //easyui datagrid post acceptChanges 
        [HttpPost]
        public async Task<JsonResult> SaveDataAsync(CompanyChangeViewModel companies)
        {
            if (companies == null)
            {
                throw new ArgumentNullException(nameof(companies));
            }
            if (this.ModelState.IsValid)
            {
                if (companies.updated != null)
                {
                    foreach (var item in companies.updated)
                    {
                        this.companyService.Update(item);
                    }
                }
                if (companies.deleted != null)
                {
                    foreach (var item in companies.deleted)
                    {
                        this.companyService.Delete(item);
                    }
                }
                if (companies.inserted != null)
                {
                    foreach (var item in companies.inserted)
                    {
                        this.companyService.Insert(item);
                    }
                }
                try
                {
                    var result = await this.unitOfWork.SaveChangesAsync();
                    return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }

        }
        //[OutputCache(Duration = 360, VaryByParam = "none")]
        public async Task<JsonResult> GetCompaniesAsync(string q = "")
        {
            var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
            var rows = await companyRepository
                            .Queryable()
                            .Where(n => n.Name.Contains(q))
                            .OrderBy(n => n.Name)
                            .Select(n => new { Id = n.Id, Name = n.Name })
                            .ToListAsync();

            return this.Json(rows, JsonRequestBehavior.AllowGet);
        }
        //[OutputCache(Duration = 360, VaryByParam = "none")]
        //GET: Companies/Details/:id
        public ActionResult Details(int id)
        {

            var company = this.companyService.Find(id);
            if (company == null)
            {
                return this.HttpNotFound();
            }
            return this.View(company);
        }
        //GET: Companies/GetItemAsync/:id
        [HttpGet]
        public async Task<JsonResult> GetItemAsync(int id)
        {
            var company = await this.companyService.FindAsync(id);
            return this.Json(company, JsonRequestBehavior.AllowGet);
        }
        //GET: Companies/Create
        public ActionResult Create()
        {
            var company = new Company();
            //set default value
            return this.View(company);
        }
        //POST: Companies/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            if (this.ModelState.IsValid)
            {
                company.TrackingState = TrackingState.Added;
                foreach (var item in company.Departments)
                {
                    item.CompanyId = company.Id;
                    item.TrackingState = TrackingState.Added;
                }
                foreach (var item in company.Employees)
                {
                    item.CompanyId = company.Id;
                    item.TrackingState = TrackingState.Added;
                }
                this.companyService.ApplyChanges(company);
                try
                {
                    var result = await this.unitOfWork.SaveChangesAsync();
                    return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
                //DisplaySuccessMessage("Has update a company record");
            }
            else
            {
                var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
                //DisplayErrorMessage(modelStateErrors);
            }
            //return View(company);
        }

        //新增对象初始化
        [HttpGet]
        public JsonResult PopupCreate()
        {
            var company = new Company();
            return this.Json(company, JsonRequestBehavior.AllowGet);
        }

        //GET: Companies/PopupEdit/:id
        //[OutputCache(Duration = 360, VaryByParam = "id")]
        [HttpGet]
        public async Task<JsonResult> PopupEditAsync(int id)
        {

            var company = await this.companyService.FindAsync(id);
            return this.Json(company, JsonRequestBehavior.AllowGet);
        }

        //GET: Companies/Edit/:id
        public ActionResult Edit(int id)
        {
            var company = this.companyService.Find(id);
            if (company == null)
            {
                return this.HttpNotFound();
            }
            return this.View(company);
        }
        //POST: Companies/Edit/:id
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            if (this.ModelState.IsValid)
            {
                company.TrackingState = TrackingState.Modified;
                foreach (var item in company.Departments)
                {
                    item.CompanyId = company.Id;
                    //set ObjectState with conditions
                    if (item.Id <= 0)
                    {
                        item.TrackingState = TrackingState.Added;
                    }
                    else
                    {
                        item.TrackingState = TrackingState.Modified;
                    }
                }
                foreach (var item in company.Employees)
                {
                    item.CompanyId = company.Id;
                    //set ObjectState with conditions
                    if (item.Id <= 0)
                    {
                        item.TrackingState = TrackingState.Added;
                    }
                    else
                    {
                        item.TrackingState = TrackingState.Modified;
                    }
                }

                this.companyService.ApplyChanges(company);
                try
                {
                    var result = await this.unitOfWork.SaveChangesAsync();
                    return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }

                //DisplaySuccessMessage("Has update a Company record");
                //return RedirectToAction("Index");
            }
            else
            {
                var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
                //DisplayErrorMessage(modelStateErrors);
            }
            //return View(company);
        }
        //GET: Companies/Delete/:id
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var company = await this.companyService.FindAsync(id);
            if (company == null)
            {
                return this.HttpNotFound();
            }
            return this.View(company);
        }
        //POST: Companies/Delete/:id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var company = await this.companyService.FindAsync(id);
            this.companyService.Delete(company);
            var result = await this.unitOfWork.SaveChangesAsync();
            if (this.Request.IsAjaxRequest())
            {
                return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
            }
            this.DisplaySuccessMessage("Has delete a Company record");
            return this.RedirectToAction("Index");
        }
        //Get Detail Row By Id For Edit
        //Get : Companies/EditDepartment/:id
        [HttpGet]
        public async Task<ActionResult> EditDepartment(int id)
        {
            var departmentRepository = this.unitOfWork.RepositoryAsync<Department>();
            var department = await departmentRepository.FindAsync(id);
            var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
            if (department == null)
            {
                this.ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
                //return HttpNotFound();
                return this.PartialView("_DepartmentEditForm", new Department());
            }
            else
            {
                this.ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name", department.CompanyId);
            }
            return this.PartialView("_DepartmentEditForm", department);
        }
        //Get Create Row By Id For Edit
        //Get : Companies/CreateDepartment
        [HttpGet]
        public async Task<ActionResult> CreateDepartmentAsync(int companyid)
        {
            var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
            this.ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
            return this.PartialView("_DepartmentEditForm");
        }
        //Post Delete Detail Row By Id
        //Get : Companies/DeleteDepartment/:id
        [HttpGet]
        public async Task<ActionResult> DeleteDepartmentAsync(int id)
        {
            try
            {
                var departmentRepository = this.unitOfWork.RepositoryAsync<Department>();
                departmentRepository.Delete(id);
                var result = await this.unitOfWork.SaveChangesAsync();
                return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //Get Detail Row By Id For Edit
        //Get : Companies/EditEmployee/:id
        [HttpGet]
        public async Task<ActionResult> EditEmployee(int id)
        {
            var employeeRepository = this.unitOfWork.RepositoryAsync<Employee>();
            var employee = await employeeRepository.FindAsync(id);
            var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
            if (employee == null)
            {
                this.ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
                //return HttpNotFound();
                return this.PartialView("_EmployeeEditForm", new Employee());
            }
            else
            {
                this.ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name", employee.CompanyId);
            }
            return this.PartialView("_EmployeeEditForm", employee);
        }
        //Get Create Row By Id For Edit
        //Get : Companies/CreateEmployee
        [HttpGet]
        public async Task<ActionResult> CreateEmployeeAsync(int companyid)
        {
            var companyRepository = this.unitOfWork.RepositoryAsync<Company>();
            this.ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
            return this.PartialView("_EmployeeEditForm");
        }
        //Post Delete Detail Row By Id
        //Get : Companies/DeleteEmployee/:id
        [HttpGet]
        public async Task<ActionResult> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employeeRepository = this.unitOfWork.RepositoryAsync<Employee>();
                employeeRepository.Delete(id);
                var result = await this.unitOfWork.SaveChangesAsync();
                return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Get : Companies/GetDepartmentsByCompanyId/:id
        [HttpGet]
        public async Task<JsonResult> GetDepartmentsByCompanyIdAsync(int id)
        {
            var departments = this.companyService.GetDepartmentsByCompanyId(id);
            var data = await departments.AsQueryable().ToListAsync();
            var rows = data.Select(n => new
            {

                CompanyName = n.Company?.Name,
                Id = n.Id,
                Name = n.Name,
                Manager = n.Manager,
                CompanyId = n.CompanyId
            });
            return this.Json(rows, JsonRequestBehavior.AllowGet);

        }
        //Get : Companies/GetEmployeesByCompanyId/:id
        [HttpGet]
        public async Task<JsonResult> GetEmployeesByCompanyIdAsync(int id)
        {
            var employees = this.companyService.GetEmployeesByCompanyId(id);
            var data = await employees.AsQueryable().ToListAsync();
            var rows = data.Select(n => new
            {

                CompanyName = n.Company?.Name,
                Id = n.Id,
                Name = n.Name,
                Title = n.Title,
                Sex = n.Sex,
                Age = n.Age,
                Brithday = n.Brithday,
                IsDeleted = n.IsDeleted,
                CompanyId = n.CompanyId
            });
            return this.Json(rows, JsonRequestBehavior.AllowGet);

        }


        //删除选中的记录
        [HttpPost]
        public async Task<JsonResult> DeleteCheckedAsync(int[] id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            try
            {
                await this.companyService.Queryable().Where(x => id.Contains(x.Id)).DeleteAsync();
                return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //导出Excel
        [HttpPost]
        public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "companies_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = this.companyService.ExportExcel(filterRules, sort, order);
            return this.File(stream, "application/vnd.ms-excel", fileName);
        }
        private void DisplaySuccessMessage(string msgText) => this.TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => this.TempData["ErrorMessage"] = msgText;

    }
}
