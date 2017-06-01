
                    
      
     
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
   public class CodeItemQuery:QueryObject<CodeItem>
    {
        public CodeItemQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Code.Contains(search) || x.Text.Contains(search) || x.BaseCodeId.ToString().Contains(search) );
            return this;
        }

		public CodeItemQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Code.Contains(search) || x.Text.Contains(search) || x.BaseCodeId.ToString().Contains(search) );
            return this;
        }

		public CodeItemQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
					
					
				    				
											if (rule.field == "Code"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Code.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Text"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Text.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    						if (rule.field == "BaseCodeId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.BaseCodeId == val);
						}
				    
					
					
				    									
                   
               }
           }
            return this;
        }
    }
}



