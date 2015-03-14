


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using WebApp.Extensions;
using PagedList;

namespace WebApp.Controllers
{
    public class CompaniesController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly ICompanyService _companyService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public CompaniesController(ICompanyService companyService, IUnitOfWorkAsync unitOfWork)
        {
            _companyService = companyService;
            _unitOfWork = unitOfWork;
        }

        // GET: Companies/Index
        public ActionResult Index()
        {

            var companies = _companyService.Queryable().AsQueryable();
            return View(companies);
        }

        // Get :Companies/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit + 1;
            var companies = _companyService.Query(new CompanyQuery().WithAnySearch(search)).OrderBy(n => n.OrderBy(sort, order)).SelectPage(pagenum, limit, out totalCount);
            var rows = companies.Select(n => new { Id = n.Id, Name = n.Name, Address = n.Address, City = n.City, Province = n.Province, RegisterDate = n.RegisterDate, Employees = n.Employees }).ToList();
            var pagelist = new { total = totalCount, rows = rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }


        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _companyService.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }


        // GET: Companies/Create
        public ActionResult Create()
        {
            Company company = new Company();
            //set default value
            return View(company);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Departments,Employee,Id,Name,Address,City,Province,RegisterDate,Employees")] Company company)
        {
            if (ModelState.IsValid)
            {
                company.ObjectState = ObjectState.Added;
                foreach (var item in company.Departments)
                {
                    item.CompanyId = company.Id;
                    item.ObjectState = ObjectState.Added;
                }
                foreach (var item in company.Employee)
                {
                    item.CompanyId = company.Id;
                    item.ObjectState = ObjectState.Added;
                }
                _companyService.InsertOrUpdateGraph(company);
                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Company record");
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _companyService.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Departments,Employee,Id,Name,Address,City,Province,RegisterDate,Employees")] Company company)
        {
            if (ModelState.IsValid)
            {
                company.ObjectState = ObjectState.Modified;
                foreach (var item in company.Departments)
                {
                    item.CompanyId = company.Id;
                    //set ObjectState with conditions
                    if (item.Id <= 0)
                        item.ObjectState = ObjectState.Added;
                    else
                        item.ObjectState = ObjectState.Modified;
                }
                foreach (var item in company.Employee)
                {
                    item.CompanyId = company.Id;
                    //set ObjectState with conditions
                    if (item.Id <= 0)
                        item.ObjectState = ObjectState.Added;
                    else
                        item.ObjectState = ObjectState.Modified;
                }

                _companyService.InsertOrUpdateGraph(company);

                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Company record");
                return RedirectToAction("Index");
            }
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _companyService.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = _companyService.Find(id);
            _companyService.Delete(company);
            _unitOfWork.SaveChanges();
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
        public ActionResult EditDepartment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var departmentRepository = _unitOfWork.Repository<Department>();
            var department = departmentRepository.Find(id);

            var companyRepository = _unitOfWork.Repository<Company>();

            if (department == null)
            {
                ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name");

                //return HttpNotFound();
                return PartialView("_DepartmentEditForm", new Department());
            }
            else
            {
                ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name", department.CompanyId);

            }
            return PartialView("_DepartmentEditForm", department);

        }

        // Get Create Row By Id For Edit
        // Get : Companies/CreateDepartment
        [HttpGet]
        public ActionResult CreateDepartment()
        {
            var companyRepository = _unitOfWork.Repository<Company>();
            ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name");
            return PartialView("_DepartmentEditForm");

        }

        // Post Delete Detail Row By Id
        // Get : Companies/DeleteDepartment/:id
        [HttpPost, ActionName("DeleteDepartment")]
        public ActionResult DeleteDepartmentConfirmed(int id)
        {
            var departmentRepository = _unitOfWork.Repository<Department>();
            departmentRepository.Delete(id);
            _unitOfWork.SaveChanges();
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
        public ActionResult EditEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employeeRepository = _unitOfWork.Repository<Employee>();
            var employee = employeeRepository.Find(id);

            var companyRepository = _unitOfWork.Repository<Company>();

            if (employee == null)
            {
                ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name");

                //return HttpNotFound();
                return PartialView("_EmployeeEditForm", new Employee());
            }
            else
            {
                ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name", employee.CompanyId);

            }
            return PartialView("_EmployeeEditForm", employee);

        }

        // Get Create Row By Id For Edit
        // Get : Companies/CreateEmployee
        [HttpGet]
        public ActionResult CreateEmployee()
        {
            var companyRepository = _unitOfWork.Repository<Company>();
            ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name");
            return PartialView("_EmployeeEditForm");

        }

        // Post Delete Detail Row By Id
        // Get : Companies/DeleteEmployee/:id
        [HttpPost, ActionName("DeleteEmployee")]
        public ActionResult DeleteEmployeeConfirmed(int id)
        {
            var employeeRepository = _unitOfWork.Repository<Employee>();
            employeeRepository.Delete(id);
            _unitOfWork.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Order record");
            return RedirectToAction("Index");
        }



        // Get : Companies/GetDepartmentsByCompanyId/:id
        [HttpGet]
        public ActionResult GetDepartmentsByCompanyId(int id)
        {
            var departments = _companyService.GetDepartmentsByCompanyId(id);
            if (Request.IsAjaxRequest())
            {
                return Json(departments.Select(n => new { CompanyName = n.Company.Name, Id = n.Id, Name = n.Name, Manager = n.Manager, CompanyId = n.CompanyId }), JsonRequestBehavior.AllowGet);
            }
            return View(departments);

        }
        // Get : Companies/GetEmployeeByCompanyId/:id
        [HttpGet]
        public ActionResult GetEmployeeByCompanyId(int id)
        {
            var employee = _companyService.GetEmployeeByCompanyId(id);
            if (Request.IsAjaxRequest())
            {
                return Json(employee.Select(n => new { CompanyName = n.Company.Name, Id = n.Id, Name = n.Name, Sex = n.Sex, Age = n.Age, Brithday = n.Brithday, CompanyId = n.CompanyId }), JsonRequestBehavior.AllowGet);
            }
            return View(employee);

        }


        private void DisplaySuccessMessage(string msgText)
        {
            TempData["SuccessMessage"] = msgText;
        }

        private void DisplayErrorMessage()
        {
            TempData["ErrorMessage"] = "Save changes was unsuccessful.";
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
