# ASP.NET MVC 5 SmartCode Scaffolding
## introduce

ASP.NET MVC 5 SmartCode Scaffolding is an ASP.NET MVC Web application code generation framework integration in Visual Studio.Net development tools, use SmartCode Scaffolding can quickly complete a set of View, Controller and Model, the Service standard code.Reduces programmers from writing repetitive code during system development, and helps team members to follow the same architecture and specifications.Reduce the time of debugging and improve the development efficiency of software projects.。

SmartCode Scaffolding is a custom extension to Visual Studio.net ASP.NET Scaffolding and provides more functionality。

The project has been quietly updating and continuously improving the version since 2014,from Visual Sutdio.Net 2013 to the last 2019 verion .

 [GITHUB SmartCode Scaffolding](https://github.com/neozhu/MVC5-Scaffolder)
 qq:942771435

 QQ：28440117，email:[new163@163.com](mailto:new163@163.com),weichat：neostwitter

#Installation & Run
> You need to generate code with the WebApp project in the Demo because it references a lot of CSS and HTML templates
## Demo Site
![](https://raw.githubusercontent.com/neozhu/smartadmin.core.urf/master/img/login.png)

[Demo](http://139.196.107.159:1060/Identity/Account/Login) \
UserName:**demo** Password:**123456** 
[Demo]（http://106.52.105.140:6200/)

![Animation7.gif](https://upload-images.jianshu.io/upload_images/11347576-34058b57299789f1.gif?imageMogr2/auto-orient/strip)


## code generation
#### Define Entity Class objects and properties
> Refer to the EntityFrameWrok Code-First specification definition. The more standard the definition is, the more information you will have for the subsequent generated Code.
The following code defines an Order, OrderDetail, one-to-many relationship. When creating the Controller of the Order class, the corresponding code in the Controller, View will be generated based on the associated entity. For example, EditView will generate both the operation on the header Order form and the DataGrid operation on the detail table OrderDetail.
The definition of orderDetail refers to Product, many-to-one.The Combox control or DropDownList control and the query method of the Controller layer are generated in the View section。
```
    //Define field description information, field length, and basic validation rules
    public partial class Order:Entity
    {
        public Order() {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        public int Id { get; set; }
        [Required] //提示必填,用红色标注
        [Display(Name = "订单号", Description = "订单号", Order = 1)]
        [MaxLength(12)] //输入长度验证
        [MinLength(12)] //输入长度验证
        [Index(IsUnique =true)]
        public string OrderNo { get; set; }
        [Required] //提示必填
        [Display(Name ="客户名称",Description ="订单所属的客户",Order =1)]
        [MaxLength(30)]
        public string Customer { get; set; }
        [Required]
        [Display(Name = "发货地址", Description = "发货地址", Order = 2)]
        [MaxLength(200)]
        public string ShippingAddress { get; set; }
        [Display(Name = "订单日期", Description = "订单日期默认当天", Order = 3)]
        [DefaultValue("now")] //初始化创建对象时设定默认值
        public DateTime OrderDate { get; set; }
        //关联订单明细 1-*
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
    public partial class OrderDetail:Entity
    {
        [Key]
        public int Id { get; set; }
        ...
    }
```
#### The generated code
+ add controller
![Animation.gif](https://upload-images.jianshu.io/upload_images/11347576-dfa57c1edbebb435.gif?imageMogr2/auto-orient/strip)
+ Generate the following code
```
Controllers\OrdersController.cs  /* MVC controller */
Repositories\Orders\OrderQuery.cs  /* Define business logic related queries such as paging selection, foreign key/primary key queries */
Repositories\Orders\OrderRepository.cs /* Repository Design patterns */
Services\Orders\IOrderService.cs /*  business logic interfaces  */
Services\Orders\OrderService.cs /*  business logic implementation  */
Views\Orders\Index.cshtml /* The order information DataGrid includes functions such as query/add/delete/modify/import/export */
Views\Orders\_PopupDetailFormView.cshtml /* Order information pops up in an edit window  */

```
###### index.html javascript Code snippet
```
  
```
###### OrderController.cs Code snippet

```
  
```
#### Register UnityConfig.cs
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

#### Runing 
![Animation2-1.gif](https://upload-images.jianshu.io/upload_images/11347576-894ea6a7eac3d8dd.gif?imageMogr2/auto-orient/strip)

![Animation3.gif](https://upload-images.jianshu.io/upload_images/11347576-f1c4b88ae8ef1a8a.gif?imageMogr2/auto-orient/strip)
以上功能一键生成，包括必填，长度等输入校验规则

#### System architecture and functionality
component

*  ”Microsoft.AspNet.Mvc” version="5.2.4"
*  “Microsoft.AspNet.Razor“ version="3.2.4"
*  "EasyUI" version="1.4.5"
*  "Hangfire" version="1.6.17"
*  "Unity.Mvc" version="5.0.13"
*  "Z.EntityFramework.Plus.EF6" version="1.7.15"
*  SmartAdmin - Responsive WebApp v1.9.1
* "EntityFramework" version="6.2.0" 支持Oracle,MySql,Sql Server,PostgreSQL,SQLite,Sybase等
![image.png](https://upload-images.jianshu.io/upload_images/11347576-76f41ad3f31a229c.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)
### Practical project
[x-TMS](https://neozhu.github.io/WebSite/x-tms.html)
![Animation4.gif](https://upload-images.jianshu.io/upload_images/11347576-26ebf707db8023fb.gif?imageMogr2/auto-orient/strip)
SCRM
![Animation5.gif](https://upload-images.jianshu.io/upload_images/11347576-a29dbb640c7d9fc6.gif?imageMogr2/auto-orient/strip)
MES 
![Animation6.gif](https://upload-images.jianshu.io/upload_images/11347576-d3cf0b232c66f610.gif?imageMogr2/auto-orient/strip)




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
