


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
    public class DepartmentsController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly IDepartmentService  _departmentService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public DepartmentsController (IDepartmentService  departmentService, IUnitOfWorkAsync unitOfWork)
        {
            _departmentService  = departmentService;
            _unitOfWork = unitOfWork;
        }

        // GET: Departments/Index
        public ActionResult Index()
        {
            
            var departments  = _departmentService.Queryable().Include(d => d.Company).AsQueryable();
            
             return View(departments);
        }

        // Get :Departments/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit +1;
                        var departments  = _departmentService.Query(new DepartmentQuery().WithAnySearch(search)).Include(d => d.Company).OrderBy(n=>n.OrderBy(sort,order)).SelectPage(pagenum, limit, out totalCount);
            
                        var rows = departments .Select(  n => new { CompanyName = n.Company.Name , Id = n.Id , Name = n.Name , Manager = n.Manager , CompanyId = n.CompanyId }).ToList();
            var pagelist = new { total = totalCount, rows = rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

       
        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _departmentService.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }
        

        // GET: Departments/Create
        public ActionResult Create()
        {
            Department department = new Department();
            //set default value
            var companyRepository = _unitOfWork.Repository<Company>();
            ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name");
            return View(department);
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Company,Id,Name,Manager,CompanyId")] Department department)
        {
            if (ModelState.IsValid)
            {
             				_departmentService.Insert(department);
                           _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Department record");
                return RedirectToAction("Index");
            }

            var companyRepository = _unitOfWork.Repository<Company>();
            ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name", department.CompanyId);
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _departmentService.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            var companyRepository = _unitOfWork.Repository<Company>();
            ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name", department.CompanyId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Company,Id,Name,Manager,CompanyId")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.ObjectState = ObjectState.Modified;
                				_departmentService.Update(department);
                                
                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Department record");
                return RedirectToAction("Index");
            }
            var companyRepository = _unitOfWork.Repository<Company>();
            ViewBag.CompanyId = new SelectList(companyRepository.Queryable(), "Id", "Name", department.CompanyId);
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _departmentService.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department =  _departmentService.Find(id);
             _departmentService.Delete(department);
            _unitOfWork.SaveChanges();
           if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            DisplaySuccessMessage("Has delete a Department record");
            return RedirectToAction("Index");
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
                //_unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
