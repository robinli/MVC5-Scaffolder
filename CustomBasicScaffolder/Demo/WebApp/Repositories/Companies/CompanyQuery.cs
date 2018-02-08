// <copyright file="CompanyQuery.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2/8/2018 2:19:12 PM </date>
// <summary>
// 配合 easyui datagrid filter 组件使用,实现对datagrid 所有字段筛选功能
// 也可以对特定的查询进行封装使用 
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
   public class CompanyQuery:QueryObject<Company>
    {
        public CompanyQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Address.Contains(search) || x.City.Contains(search) || x.Province.Contains(search) || x.RegisterDate.ToString().Contains(search) || x.Employees.ToString().Contains(search) );
            return this;
        }


		public CompanyQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
				    
					
					
				    				
											if (rule.field == "Address"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Address.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "City"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.City.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Province"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Province.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    
					
											if (rule.field == "RegisterDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.RegisterDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.RegisterDate) <= 0);
						    }
						}
				   
				    				
					
				    						if (rule.field == "Employees" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Employees == val);
                                break;
                            case "notequal":
                                And(x => x.Employees != val);
                                break;
                            case "less":
                                And(x => x.Employees < val);
                                break;
                            case "lessorequal":
                                And(x => x.Employees <= val);
                                break;
                            case "greater":
                                And(x => x.Employees > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Employees >= val);
                                break;
                            default:
                                And(x => x.Employees == val);
                                break;
                        }
						}
				    
					
					
				    									
                   
               }
           }
            return this;
        }



            }
}



