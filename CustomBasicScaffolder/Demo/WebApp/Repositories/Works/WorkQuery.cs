
                    
      
     
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
   public class WorkQuery:QueryObject<Work>
    {
        public WorkQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Name.Contains(search) || x.Status.Contains(search) || x.StartDate.ToString().Contains(search) || x.EndDate.ToString().Contains(search) );
            return this;
        }

		public WorkQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Name.Contains(search) || x.Status.Contains(search) || x.StartDate.ToString().Contains(search) || x.EndDate.ToString().Contains(search) );
            return this;
        }

		public WorkQuery Withfilter(IEnumerable<filterRule> filters)
        {
           if (filters != null)
           {
               foreach (var rule in filters)
               {
                  
											if (rule.field == "Name")
						{
							And(x => x.Name.Contains(rule.value));
						}
				    
				    
					 				
											if (rule.field == "Status")
						{
							And(x => x.Status.Contains(rule.value));
						}
				    
				    
					 				
					
				    
					 						if (rule.field == "StartDate")
						{
							And(x => x.StartDate.Date == Convert.ToDateTime(rule.value).Date);
						}
				   				
					
				    
					 						if (rule.field == "EndDate")
						{
							And(x => x.EndDate .Value.Date == Convert.ToDateTime(rule.value).Date);
						}
				   				
					
				    
					 				
					
				    						if (rule.field == "Hour")
						{
							And(x => x.Hour == Convert.ToInt32(rule.value));
						}
				   
					 				
					
				    						if (rule.field == "Priority")
						{
							And(x => x.Priority == Convert.ToInt32(rule.value));
						}
				   
					 				
					
				    						if (rule.field == "Score")
						{
							And(x => x.Score == Convert.ToInt32(rule.value));
						}
				   
					 									
                   
               }
           }
            return this;
        }
    }
}



