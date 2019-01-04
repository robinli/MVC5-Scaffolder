# ASP.NET MVC 5 SmartCode Scaffolding for Visual Studio.Net
# 介绍

ASP.NET MVC 5 SmartCode Scaffolding是一个ASP.NET MVC Web应用程序代码生成框架集成在Visual Studio.Net开发工具中，使用SmartCode Scaffolding可以快速完成一套View,Controller,Model,Service的标准代码。减少程序员在系统开发过程中编写重复的代码,同时有助于团队成员遵循统一的架构和规范进行开发。减少debug的时间，提高软件项目的开发效率。

SmartCode Scaffolding是自定义扩展Visual Studio.Net ASP.NET Scaffolding并且实现了更多功能。

该项目从2014年一直默默的在做版本更新和持续完善,从最早Visual Sutdio.Net 2013到最新2017。并且完全开源 [GITHUB SmartCode Scaffolding](https://github.com/neozhu/MVC5-Scaffolder)
qq群:942771435
![qq讨论群](https://img2018.cnblogs.com/blog/5997/201901/5997-20190104151928326-179910647.png)
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
  
```
###### OrderController.cs 代码片段

```
  
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
