             
           
 

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
    public class RoleMenuService : Service<RoleMenu>, IRoleMenuService
    {

        private readonly IRepositoryAsync<RoleMenu> _repository;
        private readonly IRepositoryAsync<MenuItem> _menurepository;
        public RoleMenuService(IRepositoryAsync<RoleMenu> repository, IRepositoryAsync<MenuItem> menurepository)
            : base(repository)
        {
            _repository = repository;
            _menurepository = menurepository;
        }

        public IEnumerable<RoleMenu> GetByMenuId(int menuid)
        {
            return _repository.GetByMenuId(menuid);
        }




        public IEnumerable<RoleMenu> GetByRoleName(string roleName)
        {
            return _repository.Queryable().Where(x => x.RoleName == roleName);
        }


        public void UpdateMenus(RoleMenusView[] list)
        {
            var rolename = list.First().RoleName;
            var menuid = list.First().MenuId;
            var mymenus = GetByRoleName(rolename);
            foreach (var item in mymenus)
            {
                Delete(item);
            }

            if (menuid > 0)
            {
                foreach (var item in list)
                {
                    RoleMenu rm = new RoleMenu();
                    rm.MenuId = item.MenuId;
                    rm.RoleName = item.RoleName;
                    Insert(rm);
                }
            }




        }

        private void FindParentMenus(List<MenuItem> list, MenuItem item)
        {
            if (item.ParentId != null && item.ParentId > 0)
            {
                var pitem = _menurepository.Find(item.ParentId);
                if (!list.Where(x => x.Id == pitem.Id).Any())
                {
                    list.Add(pitem);
                }
                if (pitem.ParentId != null && pitem.ParentId > 0)
                {
                    FindParentMenus(list, pitem);
                }
            }
        }

        public IEnumerable<MenuItem> RenderMenus(string[] roleNames)
        {
            var allmenus = _menurepository.Queryable().ToList();
            var mymenus = _repository.Queryable().Where(x => roleNames.Contains(x.RoleName));
            var menulist = new List<MenuItem>();
            foreach (var item in allmenus)
            {
                var myitem = mymenus.Where(x => x.MenuId == item.Id).Any();
                if (myitem)
                {

                    if (!menulist.Where(x => x.Id == item.Id).Any())
                    {
                        menulist.Add(item);
                    }
                    if (item.ParentId != null && item.ParentId > 0)
                    {
                        FindParentMenus(menulist, item);
                    }
                }
            }

            return menulist;

        }
    }
}



