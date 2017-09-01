                    
      
     
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.SqlServer;
using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using System.Web.WebPages;
using WebApp.Models;
using WebApp.Extensions;

namespace WebApp.Repositories
{
   public class DepartmentQuery:QueryObject<Department>
    {
        public DepartmentQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Manager.Contains(search) || x.CompanyId.ToString().Contains(search) );
            return this;
        }

		public DepartmentQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Manager.Contains(search) || x.CompanyId.ToString().Contains(search) );
            return this;
        }

		public DepartmentQuery Withfilter(IEnumerable<filterRule> filters)
        {
           if (filters != null)
           {
               foreach (var rule in filters)
               {
                  
					
				    						if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Id == val);
						}
				    
					
					
				    				
											if (rule.field == "Name"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Name.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Manager"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Manager.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    						if (rule.field == "CompanyId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.CompanyId == val);
						}
				    
					
					
				    									
                   
               }
           }
            return this;
        }
    }
}



