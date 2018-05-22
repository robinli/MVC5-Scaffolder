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
using TrackableEntities;

namespace WebApp.Controllers
{
    public class NotificationsController : Controller
    {

        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<Notification>, Repository<Notification>>();
        //container.RegisterType<INotificationService, NotificationService>();

        //private ApplicationDbContext db = new ApplicationDbContext();
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public NotificationsController(INotificationService notificationService, IUnitOfWorkAsync unitOfWork)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        // GET: Notifications/Index
        public async Task<ActionResult> Index()
        {

            var notifications = _notificationService.Queryable();
            return View(await notifications.ToListAsync());

        }

        // Get :Notifications/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            //int pagenum = offset / limit +1;
            var notifications = await _notificationService
                        .Query(new NotificationQuery().Withfilter(filters))
                        .OrderBy(n => n.OrderBy(sort, order))
                        .SelectPageAsync(page, rows, out totalCount);


            var datarows = notifications.Select(n => new { Id = n.Id, Title = n.Title, Content = n.Content, Link = n.Link, Read = n.Read, From = n.From, To = n.To, Created = n.Created, Creator = n.Creator }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [ChildActionOnly]
        public ActionResult DisplayNotification()
        {
            var item = new Notification();
            var query = _notificationService.Queryable().Where(x => x.Read == 0)
                    .GroupBy(x => x.Title)
                    .Select(x => new NotificationViewModel()
                    {
                        Group = x.Min(n => n.Group),
                        Link = x.Min(n => n.Link),
                        Time = x.Min(n => n.Created),
                        Title = x.Count().ToString() + " " + x.Key

                    });
            ViewBag.Count = _notificationService.Queryable().Where(x => x.Read == 0).Count();

            return PartialView("_NotificationView", query.ToList());
        }




        [HttpPost]
        public async Task<ActionResult> SaveData(NotificationChangeViewModel notifications)
        {
            if (notifications.updated != null)
            {
                foreach (var item in notifications.updated)
                {
                    _notificationService.Update(item);
                }
            }
            if (notifications.deleted != null)
            {
                foreach (var item in notifications.deleted)
                {
                    _notificationService.Delete(item);
                }
            }
            if (notifications.inserted != null)
            {
                foreach (var item in notifications.inserted)
                {
                    _notificationService.Insert(item);
                }
            }
            await _unitOfWork.SaveChangesAsync();

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }




        // GET: Notifications/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var notification = await _notificationService.FindAsync(id);

            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }


        // GET: Notifications/Create
        public ActionResult Create()
        {
            var notification = new Notification();
            //set default value
            return View(notification);
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Content,Link,Read,From,To,Created,Creator")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                _notificationService.Insert(notification);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Notification record");
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

            return View(notification);
        }

        // GET: Notifications/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var notification = await _notificationService.FindAsync(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Content,Link,Read,From,To,Created,Creator")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                notification.TrackingState = TrackingState.Modified;
                _notificationService.Update(notification);

                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Notification record");
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

            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var notification = await _notificationService.FindAsync(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var notification = await _notificationService.FindAsync(id);
            _notificationService.Delete(notification);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Notification record");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Notify() {

            return PartialView("_notifications");
        }






        //导出Excel
        [HttpPost]
        public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "notifications_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = _notificationService.ExportExcel(filterRules, sort, order);
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

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _unitOfWork.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
