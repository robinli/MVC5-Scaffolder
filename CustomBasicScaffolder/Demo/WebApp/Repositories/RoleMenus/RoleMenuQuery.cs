
 
                    
      
     
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
   public class RoleMenuQuery:QueryObject<RoleMenu>
    {
        public RoleMenuQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.RoleName.Contains(search) );
            return this;
        }

		public RoleMenuQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.RoleName.Contains(search) );
            return this;
        }
    }
}



