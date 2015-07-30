
                    
      

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;

using WebApp.Models;

namespace WebApp.Repositories
{
  public static class ProductRepository  
    {
 
                 public static IEnumerable<Product> GetByCategoryId(this IRepositoryAsync<Product> repository, int categoryid)
         {
             var query= repository
                .Queryable()
                .Where(x => x.CategoryId==categoryid);
             return query;

         }
             
        
         
	}
}



