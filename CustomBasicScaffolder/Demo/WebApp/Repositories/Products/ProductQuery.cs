
                    
      
     
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
   public class ProductQuery:QueryObject<Product>
    {
        public ProductQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Unit.Contains(search) || x.UnitPrice.ToString().Contains(search) || x.StockQty.ToString().Contains(search) || x.ConfirmDateTime.ToString().Contains(search) || x.CategoryId.ToString().Contains(search) );
            return this;
        }

       public ProductQuery Withfilter(IEnumerable<filterRule> filters){
           if (filters != null)
           {
               foreach (var rule in filters)
               {
                   //query = query.Where("@0.Contains(@1)", rule.field, rule.value);
                   if (rule.field == "Name")
                   {
                       And(x => x.Name.Contains(rule.value));
                   }
                   if (rule.field == "Unit")
                   {
                       And(x => x.Unit.Contains(rule.value));
                   }
                   if (rule.field == "Id")
                   {
                       And(x => x.Id == Convert.ToInt32(rule.value));
                   }
                   if (rule.field == "StockQty")
                   {
                       And(x => x.StockQty == Convert.ToInt32(rule.value));
                   }
                   if (rule.field == "")
                   {
                       And(x => x.ConfirmDateTime.Value.Date == Convert.ToDateTime(rule.value).Date);
                   }
                   
               }
           }

           return this;
       }
    }
}



