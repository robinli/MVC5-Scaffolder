
                    
      
    
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;

using WebApp.Models;

namespace WebApp.Repositories
{
  public static class CategoryRepository  
    {
 
        
                public static IEnumerable<Product>   GetProductsByCategoryId (this IRepositoryAsync<Category> repository,int categoryid)
        {
            return repository.Query(n => n.Id == categoryid).Include(x => x.Products).Select(x => x.Products).FirstOrDefault();
        }
         
	}
}



