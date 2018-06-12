
 
                    
      
     
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using WebApp.Models;
 


namespace WebApp.Repositories
{
   public class DataTableImportMappingQuery:QueryObject<DataTableImportMapping>
    {
        public DataTableImportMappingQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.EntitySetName.Contains(search) || x.FieldName.Contains(search) || x.TypeName.Contains(search) || x.SourceFieldName.Contains(search) || x.RegularExpression.Contains(search) );
            return this;
        }

		public DataTableImportMappingQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.EntitySetName.Contains(search) || x.FieldName.Contains(search) || x.TypeName.Contains(search) || x.SourceFieldName.Contains(search) || x.RegularExpression.Contains(search) );
            return this;
        }

		public DataTableImportMappingQuery Withfilter(IEnumerable<filterRule> filters)
        {
           if (filters != null)
           {
               foreach (var rule in filters)
               {
                  
					
				    						if (rule.field == "Id")
						{
							int val = Convert.ToInt32(rule.value);
							And(x => x.Id == val);
						}
				   
					 				
											if (rule.field == "EntitySetName")
						{
							And(x => x.EntitySetName.Contains(rule.value));
						}
				    
				    
					 				
											if (rule.field == "FieldName")
						{
							And(x => x.FieldName.Contains(rule.value));
						}
				    
				    
					 				
											if (rule.field == "TypeName")
						{
							And(x => x.TypeName.Contains(rule.value));
						}
				    
				    
					 				
											if (rule.field == "SourceFieldName")
						{
							And(x => x.SourceFieldName.Contains(rule.value));
						}
				    
				    
					 				
					
				    
					 				
											if (rule.field == "RegularExpression")
						{
							And(x => x.RegularExpression.Contains(rule.value));
						}

                                            if (rule.field == "IsEnabled")
                                            {
                                                
                                                var boolval=Convert.ToBoolean(rule.value);
                                                And(x => x.IsEnabled == boolval);
                                            }
				    
					 									
                   
               }
           }
            return this;
        }
    }
}



