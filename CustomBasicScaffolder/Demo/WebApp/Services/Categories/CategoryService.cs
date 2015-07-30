
             
           
 

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Service.Pattern;

using WebApp.Models;
using WebApp.Repositories;
namespace WebApp.Services
{
    public class CategoryService : Service< Category >, ICategoryService
    {

        private readonly IRepositoryAsync<Category> _repository;
        public  CategoryService(IRepositoryAsync< Category> repository)
            : base(repository)
        {
            _repository=repository;
        }
        
                         public IEnumerable<Product>   GetProductsByCategoryId (int categoryid)
        {
            return _repository.GetProductsByCategoryId(categoryid);
        }
         
        
    }
}



