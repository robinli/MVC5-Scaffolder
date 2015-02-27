

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
                order  = order .Where(n => n.Customer.Contains(searchString));
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
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDetails,Id,Customer,ShippingAddress,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.ObjectState = ObjectState.Added;
                foreach (var detail in order.OrderDetails)
                {
                    detail.ObjectState = ObjectState.Added;
                    if (detail.Product != null)
                        detail.Product.ObjectState = ObjectState.Detached;
                }
               _orderService.InsertOrUpdateGraph(order);
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has append a Order record");
                //return RedirectToAction("Index");
                return Json("{Status:Success}", JsonRequestBehavior.AllowGet);
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
            Order order = _orderService.Query().Include(n => n.OrderDetails.Select(p => p.Product)).Select().Where(n => n.Id == id).First();
            //Order order = _orderService.Find(id);

		   //Detail Models RelatedProperties 
			//var orderRepository = _unitOfWork.Repository<Order>();
            //ViewBag.OrderId = new SelectList(orderRepository.Queryable(), "Id", "Customer");

			ViewBag.OrderDetails = order.OrderDetails.Select(n => new {ProductName=n.Product.Name, Id = n.Id,ProductId = n.ProductId,Qty = n.Qty,Price = n.Price,Amount = n.Amount,OrderId = n.OrderId });

			var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");

			//ViewBag.OrderDetails = order.OrderDetails.Select(n => new { Order = n.Order,Product = n.Product,Id = n.Id,ProductId = n.ProductId,Qty = n.Qty,Price = n.Price,Amount = n.Amount,OrderId = n.OrderId });



            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetails,Id,Customer,ShippingAddress,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.ObjectState = ObjectState.Modified;
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.Id == 0)
                    {
                        detail.OrderId =order.Id;
                        detail.ObjectState = ObjectState.Added;
                    }
                    else
                    {
                        detail.ObjectState = ObjectState.Modified;
                    }
                    if (detail.Product != null)
                    {
                        //detail.Product = null;
                        detail.Product.ObjectState = ObjectState.Detached;
                    }
                }
				//_orderService.Update(order);
                _orderService.InsertOrUpdateGraph(order);
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has update a Order record");
                //return RedirectToAction("Index");
                return Json("{Status:Success}", JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult EditOrderDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderdetailRepository = _unitOfWork.Repository<OrderDetail>();
            var orderdetail=orderdetailRepository.Find(id);
            var productRepository = _unitOfWork.Repository<Product>();
            
            if (orderdetail == null)
            {
                ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");
                //return HttpNotFound();
                return PartialView("_OrderDetailForm", new OrderDetail());
            }
            else
            {
                ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name", orderdetail.ProductId);
            }
            return PartialView("_OrderDetailForm", orderdetail);

        }
        [HttpGet]
        public ActionResult CreateOrderDetail()
        {
            var productRepository = _unitOfWork.Repository<Product>();
            ViewBag.ProductId = new SelectList(productRepository.Queryable(), "Id", "Name");
            return PartialView("_OrderDetailForm");

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
