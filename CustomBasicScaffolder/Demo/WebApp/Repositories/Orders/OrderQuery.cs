
                    
      
     
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using WebApp.Models;

namespace WebApp.Repositories
{
   public class OrderQuery:QueryObject<Order>
    {
        public OrderQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Customer.Contains(search) || x.ShippingAddress.Contains(search) || x.OrderDate.ToString().Contains(search) );
            return this;
        }
    }
}



