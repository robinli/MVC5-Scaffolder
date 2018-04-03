// <copyright file="OrdersController.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2017 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>12/7/2017 1:44:35 PM </date>
// <summary>
// Create By Custom MVC5 Scaffolder for Visual Studio
// TODO: RegisterType UnityConfig.cs
// container.RegisterType<IRepositoryAsync<Order>, Repository<Order>>();
// container.RegisterType<IOrderService, OrderService>();
// </summary>
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

namespace WebApp.Controllers
{
    public class OrdersController1 : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly IOrderService _orderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public OrdersController1(IOrderService orderService, IUnitOfWorkAsync unitOfWork)
        {
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }
        // GET: Orders/Index
        public ActionResult Index()
        {
            return View();
        }
        // Get :Orders/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            //int pagenum = offset / limit +1;
            var orders = await _orderService
       .Query(new OrderQuery().Withfilter(filters))
       .OrderBy(n => n.OrderBy(sort, order))
       .SelectPageAsync(page, rows, out totalCount);
            var datarows = orders.Select(n => new { Id = n.Id, Customer = n.Customer, ShippingAddress = n.ShippingAddress, OrderDate = n.OrderDate }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> SaveData(OrderChangeViewModel orders)
        {
            if (orders.updated != null)
            {
                foreach (var item in orders.updated)
                {
                    _orderService.Update(item);
                }
            }
            if (orders.deleted != null)
            {
                foreach (var item in orders.deleted)
                {
                    _orderService.Delete(item);
                }
            }
            if (orders.inserted != null)
            {
                foreach (var item in orders.inserted)
                {
                    _orderService.Insert(item);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetOrders()
        {
            var orderRepository = _unitOfWork.RepositoryAsync<Order>();
            var data = await orderRepository.Queryable().ToListAsync();
            var rows = data.Select(n => new { Id = n.Id, Customer = n.Customer });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetProducts(string q="")
        {
            var productRepository = _unitOfWork.RepositoryAsync<Product>();
            var data = await productRepository.Queryable().Where(n=>n.Name.Contains(q)).ToListAsync();
            var rows = data.Select(n => new { Id = n.Id, Name = n.Name });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = await _orderService.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        // GET: Orders/Create
        public ActionResult Create()
        {
            var order = new Order();
            //set default value
            return View(order);
        }
        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OrderDetails,Id,Customer,ShippingAddress,OrderDate,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.ObjectState = ObjectState.Added;
                foreach (var item in order.OrderDetails)
                {
                    item.OrderId = order.Id;
                    item.ObjectState = ObjectState.Added;
                }
                _orderService.InsertOrUpdateGraph(order);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Order record");
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
            return View(order);
        }
        // GET: Orders/PopupEdit/5
        public async Task<ActionResult> PopupEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = await _orderService.FindAsync(id);
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = await _orderService.FindAsync(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "OrderDetails,Id,Customer,ShippingAddress,OrderDate,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.ObjectState = ObjectState.Modified;
                foreach (var item in order.OrderDetails)
                {
                    item.OrderId = order.Id;
                    //set ObjectState with conditions
                    if (item.Id <= 0)
                        item.ObjectState = ObjectState.Added;
                    else
                        item.ObjectState = ObjectState.Modified;
                }

                _orderService.InsertOrUpdateGraph(order);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Order record");
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
            return View(order);
        }
        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = await _orderService.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderService.FindAsync(id);
            _orderService.Delete(order);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Order record");
            return RedirectToAction("Index");
        }
        // Get Detail Row By Id For Edit
        // Get : Orders/EditOrderDetail/:id
        [HttpGet]
        public async Task<ActionResult> EditOrderDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderdetailRepository = _unitOfWork.RepositoryAsync<OrderDetail>();
            var orderdetail = await orderdetailRepository.FindAsync(id);
            var orderRepository = _unitOfWork.RepositoryAsync<Order>();
            var productRepository = _unitOfWork.RepositoryAsync<Product>();
            if (orderdetail == null)
            {
                ViewBag.OrderId = new SelectList(await orderRepository.Queryable().ToListAsync(), "Id", "Customer");
                ViewBag.ProductId = new SelectList(await productRepository.Queryable().ToListAsync(), "Id", "Name");
                //return HttpNotFound();
                return PartialView("_OrderDetailEditForm", new OrderDetail());
            }
            else
            {
                ViewBag.OrderId = new SelectList(await orderRepository.Queryable().ToListAsync(), "Id", "Customer", orderdetail.OrderId);
                ViewBag.ProductId = new SelectList(await productRepository.Queryable().ToListAsync(), "Id", "Name", orderdetail.ProductId);
            }
            return PartialView("_OrderDetailEditForm", orderdetail);
        }
        // Get Create Row By Id For Edit
        // Get : Orders/CreateOrderDetail
        [HttpGet]
        public async Task<ActionResult> CreateOrderDetail()
        {
            var orderRepository = _unitOfWork.RepositoryAsync<Order>();
            ViewBag.OrderId = new SelectList(await orderRepository.Queryable().ToListAsync(), "Id", "Customer");
            var productRepository = _unitOfWork.RepositoryAsync<Product>();
            ViewBag.ProductId = new SelectList(await productRepository.Queryable().ToListAsync(), "Id", "Name");
            return PartialView("_OrderDetailEditForm");
        }
        // Post Delete Detail Row By Id
        // Get : Orders/DeleteOrderDetail/:id
        [HttpPost, ActionName("DeleteOrderDetail")]
        public async Task<ActionResult> DeleteOrderDetailConfirmed(int id)
        {
            var orderdetailRepository = _unitOfWork.RepositoryAsync<OrderDetail>();
            orderdetailRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Order record");
            return RedirectToAction("Index");
        }

        // Get : Orders/GetOrderDetailsByOrderId/:id
        [HttpGet]
        public async Task<ActionResult> GetOrderDetailsByOrderId(int id)
        {
            var orderdetails = _orderService.GetOrderDetailsByOrderId(id);
            if (Request.IsAjaxRequest())
            {
                var data = await orderdetails.AsQueryable().ToListAsync();
                var rows = data.Select(n => new { OrderCustomer = (n.Order == null ? "" : n.Order.Customer), ProductName = (n.Product == null ? "" : n.Product.Name), Id = n.Id, ProductId = n.ProductId, Qty = n.Qty, Price = n.Price, Amount = n.Amount, OrderId = n.OrderId });
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            return View(orderdetails);
        }

        //导出Excel
        [HttpPost]
        public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "orders_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = _orderService.ExportExcel(filterRules, sort, order);
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
