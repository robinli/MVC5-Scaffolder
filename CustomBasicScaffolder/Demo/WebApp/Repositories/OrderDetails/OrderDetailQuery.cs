
                    
      
     
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
   public class OrderDetailQuery:QueryObject<OrderDetail>
    {
        public OrderDetailQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.ProductId.ToString().Contains(search) || x.Qty.ToString().Contains(search) || x.Price.ToString().Contains(search) || x.Amount.ToString().Contains(search) || x.OrderId.ToString().Contains(search) );
            return this;
        }

		public OrderDetailQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.ProductId.ToString().Contains(search) || x.Qty.ToString().Contains(search) || x.Price.ToString().Contains(search) || x.Amount.ToString().Contains(search) || x.OrderId.ToString().Contains(search) );
            return this;
        }

		public OrderDetailQuery Withfilter(IEnumerable<filterRule> filters)
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




                   if (rule.field == "ProductId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.ProductId == val);
						}




                                            if (rule.field == "Qty" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Qty == val);
						}




                                            if (rule.field == "Price" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							And(x => x.Price == val);
						}
				   
					
				    				
					
				    						if (rule.field == "Amount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Amount == val);
						}
				   
					
				    				
					
				    						if (rule.field == "OrderId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.OrderId == val);
						}
				   
					
				    									
                   
               }
           }
            return this;
        }
    }
}



