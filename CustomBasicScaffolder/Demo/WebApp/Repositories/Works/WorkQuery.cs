
                    
      
     
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
   public class WorkQuery:QueryObject<Work>
    {
        public WorkQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Status.Contains(search) || x.StartDate.ToString().Contains(search) || x.EndDate.ToString().Contains(search) || x.Hour.ToString().Contains(search) || x.Priority.ToString().Contains(search) || x.Score.ToString().Contains(search) );
            return this;
        }
    }
}



