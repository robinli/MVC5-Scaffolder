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
                    detail.Product.ObjectState = ObjectState.Unchanged;
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

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetails,Id,Customer,ShippingAddress,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.ObjectState = ObjectState.Modified;
                //_orderService.Update(order);
               
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
