# ASP.NET MVC 5 SmartCode Scaffolding for Visual Studio.Net
# 介绍

ASP.NET MVC 5 SmartCode Scaffolding是一个ASP.NET MVC Web应用程序代码生成框架集成在Visual Studio.Net开发工具中，使用SmartCode  Scaffolding可以快速添加一整套View,Controller,Model,Service可以运行的交互式代码。减少程序员在系统开发过程中编写重复的代码行数（估计可以减少80%代码Coding）,同时有助于团队成员遵循统一的架构和规范进行开发。减少debug的时间，提高软件项目的开发效率。

SmartCode Scaffolding是自定义扩展Visual Studio.Net ASP.NET Scaffolding并且实现了更多功能和生成更多的标准代码。

该项目从2014年一直默默的在做版本更新和持续完善,从最早Visual Sutdio.Net 2013到最新2017。并且完全开源 [GITHUB SmartCode Scaffolding](https://github.com/neozhu/MVC5-Scaffolder)
我的联系方式 QQ：28440117，email:[new163@163.com](mailto:new163@163.com),微信：neostwitter
我的主页:[https://neozhu.github.io/WebSite/index.html](https://neozhu.github.io/WebSite/index.html)

![image.png](https://upload-images.jianshu.io/upload_images/11347576-9bd09484ed65aa0d.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

#安装&使用
> 需要要配合Demo中WebApp 项目来生成代码，因为其中引用了大量的css和html模板

![Animation7.gif](https://upload-images.jianshu.io/upload_images/11347576-34058b57299789f1.gif?imageMogr2/auto-orient/strip)


# 代码生成的过程
#### 定义实体对象(Entity class)和属性
> 参考EntityFramewrok Code-First规范定义，定义的越规范，信息越多对后面的生成的代码就越完善。
下面代码定义一个Order，OrderDetail,一对多的关系，在创建Order类的Controller时会在controller，View，会根据关联的实体生成相应的代码，比如EditView，会同时生成对表头Order form表单的操作和明细表OrderDetail的datagrid操作。
定义OrderDetail中引用了Product，多对一的关系。会在View部分生成Combox控件或DropdownList的控件和Controller层的查询方法。
```
    //定义字段描述信息，字段长度，基本验证规则
    public partial class Order:Entity
    {
        public Order() {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="客户名称",Description ="订单所属的客户",Order =1)]
        [MaxLength(30)]
        public string Customer { get; set; }
        [Required]
        [Display(Name = "发货地址", Description = "发货地址", Order = 2)]
        [MaxLength(200)]
        public string ShippingAddress { get; set; }
        [Display(Name = "订单日期", Description = "订单日期默认当天", Order = 3)]
        public DateTime OrderDate { get; set; }
        //关联订单明细 1-*
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
    public partial class OrderDetail:Entity
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "必选")]
        [Display(Name ="商品", Description ="商品",Order =2)]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [Display(Name = "商品", Description = "商品", Order = 3)]
        public Product Product { get; set; }
        [Required(ErrorMessage="必填")]
        [Range(1,9999)]
        [Display(Name = "数量", Description = "需求数量", Order = 4)]
        public int Qty { get; set; }
        [Required(ErrorMessage = "必填")]
        [Range(1, 9999)]
        [Display(Name = "单价", Description = "单价", Order = 5)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "必填")]
        [Range(1, 9999)]
        [Display(Name = "金额", Description = "金额(数量x单价)", Order = 6)]
        public decimal Amount { get; set; }
        [Display(Name = "订单号", Description = "订单号", Order = 1)]
        public int OrderId { get; set; }
        //关联订单表头
        [ForeignKey("OrderId")]
        [Display(Name = "订单号", Description = "订单号", Order = 1)]
        public Order Order { get; set; }
    }
```
#### 生成代码
+ 添加controller
![Animation.gif](https://upload-images.jianshu.io/upload_images/11347576-dfa57c1edbebb435.gif?imageMogr2/auto-orient/strip)
+ 生成以下代码
```
Controllers\OrdersController.cs  /* MVC控制类 */
Repositories\Orders\OrderQuery.cs  /* 定义与业务逻辑相关查询比如分页帅选，外键/主键查询 */
Repositories\Orders\OrderRepository.cs /* Repository模式  */
Services\Orders\IOrderService.cs /* 具体的业务逻辑接口  */
Services\Orders\OrderService.cs /* 具体的业务逻辑实现  */
Views\Orders\Index.cshtml /* 订单信息DataGrid包括查询/新增/删除/修改/导入/导出等功能  */
Views\Orders\_PopupDetailFormView.cshtml /* 订单信息弹出编辑框  */
Views\Orders\Create.cshtml /* 订单信息新增操作页面  */
Views\Orders\Edit.cshtml /* 订单信息编辑操作页面 */
Views\Orders\EditForm.cshtml /* 订单信息编辑表单  */
```
###### index.html javascript代码片段
```
 var entityname = "Order";


 //下载Excel导入模板
 function downloadtemplate() {
     //TODO: 修改下载模板的路径
     var url = "/ExcelTemplate/Order.xlsx";
     $.fileDownload(url)
         .fail(function() {
             $.messager.alert("错误", "没有找到模板文件! {" + url + "}");
         });

 }
 //打开Excel上传导入
 function importexcel() {
     $("#importwindow").window("open");
 }
 //执行Excel到处下载
 function exportexcel() {
     var filterRules = JSON.stringify($dg.datagrid("options").filterRules);
     //console.log(filterRules);
     $.messager.progress({
         title: "正在执行导出！"
     });
     var formData = new FormData();
     formData.append("filterRules", filterRules);
     formData.append("sort", "Id");
     formData.append("order", "asc");
     $.postDownload("/Orders/ExportExcel", formData, function(fileName) {
         $.messager.progress("close");
         console.log(fileName);

     })
 }
 //显示帮助信息
 function dohelp() {

 }
 //easyui datagrid 增删改查操作
 var $dg = $("#orders_datagrid").datagrid({
     rownumbers: true,
     checkOnSelect: true,
     selectOnCheck: true,
     idField: 'Id',
     sortName: 'Id',
     sortOrder: 'desc',
     remoteFilter: true,
     singleSelect: true,
     toolbar: '#orders_toolbar',
     url: '/Orders/GetData',
     method: 'get',
     onClickCell: onClickCell,
     pagination: true,
     striped: true,
     columns: [
         [
             /*{ field: 'ck', checkbox: true },*/
             {
                 field: '_operate1',
                 title: '操作',
                 width: 120,
                 sortable: false,
                 resizable: true,
                 formatter: showdetailsformatter
             },
             /*{field:'Id',width:80 ,sortable:true,resizable:true }*/
             {
                 field: 'Customer',
                 title: '@Html.DisplayNameFor(model => model.Customer)',
                 width: 140,
                 editor: {
                     type: 'textbox',
                     options: {
                         prompt: '客户名称',
                         required: true,
                         validType: 'length[0,30]'
                     }
                 },
                 sortable: true,
                 resizable: true
             },
             {
                 field: 'ShippingAddress',
                 title: '@Html.DisplayNameFor(model => model.ShippingAddress)',
                 width: 140,
                 editor: {
                     type: 'textbox',
                     options: {
                         prompt: '发货地址',
                         required: true,
                         validType: 'length[0,200]'
                     }
                 },
                 sortable: true,
                 resizable: true
             },
             {
                 field: 'OrderDate',
                 title: '@Html.DisplayNameFor(model => model.OrderDate)',
                 width: 160,
                 align: 'right',
                 editor: {
                     type: 'datebox',
                     options: {
                         prompt: '订单日期',
                         required: true
                     }
                 },
                 sortable: true,
                 resizable: true,
                 formatter: dateformatter
             },

         ]
     ]

 });
 var editIndex = undefined;

 function reload() {
     if (endEditing()) {
         $dg.datagrid("reload");
     }
 }

 function endEditing() {
     if (editIndex == undefined) {
         return true
     }
     if ($dg.datagrid("validateRow", editIndex)) {

         $dg.datagrid("endEdit", editIndex);
         editIndex = undefined;


         return true;
     } else {
         return false;
     }
 }

 function onClickCell(index, field) {
     var _operates = ["_operate1", "_operate2", "_operate3", "ck"]
     if ($.inArray(field, _operates) >= 0) {
         return;
     }
     if (editIndex != index) {
         if (endEditing()) {
             $dg.datagrid("selectRow", index)
                 .datagrid("beginEdit", index);
             editIndex = index;
             var ed = $dg.datagrid("getEditor", {
                 index: index,
                 field: field
             });
             if (ed) {
                 ($(ed.target).data("textbox") ? $(ed.target).textbox("textbox") : $(ed.target)).focus();
             }

         } else {
             $dg.datagrid("selectRow", editIndex);
         }
     }
 }

 function append() {
     if (endEditing()) {
         //$dg.datagrid("appendRow", { Status: 0 });
         //editIndex = $dg.datagrid("getRows").length - 1;
         $dg.datagrid("insertRow", {
             index: 0,
             row: {}
         });
         editIndex = 0;
         $dg.datagrid("selectRow", editIndex)
             .datagrid("beginEdit", editIndex);
     }
 }

 function removeit() {
     if (editIndex == undefined) {
         return
     }
     $dg.datagrid("cancelEdit", editIndex)
         .datagrid("deleteRow", editIndex);
     editIndex = undefined;
 }

 function accept() {
     if (endEditing()) {
         if ($dg.datagrid("getChanges").length) {
             var inserted = $dg.datagrid("getChanges", "inserted");
             var deleted = $dg.datagrid("getChanges", "deleted");
             var updated = $dg.datagrid("getChanges", "updated");
             var effectRow = new Object();
             if (inserted.length) {
                 effectRow.inserted = inserted;
             }
             if (deleted.length) {
                 effectRow.deleted = deleted;
             }
             if (updated.length) {
                 effectRow.updated = updated;
             }
             //console.log(JSON.stringify(effectRow));
             $.post("/Orders/SaveData", effectRow, function(response) {
                 //console.log(response);
                 if (response.Success) {
                     $.messager.alert("提示", "提交成功！");
                     $dg.datagrid("acceptChanges");
                     $dg.datagrid("reload");
                 }
             }, "json").fail(function(response) {
                 //console.log(response);
                 $.messager.alert("错误", "提交错误了！", "error");
                 //$dg.datagrid("reload");
             });

         }

         //$dg.datagrid("acceptChanges");
     }
 }

 function reject() {
     $dg.datagrid("rejectChanges");
     editIndex = undefined;
 }

 function getChanges() {
     var rows = $dg.datagrid("getChanges");
     alert(rows.length + " rows are changed!");
 }

 //datagrid 开启筛选功能
 $(function() {

     $dg.datagrid("enableFilter", [

         {
             field: "Id",
             type: "numberbox",
             op: ['equal', 'notequal', 'less', 'lessorequal', 'greater', 'greaterorequal']
         },


         {
             field: "OrderDate",
             type: "dateRange",
             options: {
                 onChange: function(value) {
                     $dg.datagrid("addFilterRule", {
                         field: "OrderDate",
                         op: "between",
                         value: value
                     });

                     $dg.datagrid("doFilter");
                 }
             }
         },


     ]);
 })
 //-----------------------------------------------------
 //datagrid onSelect
 //-----------------------------------------------------
 function showdetailsformatter(value, row, index) {

     return '<a onclick="showDetailsWindow(' + row.Id + ')" class="easyui-linkbutton" href="javascript:void(0)">查看明细</a>';

 }
 //弹出明细信息
 function showDetailsWindow(id) {
     //console.log(index, row);
     $.getJSON('/Orders/PopupEdit/' + id, function(data, status, xhr) {
         //console.log(data);
         $('#detailswindow').window('open');
         loadData(id, data);


     });

 }
```
###### OrderController.cs 代码片段

```
 public class OrdersController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly IOrderService _orderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public OrdersController(IOrderService orderService, IUnitOfWorkAsync unitOfWork)
        {
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }
        // GET: Orders/Index
        [OutputCache(Duration = 360, VaryByParam = "none")]
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
        //[OutputCache(Duration = 360, VaryByParam = "none")]
        public async Task<ActionResult> GetOrders(string q = "")
        {
            var orderRepository = _unitOfWork.RepositoryAsync<Order>();
            var data = await orderRepository.Queryable().Where(n => n.Customer.Contains(q)).ToListAsync();
            var rows = data.Select(n => new { Id = n.Id, Customer = n.Customer });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        //[OutputCache(Duration = 360, VaryByParam = "none")]
        public async Task<ActionResult> GetProducts(string q = "")
        {
            var productRepository = _unitOfWork.RepositoryAsync<Product>();
            var data = await productRepository.Queryable().Where(n => n.Name.Contains(q)).ToListAsync();
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
        [ValidateAntiForgeryToken]
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
        [OutputCache(Duration = 360, VaryByParam = "id")]
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
        [ValidateAntiForgeryToken]
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
```
#### 注册UnityConfig.cs
```
        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
              container.RegisterType<IRepositoryAsync<Order>, Repository<Order>>();
              container.RegisterType<IOrderService, OrderService>();

              container.RegisterType<IRepositoryAsync<OrderDetail>, Repository<OrderDetail>>();
              container.RegisterType<IOrderDetailService, OrderDetailService>();
        }
```

#### 运行生成的代码功能
![Animation2-1.gif](https://upload-images.jianshu.io/upload_images/11347576-894ea6a7eac3d8dd.gif?imageMogr2/auto-orient/strip)

![Animation3.gif](https://upload-images.jianshu.io/upload_images/11347576-f1c4b88ae8ef1a8a.gif?imageMogr2/auto-orient/strip)
以上功能一键生成，包括必填，长度等输入校验规则

#### 整个项目的系统架构和功能
主要组件

*  ”Microsoft.AspNet.Mvc” version="5.2.4"
*  “Microsoft.AspNet.Razor“ version="3.2.4"
*  "EasyUI" version="1.4.5"
*  "Hangfire" version="1.6.17"
*  "Unity.Mvc" version="5.0.13"
*  "Z.EntityFramework.Plus.EF6" version="1.7.15"
*  SmartAdmin - Responsive WebApp v1.9.1
* "EntityFramework" version="6.2.0" 支持Oracle,MySql,Sql Server,PostgreSQL,SQLite,Sybase等
![image.png](https://upload-images.jianshu.io/upload_images/11347576-76f41ad3f31a229c.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)
### 实战项目
[x-TMS](https://neozhu.github.io/WebSite/x-tms.html)
![Animation4.gif](https://upload-images.jianshu.io/upload_images/11347576-26ebf707db8023fb.gif?imageMogr2/auto-orient/strip)
供应链协同平台
![Animation5.gif](https://upload-images.jianshu.io/upload_images/11347576-a29dbb640c7d9fc6.gif?imageMogr2/auto-orient/strip)
MES系统
![Animation6.gif](https://upload-images.jianshu.io/upload_images/11347576-d3cf0b232c66f610.gif?imageMogr2/auto-orient/strip)



### 我们还能做
承接企业内部业务系统开发，组建企业私有云，虚拟化集群服务器部署。
承接BizTalk  B2B/EAI/EDI/AS/RosettaNet 开发工作


### 联系方式
![image.png](https://upload-images.jianshu.io/upload_images/11347576-efee6f04cb478991.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)
###

### 捐助
如果这个项目对您有用，我们欢迎各方任何形式的捐助，也包括参与到项目代码更新或意见反馈中来。谢谢！
资金捐助：![image.png](https://upload-images.jianshu.io/upload_images/11347576-d884bcb748f8f6ea.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/600)

### License
Apache License Version 2.0

Copyright 2017 Neo.Zhu  

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the 
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
either express or implied. See the License for the specific language governing permissions 
and limitations under the License.