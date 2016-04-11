
                    
      
    
 
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



