/// <summary>
/// File: OrdersController.cs
/// Purpose:
/// Date: 2018/12/20 15:46:42
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: RegisterType UnityConfig.cs
///    container.RegisterType<IRepositoryAsync<Order>, Repository<Order>>();
///    container.RegisterType<IOrderService, OrderService>();
///
/// Copyright (c) 2012-2018 neo.zhu and Contributors
/// License: GNU General Public License v3.See license.txt
/// </summary>
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using TrackableEntities;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;
using Z.EntityFramework.Plus;
namespace WebApp.Controllers
{
  [Authorize]
  public class OrdersController : Controller
  {
    private readonly IOrderService orderService;
    private readonly IUnitOfWorkAsync unitOfWork;
    public OrdersController(IOrderService orderService, IUnitOfWorkAsync unitOfWork)
    {
      this.orderService = orderService;
      this.unitOfWork = unitOfWork;
    }
    //GET: Orders/Index
    //[OutputCache(Duration = 360, VaryByParam = "none")]
    public ActionResult Index() => this.View();

    //Get :Orders/GetData
    //For Index View datagrid datasource url
    [HttpGet]
    public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.orderService
                                 .Query(new OrderQuery().Withfilter(filters))
                                 .OrderBy(n => n.OrderBy(sort, order))
                                 .SelectPageAsync(page, rows, out var totalCount) )
                                 .Select(n => new
                                 {

                                   OrderDetails = n.OrderDetails,
                                   Id = n.Id,
                                   OrderNo = n.OrderNo,
                                   Customer = n.Customer,
                                   ShippingAddress = n.ShippingAddress,
                                   Remark = n.Remark,
                                   OrderDate = n.OrderDate.ToString("yyyy/MM/dd HH:mm:ss")
                                 }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return this.Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveDataAsync(OrderChangeViewModel orders)
    {
      if (orders == null)
      {
        throw new ArgumentNullException(nameof(orders));
      }
      if (this.ModelState.IsValid)
      {
        if (orders.updated != null)
        {
          foreach (var item in orders.updated)
          {
            this.orderService.Update(item);
          }
        }
        if (orders.deleted != null)
        {
          foreach (var item in orders.deleted)
          {
            this.orderService.Delete(item);
          }
        }
        if (orders.inserted != null)
        {
          foreach (var item in orders.inserted)
          {
            this.orderService.Insert(item);
          }
        }
        try
        {
          var result = await this.unitOfWork.SaveChangesAsync();
          return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
      }

    }
    //[OutputCache(Duration = 360, VaryByParam = "none")]
    public async Task<JsonResult> GetOrdersAsync(string q = "")
    {
      var orderRepository = this.unitOfWork.RepositoryAsync<Order>();
      var rows = await orderRepository
                      .Queryable()
                      .Where(n => n.OrderNo.Contains(q))
                      .OrderBy(n => n.OrderNo)
                      .Select(n => new { Id = n.Id, OrderNo = n.OrderNo })
                      .ToListAsync();

      return this.Json(rows, JsonRequestBehavior.AllowGet);
    }
    //[OutputCache(Duration = 360, VaryByParam = "none")]
    public async Task<JsonResult> GetProductsAsync(string q = "")
    {
      var productRepository = this.unitOfWork.RepositoryAsync<Product>();
      var rows = await productRepository
                      .Queryable()
                      .Where(n => n.Name.Contains(q))
                      .OrderBy(n => n.Name)
                      .Select(n => new { Id = n.Id, Name = n.Name })
                      .ToListAsync();

      return this.Json(rows, JsonRequestBehavior.AllowGet);
    }
    //GET: Orders/Details/:id
    public ActionResult Details(int id)
    {

      var order = this.orderService.Find(id);
      if (order == null)
      {
        return this.HttpNotFound();
      }
      return this.View(order);
    }
    //GET: Orders/GetItemAsync/:id
    [HttpGet]
    public async Task<JsonResult> GetItemAsync(int id)
    {
      var order = await this.orderService.FindAsync(id);
      return this.Json(order, JsonRequestBehavior.AllowGet);
    }
    //GET: Orders/Create
    public ActionResult Create()
    {
      var order = new Order();
      //set default value
      return this.View(order);
    }
    //POST: Orders/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateAsync(Order order)
    {
      if (order == null)
      {
        throw new ArgumentNullException(nameof(order));
      }
      if (this.ModelState.IsValid)
      {
        order.TrackingState = TrackingState.Added;
        foreach (var item in order.OrderDetails)
        {
          item.OrderId = order.Id;
          item.TrackingState = TrackingState.Added;
        }
        this.orderService.ApplyChanges(order);
        try
        {
          var result = await this.unitOfWork.SaveChangesAsync();
          return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
        //DisplaySuccessMessage("Has update a order record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(order);
    }

    //新增对象初始化
    [HttpGet]
    public JsonResult PopupCreate()
    {
      var order = new Order();
      return this.Json(order, JsonRequestBehavior.AllowGet);
    }

    //GET: Orders/PopupEdit/:id
    //[OutputCache(Duration = 360, VaryByParam = "id")]
    [HttpGet]
    public async Task<JsonResult> PopupEditAsync(int id)
    {

      var order = await this.orderService.FindAsync(id);
      return this.Json(order, JsonRequestBehavior.AllowGet);
    }

    //GET: Orders/Edit/:id
    public ActionResult Edit(int id)
    {
      var order = this.orderService.Find(id);
      if (order == null)
      {
        return this.HttpNotFound();
      }
      return this.View(order);
    }
    //POST: Orders/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditAsync(Order order)
    {
      if (order == null)
      {
        throw new ArgumentNullException(nameof(order));
      }
      if (this.ModelState.IsValid)
      {
        order.TrackingState = TrackingState.Modified;
        foreach (var item in order.OrderDetails)
        {
          item.OrderId = order.Id;
          //set ObjectState with conditions
          if (item.Id <= 0)
          {
            item.TrackingState = TrackingState.Added;
          }
          else
          {
            item.TrackingState = TrackingState.Modified;
            
          }
        }

        this.orderService.ApplyChanges(order);
        try
        {
          var result = await this.unitOfWork.SaveChangesAsync();
          return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }

        //DisplaySuccessMessage("Has update a Order record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(order);
    }
    //GET: Orders/Delete/:id
    public async Task<ActionResult> DeleteAsync(int id)
    {
      var order = await this.orderService.FindAsync(id);
      if (order == null)
      {
        return this.HttpNotFound();
      }
      return this.View(order);
    }
    //POST: Orders/Delete/:id
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
      var order = await this.orderService.FindAsync(id);
      this.orderService.Delete(order);
      var result = await this.unitOfWork.SaveChangesAsync();
      if (this.Request.IsAjaxRequest())
      {
        return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
      }
      this.DisplaySuccessMessage("Has delete a Order record");
      return this.RedirectToAction("Index");
    }
    //Get Detail Row By Id For Edit
    //Get : Orders/EditOrderDetail/:id
    [HttpGet]
    public async Task<ActionResult> EditOrderDetail(int id)
    {
      var orderdetailRepository = this.unitOfWork.RepositoryAsync<OrderDetail>();
      var orderdetail = await orderdetailRepository.FindAsync(id);
      var orderRepository = this.unitOfWork.RepositoryAsync<Order>();
      var productRepository = this.unitOfWork.RepositoryAsync<Product>();
      if (orderdetail == null)
      {
        this.ViewBag.OrderId = new SelectList(await orderRepository.Queryable().OrderBy(n => n.OrderNo).ToListAsync(), "Id", "OrderNo");
        this.ViewBag.ProductId = new SelectList(await productRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
        //return HttpNotFound();
        return this.PartialView("_OrderDetailEditForm", new OrderDetail());
      }
      else
      {
        this.ViewBag.OrderId = new SelectList(await orderRepository.Queryable().ToListAsync(), "Id", "OrderNo", orderdetail.OrderId);
        this.ViewBag.ProductId = new SelectList(await productRepository.Queryable().ToListAsync(), "Id", "Name", orderdetail.ProductId);
      }
      return this.PartialView("_OrderDetailEditForm", orderdetail);
    }
    //Get Create Row By Id For Edit
    //Get : Orders/CreateOrderDetail
    [HttpGet]
    public async Task<ActionResult> CreateOrderDetailAsync(int orderid)
    {
      var orderRepository = this.unitOfWork.RepositoryAsync<Order>();
      this.ViewBag.OrderId = new SelectList(await orderRepository.Queryable().OrderBy(n => n.OrderNo).ToListAsync(), "Id", "OrderNo");
      var productRepository = this.unitOfWork.RepositoryAsync<Product>();
      this.ViewBag.ProductId = new SelectList(await productRepository.Queryable().OrderBy(n => n.Name).ToListAsync(), "Id", "Name");
      return this.PartialView("_OrderDetailEditForm");
    }
    //Post Delete Detail Row By Id
    //Get : Orders/DeleteOrderDetail/:id
    [HttpGet]
    public async Task<ActionResult> DeleteOrderDetailAsync(int id)
    {
      try
      {
        var orderdetailRepository = this.unitOfWork.RepositoryAsync<OrderDetail>();
        orderdetailRepository.Delete(id);
        var result = await this.unitOfWork.SaveChangesAsync();
        return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
        return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }

    //Get : Orders/GetOrderDetailsByOrderId/:id
    [HttpGet]
    public async Task<JsonResult> GetOrderDetailsByOrderIdAsync(int id)
    {
      var orderdetails = this.orderService.GetOrderDetailsByOrderId(id);
      var data = await orderdetails.AsQueryable().ToListAsync();
      var rows = data.Select(n => new
      {

        OrderOrderNo = n.Order?.OrderNo,
        ProductName = n.Product?.Name,
        Id = n.Id,
        ProductId = n.ProductId,
        Qty = n.Qty,
        Price = n.Price,
        Amount = n.Amount,
        Remark = n.Remark,
        OrderId = n.OrderId
      });
      return this.Json(rows, JsonRequestBehavior.AllowGet);

    }


    //删除选中的记录
    [HttpPost]
    public async Task<JsonResult> DeleteCheckedAsync(int[] id)
    {
      if (id == null)
      {
        throw new ArgumentNullException(nameof(id));
      }
      try
      {
        await this.orderService.Queryable().Where(x => id.Contains(x.Id)).DeleteAsync();
        return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
        return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //导出Excel
    [HttpPost]
    public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var fileName = "orders_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = this.orderService.ExportExcel(filterRules, sort, order);
      return this.File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => this.TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => this.TempData["ErrorMessage"] = msgText;

  }
}
