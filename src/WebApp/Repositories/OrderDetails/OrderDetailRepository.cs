// <copyright file="OrderDetailRepository.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>9/29/2018 11:07:42 AM </date>
// <summary>
//  Repository封装了对业务实体数据的查询和存储逻辑(CRUD数据操作)
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
using WebApp.Models;

namespace WebApp.Repositories
{
  public static class OrderDetailRepository  
    {
 
                 public static IEnumerable<OrderDetail> GetByProductId(this IRepositoryAsync<OrderDetail> repository, int productid)
         {
             var query= repository
                .Queryable()
                .Where(x => x.ProductId==productid);
             return query;

         }
             
                 public static IEnumerable<OrderDetail> GetByOrderId(this IRepositoryAsync<OrderDetail> repository, int orderid)
         {
             var query= repository
                .Queryable()
                .Where(x => x.OrderId==orderid);
             return query;

         }
             
        
         
	}
}



