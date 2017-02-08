Visual Studio.net 2013 asp.net MVC 5 Scaffolding代码生成向导开源项目
==========================================================================
提高开发效率，规范代码编写，最好的方式就是使用简单的设计模式（MVC ， Repoistory Pattern）+ 模板生成工具。每个小型的软件公司似乎都有自己的基础开发平台，大部分都是基于数据字典+模板动态生成CRUD的操作页面；一般的项目80%代码都可以通过模板生成但并不意味着可以缩短80%的项目开发时间，毕竟很多的业务操作还是要根据用户的需求去定制开发还是需要不少时间去理解和开发的。但随着项目经验积累和沉淀，可以为以后的项目提供帮助。

现在随着移动互联网，手机APP，IPAD等移动设备的流行，似乎所有的需求都需要移动端的应用，我想这也是为什么最近MVC越来越火的原因。不单单是因为MVC的简洁（相对web Form），还有MVC确实要比Web Form更适合在不同的设备上浏览，也更容易封装和复用（Partial View，LayoutTemplate）。

 

最近抽空开发一个居于MVC的代码生成工具，其实也是在别人基础修改的，如果你也有兴趣可以一起参与完善，github是个好东西就是国内访问速度太慢。

我的项目地址 https://github.com/neozhu/MVC5-Scaffolder

+新增async controller action 方法

项目阶段
=========
+ 目前基本实现了对单个实体的增删改查功能
+ 下一步实现导航菜单动态配置动态创建
+ 页面部分全部是现实Ajax局部刷新
+ 顶部导航栏通知功能
+ 添加登陆注册页面模板
+ 一对多的新增编辑模板

MVC5-Scaffolder开源项目
===========================
这个工具的功能通过模板自动生成EntityFramework + UnitOfWork Repository Framework 项目代码，整体项目架构完全参考《Generic Unit of Work and Repositories (lightweight fluent) Framework with Sample Northwind ASP.NET MVC 5 Application》如下图所示，非常完美的架构。
![alt tag](http://images.cnitblog.com/blog/5997/201502/151429375739735.png)


 

* UI (Presentation) Layer
    - ASP.NET MVC - (Sample app: Northwind.Web)
    - Kendo UI - (Sample app: Northwind.Web)
    - AngularJS - (Sample app: Northwind.Web)
* Service and Data Layer
    - Repository Pattern - Framework (Repository.Pattern, Repository.Pattern.Ef6, Northwind.Repository)
    - Unit of Work Pattern - Framework (Repository.Pattern, Repository.Pattern.EF6, Northwind.Repository)
    - Entity Framework
    - Service Pattern - Framework (Service.Pattern, Northwind.Service)
* Domain Driven Design (*slated for release v4.0.0)
    - Domain Events
    - *more to come
    
+ 运行起来大致的样式如下采用Boostrap sb-admin.css

![alt tag](http://images.cnitblog.com/blog/5997/201502/151429415262061.png)

 

MVC5-Scaffolder项目结构和组成
-------------------------------------
![alt tag](http://images.cnitblog.com/blog/5997/201502/151429421679932.png)

Demo

---UnitOfWorkFramework –从网上下载的源代码（http://genericunitofworkandrepositories.codeplex.com/）

--WebApp -Web项目

MVC5Scaffloding -Vs.net 2013代码生成向导插件项目

---Templates ---所有代码生成的模板包括View，Controller，Repoistory，Service，依赖的外部类

 

MVC5Scaffloding.vsix –安装项目

 

代码模板
--------
![alt tag](http://images.cnitblog.com/blog/5997/201502/151429430897017.png)
MvcControllerWithContext –Controller代码模板

_layout –主页面模板

_SideNavBar –主菜单导航栏

_TopNavBa —主页面顶部导航栏

Sb-admin --css样式网上下载的最简单的样式

MvcView --CRUD模板

Repoistories -生成扩展方法可以理解成数据访问层

Services –生成业务逻辑层代码

 

实体类结构
------------
![alt tag](http://images.cnitblog.com/blog/5997/201502/151429436364918.png)

Metadata原数据类也是通过向导生成必要验证规则

![alt tag](http://images.cnitblog.com/blog/5997/201502/151429454798088.png)


也可以很方便修改

 

 

Repoistories，Services 代码结构
------------------------------------------
![alt tag](http://images.cnitblog.com/blog/5997/201502/151429461043731.png)

![alt tag](http://images.cnitblog.com/blog/5997/201502/151429466367401.png)

![alt tag](http://images.cnitblog.com/blog/5997/201502/151429475267028.png)


模板会生成与该实体相关联的实体方法比如通过外键获取关联的实体对象集合

Service层同样会生成与之相关的所有方法和实体
 
Service层在Repoistory层之上，如果业务逻辑复杂需要多个Repository实现那么一个service中会包含多个Repository

 

Controller代码结构
-------------------------------------
![alt tag](http://images.cnitblog.com/blog/5997/201502/151429486514584.png)
 

除了基本的增删改查，Index方法实现了分页查询，排序还没有实现

IProductService,IUnitOfWorkAsync则是通过Unity依赖注入创建

 

配置Unity注册信息
-------------------------------------
![alt tag](http://images.cnitblog.com/blog/5997/201502/151429501511326.png)

首先项目要通过nuget安装Unity boostrapper for asp.net mvc

![alt tag](http://images.cnitblog.com/blog/5997/201502/151429544177323.png)

把创建的Repoistory,Service类注册进去

 

运行调试
------------------------------
![alt tag](http://images.cnitblog.com/blog/5997/201502/151429551209708.png)
基本生成样式就是这样

Index首页有分页和查询功能

![alt tag](http://images.cnitblog.com/blog/5997/201502/151429556989364.png)

修改 可以删除

![alt tag](http://images.cnitblog.com/blog/5997/201502/151429567459178.png)

 

目前只是一个雏形，还有很多功能需要完善，如果你有兴趣可以一起参与帮忙。
