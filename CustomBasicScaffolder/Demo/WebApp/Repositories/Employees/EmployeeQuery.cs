// <copyright file="EmployeeQuery.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2/6/2018 10:11:11 AM </date>
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
   public class EmployeeQuery:QueryObject<Employee>
    {
        public EmployeeQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Title.Contains(search) || x.Sex.Contains(search) || x.Age.ToString().Contains(search) || x.Brithday.ToString().Contains(search) || x.IsDeleted.ToString().Contains(search) || x.CompanyId.ToString().Contains(search) );
            return this;
        }


		public EmployeeQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
				    
					
					
				    				
											if (rule.field == "Title"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Title.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Sex"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Sex.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    						if (rule.field == "Age" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Age == val);
                                break;
                            case "notequal":
                                And(x => x.Age != val);
                                break;
                            case "less":
                                And(x => x.Age < val);
                                break;
                            case "lessorequal":
                                And(x => x.Age <= val);
                                break;
                            case "greater":
                                And(x => x.Age > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Age >= val);
                                break;
                            default:
                                And(x => x.Age == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    
					
											if (rule.field == "Brithday" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.Brithday) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.Brithday) <= 0);
						    }
						}
				   
				    				
					
				    						if (rule.field == "IsDeleted" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.IsDeleted == val);
                                break;
                            case "notequal":
                                And(x => x.IsDeleted != val);
                                break;
                            case "less":
                                And(x => x.IsDeleted < val);
                                break;
                            case "lessorequal":
                                And(x => x.IsDeleted <= val);
                                break;
                            case "greater":
                                And(x => x.IsDeleted > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.IsDeleted >= val);
                                break;
                            default:
                                And(x => x.IsDeleted == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    						if (rule.field == "CompanyId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.CompanyId == val);
                                break;
                            case "notequal":
                                And(x => x.CompanyId != val);
                                break;
                            case "less":
                                And(x => x.CompanyId < val);
                                break;
                            case "lessorequal":
                                And(x => x.CompanyId <= val);
                                break;
                            case "greater":
                                And(x => x.CompanyId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.CompanyId >= val);
                                break;
                            default:
                                And(x => x.CompanyId == val);
                                break;
                        }
						}
				    
					
					
				    									
                   
               }
           }
            return this;
        }



                 public  EmployeeQuery ByCompanyIdWithfilter(int companyid, IEnumerable<filterRule> filters)
         {
            And(x => x.CompanyId == companyid);
            
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
				    
				    
					
					
				    				
											if (rule.field == "Title"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Title.Contains(rule.value));
						}
				    
				    
					
					
				    				
											if (rule.field == "Sex"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Sex.Contains(rule.value));
						}
				    
				    
					
					
				    				
					
				    						if (rule.field == "Age" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Age == val);
                                break;
                            case "notequal":
                                And(x => x.Age != val);
                                break;
                            case "less":
                                And(x => x.Age < val);
                                break;
                            case "lessorequal":
                                And(x => x.Age <= val);
                                break;
                            case "greater":
                                And(x => x.Age > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Age >= val);
                                break;
                            default:
                                And(x => x.Age == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    
					
											if (rule.field == "Brithday" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.Brithday) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.Brithday) <= 0);
						    }
                        }
				   
				    				
					
				    						if (rule.field == "IsDeleted" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.IsDeleted == val);
                                break;
                            case "notequal":
                                And(x => x.IsDeleted != val);
                                break;
                            case "less":
                                And(x => x.IsDeleted < val);
                                break;
                            case "lessorequal":
                                And(x => x.IsDeleted <= val);
                                break;
                            case "greater":
                                And(x => x.IsDeleted > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.IsDeleted >= val);
                                break;
                            default:
                                And(x => x.IsDeleted == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    						if (rule.field == "CompanyId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.CompanyId == val);
                                break;
                            case "notequal":
                                And(x => x.CompanyId != val);
                                break;
                            case "less":
                                And(x => x.CompanyId < val);
                                break;
                            case "lessorequal":
                                And(x => x.CompanyId <= val);
                                break;
                            case "greater":
                                And(x => x.CompanyId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.CompanyId >= val);
                                break;
                            default:
                                And(x => x.CompanyId == val);
                                break;
                        }
						}
				    
					
					
				    				
               }
            }
            return this;
         }
             
            }
}



