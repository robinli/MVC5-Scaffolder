// <copyright file="OrderDetailQuery.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>9/29/2018 11:07:45 AM </date>
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
   public class OrderDetailQuery:QueryObject<OrderDetail>
   {

		public OrderDetailQuery Withfilter(IEnumerable<filterRule> filters)
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
				    
					
					
				    				
					
				    						if (rule.field == "ProductId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ProductId == val);
                                break;
                            case "notequal":
                                And(x => x.ProductId != val);
                                break;
                            case "less":
                                And(x => x.ProductId < val);
                                break;
                            case "lessorequal":
                                And(x => x.ProductId <= val);
                                break;
                            case "greater":
                                And(x => x.ProductId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ProductId >= val);
                                break;
                            default:
                                And(x => x.ProductId == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    						if (rule.field == "Qty" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Qty == val);
                                break;
                            case "notequal":
                                And(x => x.Qty != val);
                                break;
                            case "less":
                                And(x => x.Qty < val);
                                break;
                            case "lessorequal":
                                And(x => x.Qty <= val);
                                break;
                            case "greater":
                                And(x => x.Qty > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Qty >= val);
                                break;
                            default:
                                And(x => x.Qty == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    
											if (rule.field == "Price" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Price == val);
                                break;
                            case "notequal":
                                And(x => x.Price != val);
                                break;
                            case "less":
                                And(x => x.Price < val);
                                break;
                            case "lessorequal":
                                And(x => x.Price <= val);
                                break;
                            case "greater":
                                And(x => x.Price > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Price >= val);
                                break;
                            default:
                                And(x => x.Price == val);
                                break;
                        }
						}
				    
					
				    				
					
				    
											if (rule.field == "Amount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Amount == val);
                                break;
                            case "notequal":
                                And(x => x.Amount != val);
                                break;
                            case "less":
                                And(x => x.Amount < val);
                                break;
                            case "lessorequal":
                                And(x => x.Amount <= val);
                                break;
                            case "greater":
                                And(x => x.Amount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Amount >= val);
                                break;
                            default:
                                And(x => x.Amount == val);
                                break;
                        }
						}
				    
					
				    				
					
				    						if (rule.field == "OrderId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.OrderId == val);
                                break;
                            case "notequal":
                                And(x => x.OrderId != val);
                                break;
                            case "less":
                                And(x => x.OrderId < val);
                                break;
                            case "lessorequal":
                                And(x => x.OrderId <= val);
                                break;
                            case "greater":
                                And(x => x.OrderId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.OrderId >= val);
                                break;
                            default:
                                And(x => x.OrderId == val);
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



                 public  OrderDetailQuery ByProductIdWithfilter(int productid, IEnumerable<filterRule> filters)
         {
            And(x => x.ProductId == productid);
            
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
				    
					
					
				    				
					
				    						if (rule.field == "ProductId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ProductId == val);
                                break;
                            case "notequal":
                                And(x => x.ProductId != val);
                                break;
                            case "less":
                                And(x => x.ProductId < val);
                                break;
                            case "lessorequal":
                                And(x => x.ProductId <= val);
                                break;
                            case "greater":
                                And(x => x.ProductId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ProductId >= val);
                                break;
                            default:
                                And(x => x.ProductId == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    						if (rule.field == "Qty" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Qty == val);
                                break;
                            case "notequal":
                                And(x => x.Qty != val);
                                break;
                            case "less":
                                And(x => x.Qty < val);
                                break;
                            case "lessorequal":
                                And(x => x.Qty <= val);
                                break;
                            case "greater":
                                And(x => x.Qty > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Qty >= val);
                                break;
                            default:
                                And(x => x.Qty == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    
											if (rule.field == "Price" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Price == val);
                                break;
                            case "notequal":
                                And(x => x.Price != val);
                                break;
                            case "less":
                                And(x => x.Price < val);
                                break;
                            case "lessorequal":
                                And(x => x.Price <= val);
                                break;
                            case "greater":
                                And(x => x.Price > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Price >= val);
                                break;
                            default:
                                And(x => x.Price == val);
                                break;
                        }
						}
				    
					
				    				
					
				    
											if (rule.field == "Amount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Amount == val);
                                break;
                            case "notequal":
                                And(x => x.Amount != val);
                                break;
                            case "less":
                                And(x => x.Amount < val);
                                break;
                            case "lessorequal":
                                And(x => x.Amount <= val);
                                break;
                            case "greater":
                                And(x => x.Amount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Amount >= val);
                                break;
                            default:
                                And(x => x.Amount == val);
                                break;
                        }
						}
				    
					
				    				
					
				    						if (rule.field == "OrderId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.OrderId == val);
                                break;
                            case "notequal":
                                And(x => x.OrderId != val);
                                break;
                            case "less":
                                And(x => x.OrderId < val);
                                break;
                            case "lessorequal":
                                And(x => x.OrderId <= val);
                                break;
                            case "greater":
                                And(x => x.OrderId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.OrderId >= val);
                                break;
                            default:
                                And(x => x.OrderId == val);
                                break;
                        }
						}
				    
					
					
				    				
               }
            }
            return this;
         }
             
                 public  OrderDetailQuery ByOrderIdWithfilter(int orderid, IEnumerable<filterRule> filters)
         {
            And(x => x.OrderId == orderid);
            
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
				    
					
					
				    				
					
				    						if (rule.field == "ProductId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ProductId == val);
                                break;
                            case "notequal":
                                And(x => x.ProductId != val);
                                break;
                            case "less":
                                And(x => x.ProductId < val);
                                break;
                            case "lessorequal":
                                And(x => x.ProductId <= val);
                                break;
                            case "greater":
                                And(x => x.ProductId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ProductId >= val);
                                break;
                            default:
                                And(x => x.ProductId == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    						if (rule.field == "Qty" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Qty == val);
                                break;
                            case "notequal":
                                And(x => x.Qty != val);
                                break;
                            case "less":
                                And(x => x.Qty < val);
                                break;
                            case "lessorequal":
                                And(x => x.Qty <= val);
                                break;
                            case "greater":
                                And(x => x.Qty > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Qty >= val);
                                break;
                            default:
                                And(x => x.Qty == val);
                                break;
                        }
						}
				    
					
					
				    				
					
				    
											if (rule.field == "Price" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Price == val);
                                break;
                            case "notequal":
                                And(x => x.Price != val);
                                break;
                            case "less":
                                And(x => x.Price < val);
                                break;
                            case "lessorequal":
                                And(x => x.Price <= val);
                                break;
                            case "greater":
                                And(x => x.Price > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Price >= val);
                                break;
                            default:
                                And(x => x.Price == val);
                                break;
                        }
						}
				    
					
				    				
					
				    
											if (rule.field == "Amount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Amount == val);
                                break;
                            case "notequal":
                                And(x => x.Amount != val);
                                break;
                            case "less":
                                And(x => x.Amount < val);
                                break;
                            case "lessorequal":
                                And(x => x.Amount <= val);
                                break;
                            case "greater":
                                And(x => x.Amount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Amount >= val);
                                break;
                            default:
                                And(x => x.Amount == val);
                                break;
                        }
						}
				    
					
				    				
					
				    						if (rule.field == "OrderId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							int val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.OrderId == val);
                                break;
                            case "notequal":
                                And(x => x.OrderId != val);
                                break;
                            case "less":
                                And(x => x.OrderId < val);
                                break;
                            case "lessorequal":
                                And(x => x.OrderId <= val);
                                break;
                            case "greater":
                                And(x => x.OrderId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.OrderId >= val);
                                break;
                            default:
                                And(x => x.OrderId == val);
                                break;
                        }
						}
				    
					
					
				    				
               }
            }
            return this;
         }
             
            }
}
