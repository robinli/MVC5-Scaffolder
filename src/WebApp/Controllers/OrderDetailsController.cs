/// <summary>
/// File: OrderDetailsController.cs
/// Purpose:
/// Date: 2018/12/18 9:07:08
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// TODO: RegisterType UnityConfig.cs
///    container.RegisterType<IRepositoryAsync<OrderDetail>, Repository<OrderDetail>>();
///    container.RegisterType<IOrderDetailService, OrderDetailService>();
///
/// Copyright (c) 2012-2018 neo.zhu and Contributors
/// License: GNU General Public License v3.See license.txt
/// </summary>
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
    [Authorize]
	public class OrderDetailsController : Controller
	{
		private readonly IOrderDetailService  orderDetailService;
		private readonly IUnitOfWorkAsync unitOfWork;
		public OrderDetailsController (IOrderDetailService  orderDetailService, IUnitOfWorkAsync unitOfWork)
		{
			this.orderDetailService  = orderDetailService;
			this.unitOfWork = unitOfWork;
		}
        		//GET: OrderDetails/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public ActionResult Index() => this.View();

		//Get :OrderDetails/GetData
		//For Index View datagrid datasource url
		[HttpGet]
		 public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var pagerows  = (await this.orderDetailService
						               .Query(new OrderDetailQuery().Withfilter(filters)).Include(o => o.Order).Include(o => o.Product)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    OrderOrderNo = n.Order?.OrderNo,
    ProductName = n.Product?.Name,
    Id = n.Id,
    ProductId = n.ProductId,
    Qty = n.Qty,
    Price = n.Price,
    Amount = n.Amount,
    Remark = n.Remark,
    OrderId = n.OrderId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
        [HttpGet]
        public async Task<JsonResult> GetDataByProductIdAsync (int  productid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			    var pagerows = (await this.orderDetailService
						               .Query(new OrderDetailQuery().ByProductIdWithfilter(productid,filters)).Include(o => o.Order).Include(o => o.Product)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    OrderOrderNo = n.Order?.OrderNo,
    ProductName = n.Product?.Name,
    Id = n.Id,
    ProductId = n.ProductId,
    Qty = n.Qty,
    Price = n.Price,
    Amount = n.Amount,
    Remark = n.Remark,
    OrderId = n.OrderId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetDataByOrderIdAsync (int  orderid ,int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {    
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			    var pagerows = (await this.orderDetailService
						               .Query(new OrderDetailQuery().ByOrderIdWithfilter(orderid,filters)).Include(o => o.Order).Include(o => o.Product)
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    OrderOrderNo = n.Order?.OrderNo,
    ProductName = n.Product?.Name,
    Id = n.Id,
    ProductId = n.ProductId,
    Qty = n.Qty,
    Price = n.Price,
    Amount = n.Amount,
    Remark = n.Remark,
    OrderId = n.OrderId
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        //easyui datagrid post acceptChanges 
		[HttpPost]
		public async Task<JsonResult> SaveDataAsync(OrderDetailChangeViewModel orderdetails)
		{
            if (orderdetails == null)
            {
                throw new ArgumentNullException(nameof(orderdetails));
            }
            if (ModelState.IsValid)
            {
			   if (orderdetails.updated != null)
			   {
				foreach (var item in orderdetails.updated)
				{
					this.orderDetailService.Update(item);
				}
			   }
			if (orderdetails.deleted != null)
			{
				foreach (var item in orderdetails.deleted)
				{
					this.orderDetailService.Delete(item);
				}
			}
			if (orderdetails.inserted != null)
			{
				foreach (var item in orderdetails.inserted)
				{
					this.orderDetailService.Insert(item);
				}
			}
            try{
			   var result = await this.unitOfWork.SaveChangesAsync();
			   return Json(new {success=true,result=result}, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                 return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
		    }
            else
            {
                var modelStateErrors = string.Join(",", ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
        
        }
				        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public async Task<JsonResult> GetOrdersAsync(string q="")
		{
			var orderRepository = this.unitOfWork.RepositoryAsync<Order>();
			var rows = await orderRepository
                            .Queryable()
                            .Where(n=>n.OrderNo.Contains(q))
                            .OrderBy(n=>n.OrderNo)
                            .Select(n => new { Id = n.Id, OrderNo = n.OrderNo })
                            .ToListAsync();
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
						        //[OutputCache(Duration = 360, VaryByParam = "none")]
		public async Task<JsonResult> GetProductsAsync(string q="")
		{
			var productRepository = this.unitOfWork.RepositoryAsync<Product>();
			var rows = await productRepository
                            .Queryable()
                            .Where(n=>n.Name.Contains(q))
                            .OrderBy(n=>n.Name)
                            .Select(n => new { Id = n.Id, Name = n.Name })
                            .ToListAsync();
			return Json(rows, JsonRequestBehavior.AllowGet);
		}
								//GET: OrderDetails/Details/:id
		public ActionResult Details(int id)
		{
			
			var orderDetail = this.orderDetailService.Find(id);
			if (orderDetail == null)
			{
				return HttpNotFound();
			}
			return View(orderDetail);
		}
        //GET: OrderDetails/GetItemAsync/:id
        [HttpGet]
        public async Task<JsonResult> GetItemAsync(int id) {
            var  orderDetail = await this.orderDetailService.FindAsync(id);
            return Json(orderDetail,JsonRequestBehavior.AllowGet);
        }
		//GET: OrderDetails/Create
        		public ActionResult Create()
				{
			var orderDetail = new OrderDetail();
			//set default value
			var orderRepository = this.unitOfWork.RepositoryAsync<Order>();
		   			ViewBag.OrderId = new SelectList(orderRepository.Queryable().OrderBy(n=>n.OrderNo), "Id", "OrderNo");
		   			var productRepository = this.unitOfWork.RepositoryAsync<Product>();
		   			ViewBag.ProductId = new SelectList(productRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name");
		   			return View(orderDetail);
		}
		//POST: OrderDetails/Create
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateAsync(OrderDetail orderDetail)
		{
			if (orderDetail == null)
            {
                throw new ArgumentNullException(nameof(orderDetail));
            } 
            if (ModelState.IsValid)
			{
				orderDetailService.Insert(orderDetail);
                try{ 
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result=result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                   var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                   return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
			    //DisplaySuccessMessage("Has update a orderDetail record");
			}
			else {
			   var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			   //DisplayErrorMessage(modelStateErrors);
			}
			//var orderRepository = this.unitOfWork.RepositoryAsync<Order>();
			//ViewBag.OrderId = new SelectList(await orderRepository.Queryable().OrderBy(n=>n.OrderNo).ToListAsync(), "Id", "OrderNo", orderDetail.OrderId);
			//var productRepository = this.unitOfWork.RepositoryAsync<Product>();
			//ViewBag.ProductId = new SelectList(await productRepository.Queryable().OrderBy(n=>n.Name).ToListAsync(), "Id", "Name", orderDetail.ProductId);
			//return View(orderDetail);
		}

        //新增对象初始化
        [HttpGet]
        public JsonResult PopupCreate() {
            var orderDetail = new OrderDetail();
            return Json(orderDetail, JsonRequestBehavior.AllowGet);
        }

        //GET: OrderDetails/PopupEdit/:id
        //[OutputCache(Duration = 360, VaryByParam = "id")]
        [HttpGet]
		public async Task<JsonResult> PopupEditAsync(int id)
		{
			
			var orderDetail = await this.orderDetailService.FindAsync(id);
			return Json(orderDetail,JsonRequestBehavior.AllowGet);
		}

		//GET: OrderDetails/Edit/:id
		public ActionResult Edit(int id)
		{
			var orderDetail = this.orderDetailService.Find(id);
			if (orderDetail == null)
			{
				return HttpNotFound();
			}
			var orderRepository = this.unitOfWork.RepositoryAsync<Order>();
			ViewBag.OrderId = new SelectList(orderRepository.Queryable().OrderBy(n=>n.OrderNo), "Id", "OrderNo", orderDetail.OrderId);
			var productRepository = this.unitOfWork.RepositoryAsync<Product>();
			ViewBag.ProductId = new SelectList(productRepository.Queryable().OrderBy(n=>n.Name), "Id", "Name", orderDetail.ProductId);
			return View(orderDetail);
		}
		//POST: OrderDetails/Edit/:id
		//To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditAsync(OrderDetail orderDetail)
		{
            if (orderDetail == null)
            {
                throw new ArgumentNullException(nameof(orderDetail));
            }
			if (ModelState.IsValid)
			{
				orderDetail.TrackingState = TrackingState.Modified;
								orderDetailService.Update(orderDetail);
				                try{
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
				
				//DisplaySuccessMessage("Has update a OrderDetail record");
				//return RedirectToAction("Index");
			}
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			//DisplayErrorMessage(modelStateErrors);
			}
						//var orderRepository = this.unitOfWork.RepositoryAsync<Order>();
												//var productRepository = this.unitOfWork.RepositoryAsync<Product>();
												//return View(orderDetail);
		}
		//GET: OrderDetails/Delete/:id
		public async Task<ActionResult> DeleteAsync(int id)
		{
			var orderDetail = await this.orderDetailService.FindAsync(id);
			if (orderDetail == null)
			{
				return HttpNotFound();
			}
			return View(orderDetail);
		}
		//POST: OrderDetails/Delete/:id
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			var orderDetail = await  this.orderDetailService.FindAsync(id);
			 this.orderDetailService.Delete(orderDetail);
			var result = await this.unitOfWork.SaveChangesAsync();
		   if (Request.IsAjaxRequest())
				{
					return Json(new { success = true,result=result }, JsonRequestBehavior.AllowGet);
				}
			DisplaySuccessMessage("Has delete a OrderDetail record");
			return RedirectToAction("Index");
		}
       
 

        //删除选中的记录
        [HttpPost]
        public async Task<JsonResult> DeleteCheckedAsync(int[] id) {
           if (id == null)
           {
                throw new ArgumentNullException(nameof(id));
           }
           try{
               await this.orderDetailService.Queryable().Where(x => id.Contains(x.Id)).DeleteAsync();
               return Json(new { success = true }, JsonRequestBehavior.AllowGet);
           }
           catch (System.Data.Entity.Validation.DbEntityValidationException e)
           {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
           }
           catch (Exception e)
           {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
           }
        }
		//导出Excel
		[HttpPost]
		public ActionResult ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "orderdetails_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream=  this.orderDetailService.ExportExcel(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;
		 
	}
}
