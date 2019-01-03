// <copyright file="MenuItemQuery.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>9/26/2018 4:20:16 PM </date>
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
   public class MenuItemQuery:QueryObject<MenuItem>
    {
        public MenuItemQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Code.Contains(search) || x.Url.Contains(search) || x.Controller.Contains(search) || x.Action.Contains(search) || x.IconCls.Contains(search) || x.ParentId.ToString().Contains(search) );
            return this;
        }


		public MenuItemQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
					
					
				    				
											if (rule.field == "Title"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Title.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Description"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Description.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Code"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Code.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Url"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Url.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Controller"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Controller.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Action"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Action.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "IconCls"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.IconCls.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
					
					
				    						if (rule.field == "IsEnabled" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
						{	
							 var boolval=Convert.ToBoolean(rule.value);
							 And(x => x.IsEnabled == boolval);
						}
				   				
					
				    						if (rule.field == "ParentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ParentId == val);
                                break;
                            case "notequal":
                                And(x => x.ParentId != val);
                                break;
                            case "less":
                                And(x => x.ParentId < val);
                                break;
                            case "lessorequal":
                                And(x => x.ParentId <= val);
                                break;
                            case "greater":
                                And(x => x.ParentId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ParentId >= val);
                                break;
                            default:
                                And(x => x.ParentId == val);
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



                 public  MenuItemQuery ByParentIdWithfilter(int parentid, IEnumerable<filterRule> filters)
         {
            And(x => x.ParentId == parentid);
            
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
				    
					
					
				    				
											if (rule.field == "Title"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Title.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Description"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Description.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Code"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Code.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Url"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Url.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Controller"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Controller.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Action"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Action.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "IconCls"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.IconCls.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
					
					
				    						if (rule.field == "IsEnabled" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
						{	
							 var boolval=Convert.ToBoolean(rule.value);
							 And(x => x.IsEnabled == boolval);
						}
				   				
					
				    						if (rule.field == "ParentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ParentId == val);
                                break;
                            case "notequal":
                                And(x => x.ParentId != val);
                                break;
                            case "less":
                                And(x => x.ParentId < val);
                                break;
                            case "lessorequal":
                                And(x => x.ParentId <= val);
                                break;
                            case "greater":
                                And(x => x.ParentId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ParentId >= val);
                                break;
                            default:
                                And(x => x.ParentId == val);
                                break;
                        }
						}
				    
					
					
				    				
               }
            }
            return this;
         }
             
            }
}



