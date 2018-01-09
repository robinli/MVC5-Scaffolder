                    
      
     
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
 

namespace WebApp.Repositories
{
   public class EmployeeQuery:QueryObject<Employee>
    {
        public EmployeeQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Sex.Contains(search) || x.Age.ToString().Contains(search) || x.Brithday.ToString().Contains(search) || x.CompanyId.ToString().Contains(search) );
            return this;
        }

		public EmployeeQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Sex.Contains(search) || x.Age.ToString().Contains(search) || x.Brithday.ToString().Contains(search) || x.CompanyId.ToString().Contains(search) );
            return this;
        }

		public EmployeeQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
				    
					
					
				    				
											if (rule.field == "Sex"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Sex.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    						if (rule.field == "Age" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Age == val);
						}
				    
					
					
				    				
					
				    
					
											if (rule.field == "Brithday" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDateTime())
						{	
							var date = Convert.ToDateTime(rule.value) ;
							And(x => SqlFunctions.DateDiff("d", date, x.Brithday)>=0);
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



