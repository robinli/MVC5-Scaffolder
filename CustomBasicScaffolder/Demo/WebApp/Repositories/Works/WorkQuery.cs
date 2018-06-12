// <copyright file="WorkQuery.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>5/22/2018 8:27:47 AM </date>
// <summary>
// 配合 easyui datagrid filter 组件使用,实现对datagrid 所有字段筛选功能
// 也可以对特定的业务逻辑查询进行封装
//  
//  
// </summary>

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
   public class WorkQuery:QueryObject<Work>
    {
        public WorkQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Status.Contains(search) || x.StartDate.ToString().Contains(search) || x.EndDate.ToString().Contains(search) || x.Hour.ToString().Contains(search) || x.Priority.ToString().Contains(search) || x.Score.ToString().Contains(search) );
            return this;
        }


		public WorkQuery Withfilter(IEnumerable<filterRule> filters)
        {
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
				    
				    
					
					
				    				
											if (rule.field == "Status"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Status.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
					
											if (rule.field == "StartDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.StartDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.StartDate) <= 0);
						    }
						}
				   
				    				
					
				    
					
											if (rule.field == "EndDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.EndDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.EndDate) <= 0);
						    }
						}
				   
				    				
					
				    
					
					
				    						if (rule.field == "Enableed" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
						{	
							 var boolval=Convert.ToBoolean(rule.value);
							 And(x => x.Enableed == boolval);
						}
				   				
					
				    						if (rule.field == "Hour" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Hour == val);
                                break;
                            case "notequal":
                                And(x => x.Hour != val);
                                break;
                            case "less":
                                And(x => x.Hour < val);
                                break;
                            case "lessorequal":
                                And(x => x.Hour <= val);
                                break;
                            case "greater":
                                And(x => x.Hour > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Hour >= val);
                                break;
                            default:
                                And(x => x.Hour == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    						if (rule.field == "Priority" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Priority == val);
                                break;
                            case "notequal":
                                And(x => x.Priority != val);
                                break;
                            case "less":
                                And(x => x.Priority < val);
                                break;
                            case "lessorequal":
                                And(x => x.Priority <= val);
                                break;
                            case "greater":
                                And(x => x.Priority > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Priority >= val);
                                break;
                            default:
                                And(x => x.Priority == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    
											if (rule.field == "Score" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Score == val);
                                break;
                            case "notequal":
                                And(x => x.Score != val);
                                break;
                            case "less":
                                And(x => x.Score < val);
                                break;
                            case "lessorequal":
                                And(x => x.Score <= val);
                                break;
                            case "greater":
                                And(x => x.Score > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Score >= val);
                                break;
                            default:
                                And(x => x.Score == val);
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



            }
}



