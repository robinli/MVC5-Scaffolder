Visual Studio.net 2013 asp.net MVC 5 Scaffolding代码生成向导开源项目
==========================================================================
这个项目经过了大半年的持续更新到目前的阶段基本稳定
所有源代码都是开源的，在github https://github.com/neozhu/MVC5-Scaffolder 共享
整个项目结构，技术框架完全是基于http://genericunitofworkandrepositories.codeplex.com/ 实现。
轻量级的N层架构，Unit Of Work and Repository 设计模式，Entity Framework Code-first的实现方式，这样的技术架构非常简洁和完美。
而我做的就是通过visual studio 2013提供的 Scaffolder代码生成向导的扩展接口上进行自定义开发通过实体类生成这些数据架构所需要源代码，把大量重复的代码利用工具自动生成实现快速开发的同时又有利于规范开发人员的编程习惯。
已经实现的基本功能
1.	单个实体类的增删改查，都是通过easyui datagrid实现
2.	实体类中定义了有外键关键字的字段，会自动生成combox的查询和编辑操作控件
3.	实体类中定义了一对多，主从表结构的，系统自动主从表同时编辑操作查询的页面和功能
4.	编辑功能，会根据字段类型，验证规则生成不同的编辑模式，比如日期类型用datebox，数字类型就用numberbox,必填的验证可以实现
5.	查询功能，会根据字段类型的不同生成不同的控件方便操作，datebox，combox
6.	Excel导入功能，目前只能实现简单表导入的配置


感谢你的支持

 

 

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

![alt tag](http://images2015.cnblogs.com/blog/5997/201604/5997-20160412105628129-1137774382.png)

 

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
![alt tag](http://images2015.cnblogs.com/blog/5997/201604/5997-20160412105628129-1137774382.png)
基本生成样式就是这样

Index首页有分页和查询功能

![alt tag](http://images2015.cnblogs.com/blog/5997/201604/5997-20160412105628504-475147985.png)

修改 可以删除

![alt tag](http://images2015.cnblogs.com/blog/5997/201604/5997-20160412105628926-265270641.png)

 

目前只是一个雏形，还有很多功能需要完善，如果你有兴趣可以一起参与帮忙。
