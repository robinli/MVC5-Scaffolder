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
 
using Z.EntityFramework.Plus;
using WebApp.App_Start;

namespace WebApp.Controllers
{
    public class MessagesController : Controller
    {

        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<Message>, Repository<Message>>();
        //container.RegisterType<IMessageService, MessageService>();

        //private StoreContext db = new StoreContext();
        private readonly IMessageService _messageService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public MessagesController(IMessageService messageService, IUnitOfWorkAsync unitOfWork)
        {
            _messageService = messageService;
            _unitOfWork = unitOfWork;
        }

        // GET: Messages/Index
        public async Task<ActionResult> Index()
        {

            ViewBag.TotalSysError = await _messageService.Queryable().Where(x => x.Group == (int)MessageGroup.System &&
               x.Type == (int)MessageType.Error).CountAsync();


            ViewBag.TotalOpError = await _messageService.Queryable().Where(x => x.Group == (int)MessageGroup.Operator &&
               x.Type == (int)MessageType.Error).CountAsync();
            ViewBag.TotalInterfaceError = await _messageService.Queryable().Where(x => x.Group == (int)MessageGroup.Intface &&
               x.Type == (int)MessageType.Error).CountAsync();
            ViewBag.TotalNewError = await _messageService.Queryable().Where(x =>
               x.Type == (int)MessageType.Error && x.IsNew == 0).CountAsync();



            return View();

        }

        // Get :Messages/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {

            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            //int pagenum = offset / limit +1;
            var messages = await _messageService
                        .Query(new MessageQuery().Withfilter(filters))
                        .OrderBy(n => n.OrderBy(sort, order))
                        .SelectPageAsync(page, rows, out totalCount);


            var datarows = messages.Select(n => new
            {
                Id = n.Id,
                Group = n.Group,
                StackTrace = n.StackTrace,
                ExtensionKey1 = n.ExtensionKey1,
                Type = n.Type,
                Code = n.Code,
                Content = n.Content,
                ExtensionKey2 = n.ExtensionKey2,
                Tags = n.Tags,
                Method = n.Method,
                Created = n.Created,
                IsNew = n.IsNew,
                User = n.User,
                IsNotification = n.IsNotification
            }
            ).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SaveData(MessageChangeViewModel messages)
        {
            if (messages.updated != null)
            {
                foreach (var updated in messages.updated)
                {
                    _messageService.Update(updated);
                }
            }
            if (messages.deleted != null)
            {
                foreach (var deleted in messages.deleted)
                {
                    _messageService.Delete(deleted);
                }
            }
            if (messages.inserted != null)
            {
                foreach (var inserted in messages.inserted)
                {
                    _messageService.Insert(inserted);
                }
            }
            await _unitOfWork.SaveChangesAsync();

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Notify() {

            return PartialView("_notifications");
        }

        // GET: Messages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var message = await _messageService.FindAsync(id);
            await _messageService.Queryable().Where(x => x.Id == id).UpdateAsync(x => new Message() { IsNew = 1 });

            if (message == null)
            {
                return HttpNotFound();
            }
            return PartialView(message);
        }


        // GET: Messages/Create
        public ActionResult Create()
        {
            var message = new Message();
            //set default value
            return View(message);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Group,ExtensionKey1,Type,Code,Content,ExtensionKey2,Tags,Method,Created,IsNew,IsNotification")] Message message)
        {
            if (ModelState.IsValid)
            {
                _messageService.Insert(message);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Message record");
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

            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var message = await _messageService.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Group,ExtensionKey1,Type,Code,Content,ExtensionKey2,Tags,Method,Created,IsNew,IsNotification")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.ObjectState = ObjectState.Modified;
                _messageService.Update(message);

                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Message record");
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

            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var message = await _messageService.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var message = await _messageService.FindAsync(id);
            _messageService.Delete(message);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Message record");
            return RedirectToAction("Index");
        }






        //导出Excel
        [HttpPost]
        public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "messages_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = _messageService.ExportExcel(filterRules, sort, order);
            return File(stream, "application/vnd.ms-excel", fileName);

        }

        [HttpPost]
        public async Task<ActionResult> SaveErrorLog(string tags,string content, string url="", string line="", string col="")
        {
            var message = new Message()
            {
                Group = (int)MessageGroup.Operator,
                Content = $"{content} at line:{line} col:{col}.",
                Type = (int)MessageType.Error,
                Method = SubUrlString(url),
                Tags = tags,
                ExtensionKey1 = "js",
                User = Auth.CurrentUserName



            };
            _messageService.Insert(message);
            await _unitOfWork.SaveChangesAsync();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        private string SubUrlString(string url)
        {
            string[] spstr = url.Split(new char[] { '/' });
            var strPath = string.Empty;
            for (var i = 0; i < spstr.Length; i++)
            {
                if (i >= 3)
                    strPath += spstr[i] + "/";
            }
            return strPath;
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

