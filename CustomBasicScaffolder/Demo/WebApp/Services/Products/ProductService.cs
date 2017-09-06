             
           
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using WebApp.Extensions;
using System.IO;

namespace WebApp.Services
{
    public class ProductService : Service< Product >, IProductService
    {

        private readonly IRepositoryAsync<Product> _repository;
		 private readonly IDataTableImportMappingService _mappingservice;
        public  ProductService(IRepositoryAsync< Product> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository=repository;
			_mappingservice = mappingservice;
        }
        
                 public  IEnumerable<Product> GetByCategoryId(int  categoryid)
         {
            return _repository.GetByCategoryId(categoryid);
         }
                   
        

		public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                 
                Product item = new Product();
				var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "Product" &&  x.IsEnabled==true).ToList();

                foreach (var field in mapping)
                {
                 
						var defval = field.DefaultValue;
						var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));
						if (contation && row[field.SourceFieldName] != DBNull.Value)
						{
							Type producttype = item.GetType();
							PropertyInfo propertyInfo = producttype.GetProperty(field.FieldName);
							propertyInfo.SetValue(item, Convert.ChangeType(row[field.SourceFieldName], propertyInfo.PropertyType), null);
						}
						else if (!string.IsNullOrEmpty(defval))
						{
							Type producttype = item.GetType();
							PropertyInfo propertyInfo = producttype.GetProperty(field.FieldName);
							if (defval.ToLower() == "now" && propertyInfo.PropertyType ==typeof(DateTime))
                            {
                                propertyInfo.SetValue(item, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
                            }
                            else
                            {
                                propertyInfo.SetValue(item, Convert.ChangeType(defval, propertyInfo.PropertyType), null);
                            }
						}
                }
                
                this.Insert(item);
               

            }
        }
		
		public Stream ExportExcel(string filterRules = "",string sort = "Id", string order = "asc")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
                       			 
            var products  = this.Query(new ProductQuery().Withfilter(filters)).Include(p => p.Category).OrderBy(n=>n.OrderBy(sort,order)).Select().ToList();
            
                        var datarows = products .Select(  n => new { CategoryName = (n.Category==null?"": n.Category.Name) , Id = n.Id , Name = n.Name , Unit = n.Unit , UnitPrice = n.UnitPrice , StockQty = n.StockQty , ConfirmDateTime = n.ConfirmDateTime , IsRequiredQc = n.IsRequiredQc , CategoryId = n.CategoryId }).ToList();
           
            return ExcelHelper.ExportExcel(typeof(Product), datarows);

        }
    }
}



