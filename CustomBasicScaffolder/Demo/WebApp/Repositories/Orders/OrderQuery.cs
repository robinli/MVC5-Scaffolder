                    
      
     
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
   public class OrderQuery:QueryObject<Order>
    {
        public OrderQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Customer.Contains(search) || x.ShippingAddress.Contains(search) || x.OrderDate.ToString().Contains(search) );
            return this;
        }


		public OrderQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
					
					
				    				
											if (rule.field == "Customer"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Customer.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "ShippingAddress"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.ShippingAddress.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
					
											if (rule.field == "OrderDate" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDateTime())
						{	
							var date = Convert.ToDateTime(rule.value) ;
							And(x => SqlFunctions.DateDiff("d", date, x.OrderDate)>=0);
						}
				   
				    									
                   
               }
           }
            return this;
        }



            }
}



