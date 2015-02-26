
                    
      

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
 
        
                public static IEnumerable<OrderDetail>   GetOrderDetails (this IRepositoryAsync<Order> repository,int id)
        {
            return repository.Query(n => n.Id == id).Include(x => x.OrderDetails).Select(x => x.OrderDetails).FirstOrDefault();
        }
         
	}
}



