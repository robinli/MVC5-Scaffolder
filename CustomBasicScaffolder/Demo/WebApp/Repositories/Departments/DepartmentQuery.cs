
                    
      
     
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using WebApp.Models;
using WebApp.Extensions;

namespace WebApp.Repositories
{
   public class DepartmentQuery:QueryObject<Department>
    {
        public DepartmentQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Name.Contains(search) || x.Manager.Contains(search) );
            return this;
        }

		public DepartmentQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Name.Contains(search) || x.Manager.Contains(search) );
            return this;
        }

		public DepartmentQuery Withfilter(IEnumerable<filterRule> filters)
        {
           if (filters != null)
           {
               foreach (var rule in filters)
               {
                  
					
				    
					 				
					
				    						if (rule.field == "Id")
						{
							And(x => x.Id == Convert.ToInt32(rule.value));
						}
				   
					 				
											if (rule.field == "Name")
						{
							And(x => x.Name.Contains(rule.value));
						}
				    
				    
					 				
											if (rule.field == "Manager")
						{
							And(x => x.Manager.Contains(rule.value));
						}
				    
				    
					 									
                   
               }
           }
            return this;
        }
    }
}



