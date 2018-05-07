// <summary>
// <date> 4/8/2018 4:52:48 PM </date>
// Create By SmartCode MVC5 Scaffolder for Visual Studio
// TODO: RegisterType UnityConfig.cs
// container.RegisterType<IRepositoryAsync<Department>, Repository<Department>>();
// container.RegisterType<IDepartmentService, DepartmentService>();
//
// Copyright (c) 2012-2018 neo.zhu
// Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
// and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
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
/// <summary>
///
/// </summary>
namespace WebApp.Controllers
{
    //[Authorize]
    public class DepartmentsController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly IDepartmentService _departmentService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public DepartmentsController(IDepartmentService departmentService, IUnitOfWorkAsync unitOfWork)
        {
            _departmentService = departmentService;
            _unitOfWork = unitOfWork;
        }
        // GET: Departments/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }
        // Get :Departments/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            //int pagenum = offset / limit +1;
            var departments = await _departmentService
       .Query(new DepartmentQuery().Withfilter(filters)).Include(d => d.Company)
       .OrderBy(n => n.OrderBy(sort, order))
       .SelectPageAsync(page, rows, out totalCount);
            var datarows = departments.Select(n => new { CompanyName = (n.Company == null ? "" : n.Company.Name), Id = n.Id, Name = n.Name, Manager = n.Manager, CompanyId = n.CompanyId }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> GetDataByCompanyId(int companyid, int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            var departments = await _departmentService
                       .Query(new DepartmentQuery().ByCompanyIdWithfilter(companyid, filters)).Include(d => d.Company)
                       .OrderBy(n => n.OrderBy(sort, order))
                       .SelectPageAsync(page, rows, out totalCount);
            var datarows = departments.Select(n => new { CompanyName = (n.Company == null ? "" : n.Company.Name), Id = n.Id, Name = n.Name, Manager = n.Manager, CompanyId = n.CompanyId }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> SaveData(DepartmentChangeViewModel departments)
        {
            if (departments.updated != null)
            {
                foreach (var item in departments.updated)
                {
                    _departmentService.Update(item);
                }
            }
            if (departments.deleted != null)
            {
                foreach (var item in departments.deleted)
                {
                    _departmentService.Delete(item);
                }
            }
            if (departments.inserted != null)
            {
                foreach (var item in departments.inserted)
                {
                    _departmentService.Insert(item);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
        //[OutputCache(Duration = 360, VaryByParam = "none")]
        public async Task<JsonResult> GetCompanies(string q = "")
        {
            var companyRepository = _unitOfWork.RepositoryAsync<Company>();
            var data = await companyRepository.Queryable().Where(n => n.Name.Contains(q)).ToListAsync();
            var rows = data.Select(n => new { Id = n.Id, Name = n.Name });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        // GET: Departments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = await _departmentService.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }
        // GET: Departments/Create
        public ActionResult Create()
        {
            var department = new Department();
            //set default value
            var companyRepository = _unitOfWork.RepositoryAsync<Company>();
            ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name");
            return View(department);
        }
        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Company,Id,Name,Manager,CompanyId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentService.Insert(department);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Department record");
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
            var companyRepository = _unitOfWork.RepositoryAsync<Company>();
            ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name", department.CompanyId);
            return View(department);
        }
        // GET: Departments/PopupEdit/5
        //[OutputCache(Duration = 360, VaryByParam = "id")]
        public async Task<JsonResult> PopupEdit(int? id)
        {

            var department = await _departmentService.FindAsync(id);
            return Json(department, JsonRequestBehavior.AllowGet);
        }

        // GET: Departments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = await _departmentService.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            var companyRepository = _unitOfWork.RepositoryAsync<Company>();
            ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name", department.CompanyId);
            return View(department);
        }
        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Company,Id,Name,Manager,CompanyId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.ObjectState = ObjectState.Modified;
                _departmentService.Update(department);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Department record");
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
            var companyRepository = _unitOfWork.RepositoryAsync<Company>();
            ViewBag.CompanyId = new SelectList(await companyRepository.Queryable().ToListAsync(), "Id", "Name", department.CompanyId);
            return View(department);
        }
        // GET: Departments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = await _departmentService.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }
        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var department = await _departmentService.FindAsync(id);
            _departmentService.Delete(department);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Department record");
            return RedirectToAction("Index");
        }


        //导出Excel
        [HttpPost]
        public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "departments_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = _departmentService.ExportExcel(filterRules, sort, order);
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
