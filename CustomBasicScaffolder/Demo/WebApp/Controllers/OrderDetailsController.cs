


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
    public class OrderDetailsController : Controller
    {
        
        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<OrderDetail>, Repository<OrderDetail>>();
        //container.RegisterType<IOrderDetailService, OrderDetailService>();
        
        //private StoreContext db = new StoreContext();
        private readonly IOrderDetailService  _orderDetailService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public OrderDetailsController (IOrderDetailService  orderDetailService, IUnitOfWorkAsync unitOfWork)
        {
            _orderDetailService  = orderDetailService;
            _unitOfWork = unitOfWork;
        }

        // GET: OrderDetails/Index
        public ActionResult Index()
        {
            
            //var orderdetails  = _orderDetailService.Queryable().Include(o => o.Order).Include(o => o.Product).AsQueryable();
            
             //return View(orderdetails);
			 return View();
        }

        // Get :OrderDetails/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult GetData(int page = 1, int rows = 10, string sort = "Id", string order = "desc", string filterRules = "")
        {
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            int totalCount = 0;
            //int pagenum = offset / limit +1;
            			 
            var orderdetails  = _orderDetailService.Query(new OrderDetailQuery().Withfilter(filters)).Include(o => o.Order).Include(o => o.Product).OrderBy(n=>n.OrderBy(sort,order)).SelectPage(page, rows, out totalCount);
            
                        var datarows = orderdetails .Select(  n => new { OrderCustomer = (n.Order==null?"": n.Order.Customer) ,ProductName = (n.Product==null?"": n.Product.Name) , Id = n.Id , ProductId = n.ProductId , Qty = n.Qty , Price = n.Price , Amount = n.Amount , OrderId = n.OrderId }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
		public ActionResult SaveData(OrderDetailChangeViewModel orderdetails)
        {
            if (orderdetails.updated != null)
            {
                foreach (var updated in orderdetails.updated)
                {
                    _orderDetailService.Update(updated);
                }
            }
            if (orderdetails.deleted != null)
            {
                foreach (var deleted in orderdetails.deleted)
                {
                    _orderDetailService.Delete(deleted);
                }
            }
            if (orderdetails.inserted != null)
            {
                foreach (var inserted in orderdetails.inserted)
                {
                    _orderDetailService.Insert(inserted);
                }
            }
            _unitOfWork.SaveChanges();

            return Json(new {Success=true}, JsonRequestBehavior.AllowGet);
        }

				public ActionResult GetOrders()
        {
            var orderRepository = _unitOfWork.Repository<Order>();
            var data = orderRepository.Queryable().ToList();
            var rows = data.Select(n => new { Id = n.Id, Customer = n.Customer });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
				public ActionResult GetProducts()
        {
            var productRepository = _unitOfWork.Repository<Product>();
            var data = productRepository.Queryable().ToList();
            var rows = data.Select(n => new { Id = n.Id, Name = n.Name });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
		
		
       
        // GET: OrderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = _orderDetailService.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }
        

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            OrderDetail orderDetail = new OrderDetail();
            //set default value
            var orderRepository = _unitOfWork.Repository<Order>();
            ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer");
            var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");
            return View(orderDetail);
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Order,Product,Id,ProductId,Qty,Price,Amount,OrderId")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
             				_orderDetailService.Insert(orderDetail);
                           _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a OrderDetail record");
                return RedirectToAction("Index");
            }

            var orderRepository = _unitOfWork.Repository<Order>();
            ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer", orderDetail.OrderId);
            var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name", orderDetail.ProductId);
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = _orderDetailService.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            var orderRepository = _unitOfWork.Repository<Order>();
            ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer", orderDetail.OrderId);
            var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name", orderDetail.ProductId);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Order,Product,Id,ProductId,Qty,Price,Amount,OrderId")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                orderDetail.ObjectState = ObjectState.Modified;
                				_orderDetailService.Update(orderDetail);
                                
                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a OrderDetail record");
                return RedirectToAction("Index");
            }
            var orderRepository = _unitOfWork.Repository<Order>();
            ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer", orderDetail.OrderId);
            var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name", orderDetail.ProductId);
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = _orderDetailService.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderDetail =  _orderDetailService.Find(id);
             _orderDetailService.Delete(orderDetail);
            _unitOfWork.SaveChanges();
           if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            DisplaySuccessMessage("Has delete a OrderDetail record");
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
