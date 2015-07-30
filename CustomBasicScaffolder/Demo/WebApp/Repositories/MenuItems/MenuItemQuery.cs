
 
                    
      
     
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
   public class MenuItemQuery:QueryObject<MenuItem>
    {
        public MenuItemQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Title.Contains(search) || x.Description.Contains(search) || x.Code.Contains(search) || x.Url.Contains(search) );
            return this;
        }

		public MenuItemQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Title.Contains(search) || x.Description.Contains(search) || x.Code.Contains(search) || x.Url.Contains(search) );
            return this;
        }
    }
}



