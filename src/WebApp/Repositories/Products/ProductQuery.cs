/// <summary>
/// File: ProductQuery.cs
/// Purpose: easyui datagrid filter query 
/// Date: 2018/10/18 16:29:54
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 neo.zhu and Contributors
/// License: GNU General Public License v3.See license.txt
/// </summary>
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
   public class ProductQuery:QueryObject<Product>
   {

		public ProductQuery Withfilter(IEnumerable<filterRule> filters)
        {
           if (filters != null)
           {
               foreach (var rule in filters)
               {
                  
					
				    						if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Id == val);
                                break;
                            case "notequal":
                                And(x => x.Id != val);
                                break;
                            case "less":
                                And(x => x.Id < val);
                                break;
                            case "lessorequal":
                                And(x => x.Id <= val);
                                break;
                            case "greater":
                                And(x => x.Id > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Id >= val);
                                break;
                            default:
                                And(x => x.Id == val);
                                break;
                        }
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
							switch (rule.op) {
                            case "equal":
                                And(x => x.UnitPrice == val);
                                break;
                            case "notequal":
                                And(x => x.UnitPrice != val);
                                break;
                            case "less":
                                And(x => x.UnitPrice < val);
                                break;
                            case "lessorequal":
                                And(x => x.UnitPrice <= val);
                                break;
                            case "greater":
                                And(x => x.UnitPrice > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.UnitPrice >= val);
                                break;
                            default:
                                And(x => x.UnitPrice == val);
                                break;
                        }
						}
				    
					
				    				
					
				    						if (rule.field == "StockQty" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.StockQty == val);
                                break;
                            case "notequal":
                                And(x => x.StockQty != val);
                                break;
                            case "less":
                                And(x => x.StockQty < val);
                                break;
                            case "lessorequal":
                                And(x => x.StockQty <= val);
                                break;
                            case "greater":
                                And(x => x.StockQty > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.StockQty >= val);
                                break;
                            default:
                                And(x => x.StockQty == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    
					
					
				    						if (rule.field == "IsRequiredQc" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
						{	
							 var boolval=Convert.ToBoolean(rule.value);
							 And(x => x.IsRequiredQc == boolval);
						}
				   				
					
				    
					
											if (rule.field == "ConfirmDateTime" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.ConfirmDateTime) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.ConfirmDateTime) <= 0);
						    }
						}
				   
				    				
					
				    						if (rule.field == "CategoryId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.CategoryId == val);
                                break;
                            case "notequal":
                                And(x => x.CategoryId != val);
                                break;
                            case "less":
                                And(x => x.CategoryId < val);
                                break;
                            case "lessorequal":
                                And(x => x.CategoryId <= val);
                                break;
                            case "greater":
                                And(x => x.CategoryId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.CategoryId >= val);
                                break;
                            default:
                                And(x => x.CategoryId == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    
					
											if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
						    }
						}
				   
				    				
											if (rule.field == "CreatedBy"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.CreatedBy.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
					
											if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
						    }
						}
				   
				    				
											if (rule.field == "LastModifiedBy"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.LastModifiedBy.Contains(rule.value));
						}
				    
				    
					
					
				    									
                   
               }
           }
            return this;
        }



                 public  ProductQuery ByCategoryIdWithfilter(int categoryid, IEnumerable<filterRule> filters)
         {
            And(x => x.CategoryId == categoryid);
            
            if (filters != null)
           {
               foreach (var rule in filters)
               {
                     
                
					
				    						if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Id == val);
                                break;
                            case "notequal":
                                And(x => x.Id != val);
                                break;
                            case "less":
                                And(x => x.Id < val);
                                break;
                            case "lessorequal":
                                And(x => x.Id <= val);
                                break;
                            case "greater":
                                And(x => x.Id > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Id >= val);
                                break;
                            default:
                                And(x => x.Id == val);
                                break;
                        }
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
							switch (rule.op) {
                            case "equal":
                                And(x => x.UnitPrice == val);
                                break;
                            case "notequal":
                                And(x => x.UnitPrice != val);
                                break;
                            case "less":
                                And(x => x.UnitPrice < val);
                                break;
                            case "lessorequal":
                                And(x => x.UnitPrice <= val);
                                break;
                            case "greater":
                                And(x => x.UnitPrice > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.UnitPrice >= val);
                                break;
                            default:
                                And(x => x.UnitPrice == val);
                                break;
                        }
						}
				    
					
				    				
					
				    						if (rule.field == "StockQty" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.StockQty == val);
                                break;
                            case "notequal":
                                And(x => x.StockQty != val);
                                break;
                            case "less":
                                And(x => x.StockQty < val);
                                break;
                            case "lessorequal":
                                And(x => x.StockQty <= val);
                                break;
                            case "greater":
                                And(x => x.StockQty > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.StockQty >= val);
                                break;
                            default:
                                And(x => x.StockQty == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    
					
					
				    						if (rule.field == "IsRequiredQc" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
						{	
							 var boolval=Convert.ToBoolean(rule.value);
							 And(x => x.IsRequiredQc == boolval);
						}
				   				
					
				    
					
											if (rule.field == "ConfirmDateTime" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.ConfirmDateTime) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.ConfirmDateTime) <= 0);
						    }
                        }
				   
				    				
					
				    						if (rule.field == "CategoryId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.CategoryId == val);
                                break;
                            case "notequal":
                                And(x => x.CategoryId != val);
                                break;
                            case "less":
                                And(x => x.CategoryId < val);
                                break;
                            case "lessorequal":
                                And(x => x.CategoryId <= val);
                                break;
                            case "greater":
                                And(x => x.CategoryId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.CategoryId >= val);
                                break;
                            default:
                                And(x => x.CategoryId == val);
                                break;
                        }
						}
				    
					
					
				    				
               }
            }
            return this;
         }
             
            }
}
