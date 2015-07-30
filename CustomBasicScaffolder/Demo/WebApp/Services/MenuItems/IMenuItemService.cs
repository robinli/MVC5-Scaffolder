
     
 
 
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
    public interface IMenuItemService : IService<MenuItem>
    {

        IEnumerable<MenuItem> GetByParentId(int parentid);

        IEnumerable<MenuItem> GetSubMenusByParentId(int parentid);


        IEnumerable<MenuItem> AllMenus();





    }
}