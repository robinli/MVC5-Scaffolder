                    
      
     
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
   public class NotificationQuery:QueryObject<Notification>
    {
        public NotificationQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Title.Contains(search) || x.Content.Contains(search) || x.Link.Contains(search) || x.Read.ToString().Contains(search) || x.From.Contains(search) || x.To.Contains(search) || x.Created.ToString().Contains(search) || x.Creator.Contains(search) );
            return this;
        }

		public NotificationQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Title.Contains(search) || x.Content.Contains(search) || x.Link.Contains(search) || x.Read.ToString().Contains(search) || x.From.Contains(search) || x.To.Contains(search) || x.Created.ToString().Contains(search) || x.Creator.Contains(search) );
            return this;
        }

		public NotificationQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
					
					
				    				
											if (rule.field == "Title"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Title.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Content"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Content.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Link"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Link.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    						if (rule.field == "Read" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Read == val);
						}
				    
					
					
				    				
											if (rule.field == "From"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.From.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "To"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.To.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
					
											if (rule.field == "Created" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDateTime())
						{	
							var date = Convert.ToDateTime(rule.value) ;
							And(x => SqlFunctions.DateDiff("d", date, x.Created)>=0);
						}
				   
				    				
											if (rule.field == "Creator"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Creator.Contains(rule.value));
						}
				    
				    
					
					
				    									
                   
               }
           }
            return this;
        }
    }
}



