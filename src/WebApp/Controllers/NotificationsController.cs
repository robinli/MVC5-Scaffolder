///<summary>
///Provides functionality to the /Notification/ route.
///<date> 9/7/2018 10:08:09 AM </date>
///Create By SmartCode MVC5 Scaffolder for Visual Studio
///TODO: RegisterType UnityConfig.cs
///container.RegisterType<IRepositoryAsync<Notification>, Repository<Notification>>();
///container.RegisterType<INotificationService, NotificationService>();
///
///Copyright (c) 2012-2018 neo.zhu
///Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
///and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
///</summary>
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
	public class NotificationsController : Controller
	{
		private readonly INotificationService  notificationService;
		private readonly IUnitOfWorkAsync unitOfWork;
		public NotificationsController (INotificationService  notificationService, IUnitOfWorkAsync unitOfWork)
		{
			this.notificationService  = notificationService;
			this.unitOfWork = unitOfWork;
		}
        		//GET: Notifications/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public ActionResult Index()
		{
			 return View();
		}
		//Get :Notifications/GetData
		//For Index View datagrid datasource url
		[HttpGet]
		 public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var totalCount = 0;
			//int pagenum = offset / limit +1;
			var pagerows  = (await this.notificationService
						               .Query(new NotificationQuery().Withfilter(filters))
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out totalCount))
                                       .Select(  n => new { 

    Id = n.Id,
    Title = n.Title,
    Content = n.Content,
    Link = n.Link,
    Read = n.Read,
    From = n.From,
    To = n.To,
    Group = n.Group,
    Created = n.Created,
    Creator = n.Creator,
    CreatedDate = n.CreatedDate,
    CreatedBy = n.CreatedBy,
    LastModifiedDate = n.LastModifiedDate,
    LastModifiedBy = n.LastModifiedBy
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
                 //easyui datagrid post acceptChanges 
		[HttpPost]
		public async Task<JsonResult> SaveDataAsync(NotificationChangeViewModel notifications)
		{
            if (notifications == null)
            {
                throw new ArgumentNullException(nameof(notifications));
            }
			if (notifications.updated != null)
			{
				foreach (var item in notifications.updated)
				{
					this.notificationService.Update(item);
				}
			}
			if (notifications.deleted != null)
			{
				foreach (var item in notifications.deleted)
				{
					this.notificationService.Delete(item);
				}
			}
			if (notifications.inserted != null)
			{
				foreach (var item in notifications.inserted)
				{
					this.notificationService.Insert(item);
				}
			}
			var result = await this.unitOfWork.SaveChangesAsync();
			return Json(new {success=true,result=result}, JsonRequestBehavior.AllowGet);
		}
						//GET: Notifications/Details/:id
		public ActionResult Details(int id)
		{
			
			var notification = this.notificationService.Find(id);
			if (notification == null)
			{
				return HttpNotFound();
			}
			return View(notification);
		}
        //GET: Notifications/GetItemAsync/:id
        [HttpGet]
        public async Task<JsonResult> GetItemAsync(int id) {
            var  notification = await this.notificationService.FindAsync(id);
            return Json(notification,JsonRequestBehavior.AllowGet);
        }
		//GET: Notifications/Create
        		public ActionResult Create()
				{
			var notification = new Notification();
			//set default value
			return View(notification);
		}
		//POST: Notifications/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateAsync(Notification notification)
		{
			if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            } 
            if (ModelState.IsValid)
			{
				notificationService.Insert(notification);
				var result = await this.unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true,result=result }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has append a Notification record");
				return RedirectToAction("Index");
			}
			else {
			 var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			 if (Request.IsAjaxRequest())
			 {
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			 }
			 DisplayErrorMessage(modelStateErrors);
			}
			return View(notification);
		}
        //GET: Notifications/PopupEdit/:id
        //[OutputCache(Duration = 360, VaryByParam = "id")]
		public async Task<JsonResult> PopupEditAsync(int id)
		{
			
			var notification = await this.notificationService.FindAsync(id);
			return Json(notification,JsonRequestBehavior.AllowGet);
		}

		//GET: Notifications/Edit/:id
		public ActionResult Edit(int id)
		{
			var notification = this.notificationService.Find(id);
			if (notification == null)
			{
				return HttpNotFound();
			}
			return View(notification);
		}
		//POST: Notifications/Edit/:id
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditAsync(Notification notification)
		{
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
			if (ModelState.IsValid)
			{
				notification.TrackingState = TrackingState.Modified;
								notificationService.Update(notification);
								var result = await this.unitOfWork.SaveChangesAsync();
				if (Request.IsAjaxRequest())
				{
					return Json(new { success = true,result = result }, JsonRequestBehavior.AllowGet);
				}
				DisplaySuccessMessage("Has update a Notification record");
				return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			if (Request.IsAjaxRequest())
			{
				return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			}
			DisplayErrorMessage(modelStateErrors);
			}
						return View(notification);
		}
		//GET: Notifications/Delete/:id
		public async Task<ActionResult> DeleteAsync(int id)
		{
			var notification = await this.notificationService.FindAsync(id);
			if (notification == null)
			{
				return HttpNotFound();
			}
			return View(notification);
		}
		//POST: Notifications/Delete/:id
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var notification = await  this.notificationService.FindAsync(id);
			 this.notificationService.Delete(notification);
			var result = await this.unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true,result=result }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a Notification record");
			return RedirectToAction("Index");
		}
       
 

        //删除选中的记录
        [HttpPost]
        public async Task<JsonResult> DeleteCheckedAsync(int[] id) {
           if (id == null)
           {
                throw new ArgumentNullException(nameof(id));
           }
           await this.notificationService.Queryable().Where(x => id.Contains(x.Id)).DeleteAsync();
           return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }
		//导出Excel
		[HttpPost]
		public ActionResult ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "notifications_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  this.notificationService.ExportExcel(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		 private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;
		 
	}
}
