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
using WebApp.Extensions;


namespace WebApp.Controllers
{
    public class WorksController : Controller
    {

        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<Work>, Repository<Work>>();
        //container.RegisterType<IWorkService, WorkService>();

        //private StoreContext db = new StoreContext();
        private readonly IWorkService _workService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public WorksController(IWorkService workService, IUnitOfWorkAsync unitOfWork)
        {
            _workService = workService;
            _unitOfWork = unitOfWork;
        }

        // GET: Works/Index
        public async Task<ActionResult> Index()
        {

            var works = _workService.Queryable();
            return View(await works.ToListAsync());

        }

        // Get :Works/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            //int pagenum = offset / limit +1;
            var works = await _workService
       .Query(new WorkQuery().Withfilter(filters))
       .OrderBy(n => n.OrderBy(sort, order))
       .SelectPageAsync(page, rows, out totalCount);

            var datarows = works.Select(n => new { Id = n.Id, Name = n.Name, Status = n.Status, StartDate = n.StartDate, EndDate = n.EndDate, Enableed = n.Enableed, Hour = n.Hour, Priority = n.Priority, Score = n.Score }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public async Task<ActionResult> SaveData(WorkChangeViewModel works)
        {
            if (works.updated != null)
            {
                foreach (var item in works.updated)
                {
                    _workService.Update(item);
                }
            }
            if (works.deleted != null)
            {
                foreach (var item in works.deleted)
                {
                    _workService.Delete(item);
                }
            }
            if (works.inserted != null)
            {
                foreach (var item in works.inserted)
                {
                    _workService.Insert(item);
                }
            }
         


            await _unitOfWork.SaveChangesAsync();

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }




        // GET: Works/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var work = await _workService.FindAsync(id);

            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }


        // GET: Works/Create
        public ActionResult Create()
        {
            var work = new Work();
            //set default value
            return View(work);
        }

        // POST: Works/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Status,StartDate,EndDate,Enableed,Hour,Priority,Score")] Work work)
        {
            if (ModelState.IsValid)
            {
                _workService.Insert(work);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Work record");
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

            return View(work);
        }

        // GET: Works/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var work = await _workService.FindAsync(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Status,StartDate,EndDate,Enableed,Hour,Priority,Score")] Work work)
        {
            if (ModelState.IsValid)
            {
                work.ObjectState = ObjectState.Modified;
                _workService.Update(work);

                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Work record");
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

            return View(work);
        }

        // GET: Works/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var work = await _workService.FindAsync(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var work = await _workService.FindAsync(id);
            _workService.Delete(work);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Work record");
            return RedirectToAction("Index");
        }






        //导出Excel
        [HttpPost]
        public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "works_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = _workService.ExportExcel(filterRules, sort, order);
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
