


using System;
using Newtonsoft.Json;
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


namespace WebApp.Controllers
{
    public class WorksController : Controller
    {
        
        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<Work>, Repository<Work>>();
        //container.RegisterType<IWorkService, WorkService>();
        
        //private StoreContext db = new StoreContext();
        private readonly IWorkService  _workService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public WorksController (IWorkService  workService, IUnitOfWorkAsync unitOfWork)
        {
            _workService  = workService;
            _unitOfWork = unitOfWork;
        }

        // GET: Works/Index
        public ActionResult Index()
        {
            
            //var works  = _workService.Queryable().AsQueryable();
            //return View(works  );
			return View();
        }

        // Get :Works/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            int totalCount = 0;
            //int pagenum = offset / limit +1;
                        var works  = _workService.Query(new WorkQuery().Withfilter(filters)).OrderBy(n=>n.OrderBy(sort,order)).SelectPage(page, rows, out totalCount);
                        var datarows = works .Select(  n => new {  Name = n.Name , Status = n.Status , StartDate = n.StartDate , EndDate = n.EndDate , Enableed = n.Enableed , Hour = n.Hour , Priority = n.Priority , Score = n.Score }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
		public ActionResult SaveData(WorkChangeViewModel works)
        {
            if (works.updated != null)
            {
                foreach (var updated in works.updated)
                {
                    _workService.Update(updated);
                }
            }
            if (works.deleted != null)
            {
                foreach (var deleted in works.deleted)
                {
                    _workService.Delete(deleted);
                }
            }
            if (works.inserted != null)
            {
                foreach (var inserted in works.inserted)
                {
                    _workService.Insert(inserted);
                }
            }
            _unitOfWork.SaveChanges();

            return Json(new {Success=true}, JsonRequestBehavior.AllowGet);
        }

		
		
       
        // GET: Works/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = _workService.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }
        

        // GET: Works/Create
        public ActionResult Create()
        {
            Work work = new Work();
            //set default value
            return View(work);
        }

        // POST: Works/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Status,StartDate,EndDate,Enableed,Hour,Priority,Score")] Work work)
        {
            if (ModelState.IsValid)
            {
             				_workService.Insert(work);
                           _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Work record");
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(work);
        }

        // GET: Works/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = _workService.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Status,StartDate,EndDate,Enableed,Hour,Priority,Score")] Work work)
        {
            if (ModelState.IsValid)
            {
                work.ObjectState = ObjectState.Modified;
                				_workService.Update(work);
                                
                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Work record");
                return RedirectToAction("Index");
            }
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(work);
        }

        // GET: Works/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Work work = _workService.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Work work =  _workService.Find(id);
             _workService.Delete(work);
            _unitOfWork.SaveChanges();
           if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            DisplaySuccessMessage("Has delete a Work record");
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
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
