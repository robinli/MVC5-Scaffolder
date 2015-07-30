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
using PagedList;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace WebApp.Controllers
{
    /*
    public class JsonNetResult : JsonResult
    {
        public JsonSerializerSettings Settings { get; private set; }

        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                //这句是解决问题的关键,也就是json.net官方给出的解决配置选项.
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
            string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(this.ContentType) ?
             "application/json" : this.ContentType;
            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;
            var scriptSerializer = JsonSerializer.Create(this.Settings);
            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, this.Data);
                response.Write(sw.ToString());
            }
        }
    }
    public class OrdersController__ : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly IOrderService  _orderService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public OrdersController__ (IOrderService  orderService, IUnitOfWorkAsync unitOfWork)
        {
            _orderService  = orderService;
            _unitOfWork = unitOfWork;
        }

        // GET: Orders/Index
        public ActionResult Index(string sortOrder, string currentFilter, string search_field, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            if (searchString != null)
            {
                page = 1;
                currentFilter = searchString;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            //string expression = string.Format("{0} = '{1}'", search_field, searchString);
            //ViewBag.SearchField = search_field;
            var order  = _orderService.Queryable().OrderBy(n=>n.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                order  = order .Where(n => n.ShippingAddress.Contains(searchString));
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(order .ToPagedList(pageNumber, pageSize));
        }

       
        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _orderService.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        

        // GET: Orders/Create
        public ActionResult Create()
        {
            var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDetails,Id,Customer,ShippingAddress,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            { 
                foreach (var detail in order.OrderDetails)
                {
                    detail.ObjectState = ObjectState.Added;
                    //detail.Product = null;
                    detail.Product.ObjectState = ObjectState.Detached;
                    
                }
                order.ObjectState = ObjectState.Added;
                _orderService.InsertOrUpdateGraph(order);
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has append a Order record");
                return RedirectToAction("Index");
                //return Json("");
            }
            var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");
            DisplayErrorMessage();
            return View(order);
            //return Json("");
        }
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding,
        JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //return null;
            }
            //Order order = _orderService.Query().Include(n=>n.OrderDetails.Select(p=>p.Product)).Select().Where(n=>n.Id==id).First();
            Order order = _orderService.Find(id);

            var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");
          
            ViewBag.OrderDetails = order.OrderDetails.Select(n => new { OrderId = n.OrderId, Id = n.Id, ProductId = n.ProductId, ProductName = n.Product.Name,Qty=n.Qty,Price=n.Price, Amount=n.Amount });
            if (order == null)
            {
                return HttpNotFound();
            }
            //return Json(order,"", Encoding.UTF8 , JsonRequestBehavior.AllowGet);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetails,Id,Customer,ShippingAddress,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.ObjectState = ObjectState.Modified;
                _orderService.Update(order);
               
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has update a Order record");
                return RedirectToAction("Index");
            }
            var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");
            DisplayErrorMessage();
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _orderService.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order =  _orderService.Find(id);
             _orderService.Delete(order);
            _unitOfWork.SaveChanges();
            DisplaySuccessMessage("Has delete a Order record");
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
    }*/
}
