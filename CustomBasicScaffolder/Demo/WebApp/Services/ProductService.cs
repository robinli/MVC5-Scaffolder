
             
           

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
    public class ProductService : Service< Product >, IProductService
    {

        private readonly IRepositoryAsync<Product> _repository;
        public  ProductService(IRepositoryAsync< Product> repository)
            : base(repository)
        {
            _repository=repository;
        }
        
                 public  IEnumerable<Product> GetByCategoryId(int  categoryid)
         {
            return _repository.GetByCategoryId(categoryid);
         }
                   
        
    }
}



