                    
      
     
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
   public class MessageQuery:QueryObject<Message>
    {
        public MessageQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Group.ToString().Contains(search) || x.ExtensionKey1.Contains(search) || x.Type.ToString().Contains(search) || x.Code.Contains(search) || x.Content.Contains(search) || x.ExtensionKey2.Contains(search) || x.Tags.Contains(search) || x.Method.Contains(search) || x.Created.ToString().Contains(search) || x.IsNew.ToString().Contains(search) || x.IsNotification.ToString().Contains(search) );
            return this;
        }

		public MessageQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Group.ToString().Contains(search) || x.ExtensionKey1.Contains(search) || x.Type.ToString().Contains(search) || x.Code.Contains(search) || x.Content.Contains(search) || x.ExtensionKey2.Contains(search) || x.Tags.Contains(search) || x.Method.Contains(search) || x.Created.ToString().Contains(search) || x.IsNew.ToString().Contains(search) || x.IsNotification.ToString().Contains(search) );
            return this;
        }

		public MessageQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
					
					
				    				
					
				    						if (rule.field == "Group" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Group == val);
						}
				    
					
					
				    				
											if (rule.field == "ExtensionKey1"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.ExtensionKey1.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    						if (rule.field == "Type" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Type == val);
						}
				    
					
					
				    				
											if (rule.field == "Code"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Code.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Content"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Content.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "ExtensionKey2"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.ExtensionKey2.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Tags"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Tags.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Method"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Method.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
					
											if (rule.field == "Created" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDateTime())
						{	
							var date = Convert.ToDateTime(rule.value) ;
							And(x => SqlFunctions.DateDiff("d", date, x.Created)>=0);
						}
				   
				    				
					
				    						if (rule.field == "IsNew" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.IsNew == val);
						}
				    
					
					
				    				
					
				    						if (rule.field == "IsNotification" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.IsNotification == val);
						}
				    
					
					
				    									
                   
               }
           }
            return this;
        }
    }
}



