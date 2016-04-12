
                    
      
     
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
   public class ProductQuery:QueryObject<Product>
    {
        public ProductQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Unit.Contains(search) || x.UnitPrice.ToString().Contains(search) || x.StockQty.ToString().Contains(search) || x.ConfirmDateTime.ToString().Contains(search) || x.CategoryId.ToString().Contains(search) );
            return this;
        }

		public ProductQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Unit.Contains(search) || x.UnitPrice.ToString().Contains(search) || x.StockQty.ToString().Contains(search) || x.ConfirmDateTime.ToString().Contains(search) || x.CategoryId.ToString().Contains(search) );
            return this;
        }

		public ProductQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
				    
					
					
				    				
											if (rule.field == "Unit"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Unit.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
											if (rule.field == "UnitPrice" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							And(x => x.UnitPrice == val);
						}
				    
					
				    				
					
				    						if (rule.field == "StockQty" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.StockQty == val);
						}
				    
					
					
				    				
					
				    
					
											if (rule.field == "ConfirmDateTime" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDateTime())
						{	
							var date = Convert.ToDateTime(rule.value) ;
							And(x => SqlFunctions.DateDiff("d", date, x.ConfirmDateTime)>=0);
						}
				   
				    				
					
				    						if (rule.field == "CategoryId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.CategoryId == val);
						}
				    
					
					
				    									
                   
               }
           }
            return this;
        }
    }
}



