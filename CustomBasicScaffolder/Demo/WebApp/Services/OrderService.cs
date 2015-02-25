
             
           

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
    public class OrderService : Service< Order >, IOrderService
    {

        private readonly IRepositoryAsync<Order> _repository;
        public  OrderService(IRepositoryAsync< Order> repository)
            : base(repository)
        {
            _repository=repository;
        }
        
                         public IEnumerable<OrderDetail>   GetOrderDetails (int id)
        {
            return _repository.GetOrderDetails(id);
        }
         
        
    }
}



