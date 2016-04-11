
                    
      
    
 
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



