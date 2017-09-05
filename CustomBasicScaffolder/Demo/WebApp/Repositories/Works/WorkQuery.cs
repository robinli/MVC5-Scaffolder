                    
      
     
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
   public class WorkQuery:QueryObject<Work>
    {
        public WorkQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Name.Contains(search) || x.Status.Contains(search) || x.StartDate.ToString().Contains(search) || x.EndDate.ToString().Contains(search) || x.Hour.ToString().Contains(search) || x.Priority.ToString().Contains(search) || x.Score.ToString().Contains(search) );
            return this;
        }

		public WorkQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Name.Contains(search) || x.Status.Contains(search) || x.StartDate.ToString().Contains(search) || x.EndDate.ToString().Contains(search) || x.Hour.ToString().Contains(search) || x.Priority.ToString().Contains(search) || x.Score.ToString().Contains(search) );
            return this;
        }

		public WorkQuery Withfilter(IEnumerable<filterRule> filters)
        {
           if (filters != null)
           {
               foreach (var rule in filters)
               {
                  
											if (rule.field == "Name"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Name.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Status"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Status.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
					
											if (rule.field == "StartDate" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDateTime())
						{	
							var date = Convert.ToDateTime(rule.value) ;
							And(x => SqlFunctions.DateDiff("d", date, x.StartDate)>=0);
						}
				   
				    				
					
				    
					
											if (rule.field == "EndDate" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDateTime())
						{	
							var date = Convert.ToDateTime(rule.value) ;
							And(x => SqlFunctions.DateDiff("d", date, x.EndDate)>=0);
						}
				   
				    				
					
				    
					
					
				    						if (rule.field == "Enableed" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
						{	
							 var boolval=Convert.ToBoolean(rule.value);
							 And(x => x.Enableed == boolval);
						}
				   				
					
				    						if (rule.field == "Hour" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Hour == val);
						}
				    
					
					
				    				
					
				    						if (rule.field == "Priority" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Priority == val);
						}
				    
					
					
				    				
					
				    
											if (rule.field == "Score" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							And(x => x.Score == val);
						}
				    
					
				    									
                   
               }
           }
            return this;
        }
    }
}



