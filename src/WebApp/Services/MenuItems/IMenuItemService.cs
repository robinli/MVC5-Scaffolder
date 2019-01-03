// <copyright file="IMenuItemService.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>9/26/2018 4:20:19 PM </date>
// <summary>
//  根据业务需求定义具体的业务逻辑接口
//   
//  
//  
// </summary>

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
using System.Data;
using System.IO;

namespace WebApp.Services
{
    public interface IMenuItemService:IService<MenuItem>
    {

                  IEnumerable<MenuItem> GetByParentId(int  parentid);
        
                 IEnumerable<MenuItem>   GetSubMenusByParentId (int parentid);
         
         
 
		void ImportDataTable(DataTable datatable);
		Stream ExportExcel( string filterRules = "",string sort = "Id", string order = "asc");


        IEnumerable<MenuItem> CreateWithController();
        IEnumerable<MenuItem> ReBuildMenus();
    }
}