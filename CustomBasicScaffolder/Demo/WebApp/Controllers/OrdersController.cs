

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
using WebApp.Extensions;
namespace WebApp.Controllers
{
    public class OrdersController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly IOrderService  _orderService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public OrdersController (IOrderService  orderService, IUnitOfWorkAsync unitOfWork)
        {
            _orderService  = orderService;
            _unitOfWork = unitOfWork;
        }

        // GET: Orders/Index
        public ActionResult Index()
        {
            
            var order  = _orderService.Queryable().AsQueryable();
            return View(order  );
        }

        // Get :Orders/PageList
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit +1;
                        var orders  = _orderService.Query(new OrderQuery().WithAnySearch(search)).OrderBy(n=>n.OrderBy(sort,order)).SelectPage(pagenum, limit, out totalCount);
                        var rows = orders .Select( n => new {  Id = n.Id , Customer = n.Customer , ShippingAddress = n.ShippingAddress , OrderDate = n.OrderDate }).ToList();
            var pagelist = new { total = totalCount, rows = rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
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
		   //Detail Models RelatedProperties 
			var orderRepository = _unitOfWork.Repository<Order>();
            ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer");

			var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDetails,Id,Customer,ShippingAddress,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
               _orderService.Insert(order);
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has append a Order record");
                return RedirectToAction("Index");
            }

            DisplayErrorMessage();
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _orderService.Find(id);

		   //Detail Models RelatedProperties 
			var orderRepository = _unitOfWork.Repository<Order>();
            ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer");

			ViewBag.OrderDetails = order.OrderDetails.Select(n => new { Order = n.Order,Product = n.Product,Id = n.Id,ProductId = n.ProductId,Qty = n.Qty,Price = n.Price,Amount = n.Amount,OrderId = n.OrderId });

			var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");

			ViewBag.OrderDetails = order.OrderDetails.Select(n => new { Order = n.Order,Product = n.Product,Id = n.Id,ProductId = n.ProductId,Qty = n.Qty,Price = n.Price,Amount = n.Amount,OrderId = n.OrderId });



            if (order == null)
            {
                return HttpNotFound();
            }
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


        // Get Detail Row By Id For Edit
        // Get : Orders/EditOrderDetail/:id
        [HttpGet]
        public ActionResult EditOrderDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderdetailRepository = _unitOfWork.Repository<OrderDetail>();
            var orderdetail = orderdetailRepository.Find(id);

                        var orderRepository = _unitOfWork.Repository<Order>();             
                        var productRepository = _unitOfWork.Repository<Product>();             
            
            if (orderdetail == null)
            {
                            ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer" );
                            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name" );
                            
                //return HttpNotFound();
                return PartialView("_OrderDetailForm", new OrderDetail());
            }
            else
            {
                            ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer" , orderdetail.OrderId );  
                            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name" , orderdetail.ProductId );  
                             
            }
            return PartialView("_OrderDetailForm", orderdetail);

        }
        
        // Get Create Row By Id For Edit
        // Get : Orders/CreateOrderDetail
        [HttpGet]
        public ActionResult CreateOrderDetail()
        {
                        var orderRepository = _unitOfWork.Repository<Order>();    
              ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer" );
                        var productRepository = _unitOfWork.Repository<Product>();    
              ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name" );
                      return PartialView("_OrderDetailForm");

        }

        // Post Delete Detail Row By Id
        // Get : Orders/DeleteOrderDetailConfirmed/:id
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteOrderDetailConfirmed(int  id)
        {
            var orderdetailRepository = _unitOfWork.Repository<OrderDetail>();
            orderdetailRepository.Delete(id);
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
                //_unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
