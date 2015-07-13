             
           
 

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
    public class MenuItemService : Service<MenuItem>, IMenuItemService
    {

        private readonly IRepositoryAsync<MenuItem> _repository;
        public MenuItemService(IRepositoryAsync<MenuItem> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<MenuItem> GetByParentId(int parentid)
        {
            return _repository.GetByParentId(parentid);
        }
        public IEnumerable<MenuItem> GetSubMenusByParentId(int parentid)
        {
            return _repository.GetSubMenusByParentId(parentid);
        }







        public IEnumerable<MenuItem> AllMenus()
        {


            return _repository.Queryable();

        }
    }
}



