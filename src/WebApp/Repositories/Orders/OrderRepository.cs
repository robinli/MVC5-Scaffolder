/// <summary>
/// File: OrderRepository.cs
/// Purpose: The repository and unit of work patterns are intended
/// to create an abstraction layer between the data access layer and
/// the business logic layer of an application.
/// Date: 2018/11/13 14:24:27
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 neo.zhu and Contributors
/// License: GNU General Public License v3.See license.txt
/// </summary>
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using WebApp.Models;

namespace WebApp.Repositories
{
  public static class OrderRepository  
    {
 
        
                public static IEnumerable<OrderDetail>   GetOrderDetailsByOrderId (this IRepositoryAsync<Order> repository,int orderid)
        {
			var orderdetailRepository = repository.GetRepository<OrderDetail>(); 
            return orderdetailRepository.Queryable().Include(x => x.Order).Include(x => x.Product).Where(n => n.OrderId == orderid);
        }
         
	}
}



