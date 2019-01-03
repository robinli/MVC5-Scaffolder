/// <summary>
/// File: ProductService.cs
/// Purpose: Within the service layer, you define and implement 
/// the service interface and the data contracts (or message types).
/// One of the more important concepts to keep in mind is that a service
/// should never expose details of the internal processes or 
/// the business entities used within the application. 
/// Date: 2018/10/18 16:29:56
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
using Repository.Pattern.Repositories;
using Repository.Pattern.Infrastructure;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
namespace WebApp.Services
{
    public class ProductService : Service< Product >, IProductService
    {
        private readonly IRepositoryAsync<Product> repository;
		private readonly IDataTableImportMappingService mappingservice;
        public  ProductService(IRepositoryAsync< Product> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            this.repository=repository;
			this.mappingservice = mappingservice;
        }
                 public  IEnumerable<Product> GetByCategoryId(int  categoryid) => repository.GetByCategoryId(categoryid);
                   
        
        		 
                private int getCategoryIdByName(string name)
        {
            var categoryRepository = this.repository.GetRepository<Category>();
            var category = categoryRepository.Queryable().Where(x => x.Name == name).FirstOrDefault();
            if (category == null)
            {
                throw new Exception("not found ForeignKey:CategoryId with " + name);
            }
            else
            {
                return category.Id;
            }
        }
                
        public void ImportDataTable(System.Data.DataTable datatable)
        {
            var mapping = this.mappingservice.Queryable().Where(x => x.EntitySetName == "Product" && ( ( x.IsEnabled == true ) || ( x.IsEnabled == false && !( x.DefaultValue == null || x.DefaultValue.Equals(string.Empty) ) ) )).ToList();
            if (mapping == null || mapping.Count == 0)
            {
                throw new KeyNotFoundException("没有找到Product对象的Excel导入配置信息，请执行[系统管理/Excel导入配置]");
            }
            foreach (DataRow row in datatable.Rows)
            {
                var item = new Product();
                var requiredfield = mapping.Where(x => x.IsRequired == true).FirstOrDefault()?.SourceFieldName;
                if (requiredfield != null && !row.IsNull(requiredfield) &&  row[requiredfield] != DBNull.Value && Convert.ToString(row[requiredfield]).Trim() != string.Empty)
                {
                    foreach (var field in mapping)
                    {
						var defval = field.DefaultValue;
						var contain = datatable.Columns.Contains(field.SourceFieldName ?? "");
						if (contain && !row.IsNull(field.SourceFieldName) && row[field.SourceFieldName] != DBNull.Value && row[field.SourceFieldName].ToString()!=string.Empty )
						{
                            var producttype = item.GetType();
							var propertyInfo = producttype.GetProperty(field.FieldName);
                                                        //关联外键查询获取Id
                            switch (field.FieldName) {
                                                                 case "CategoryId":
                                     var name =  row[field.SourceFieldName].ToString();
                                     var categoryid = this.getCategoryIdByName(name);
                                     propertyInfo.SetValue(item, Convert.ChangeType(categoryid, propertyInfo.PropertyType), null);
                                     break;
                                                                default:
                                    var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                                    var safeValue = (row[field.SourceFieldName] == null) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
                                    propertyInfo.SetValue(item, safeValue, null);
                                    break;
                            }
                                                    }
						else if (!string.IsNullOrEmpty(defval))
						{
							var producttype = item.GetType();
							var propertyInfo = producttype.GetProperty(field.FieldName);
							if (defval.ToLower() == "now" && propertyInfo.PropertyType ==typeof(DateTime))
                            {
                                propertyInfo.SetValue(item, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
                            }
                            else
                            {
                                var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                                var safeValue = Convert.ChangeType(defval, safetype);
                                propertyInfo.SetValue(item, safeValue, null);
                            }
						}
                    }
                    this.Insert(item);
               }

            }
        }
		
		public Stream ExportExcel(string filterRules = "",string sort = "Id", string order = "asc")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
                       			 
            var products  = this.Query(new ProductQuery().Withfilter(filters)).Include(p => p.Category).OrderBy(n=>n.OrderBy(sort,order)).Select().ToList();
            
                        var datarows = products .Select(  n => new { 

    CategoryName = (n.Category==null?"": n.Category.Name) ,
    Id = n.Id,
    Name = n.Name,
    Unit = n.Unit,
    UnitPrice = n.UnitPrice,
    StockQty = n.StockQty,
    IsRequiredQc = n.IsRequiredQc,
    ConfirmDateTime = n.ConfirmDateTime,
    CategoryId = n.CategoryId
}).ToList();
           
            return ExcelHelper.ExportExcel(typeof(Product), datarows);

        }
    }
}



