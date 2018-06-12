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
using Repository.Pattern.Infrastructure;
using System.IO;

namespace WebApp.Services
{
    public class OrderDetailService : Service< OrderDetail >, IOrderDetailService
    {

        private readonly IRepositoryAsync<OrderDetail> _repository;
		 private readonly IDataTableImportMappingService _mappingservice;
        public  OrderDetailService(IRepositoryAsync< OrderDetail> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository=repository;
			_mappingservice = mappingservice;
        }
        
                 public  IEnumerable<OrderDetail> GetByProductId(int  productid)
         {
            return _repository.GetByProductId(productid);
         }
                  public  IEnumerable<OrderDetail> GetByOrderId(int  orderid)
         {
            return _repository.GetByOrderId(orderid);
         }
                   
        

		public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                 
                OrderDetail item = new OrderDetail();
				var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "OrderDetail" &&  x.IsEnabled==true).ToList();

                foreach (var field in mapping)
                {
                 
						var defval = field.DefaultValue;
						var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));
						if (contation && row[field.SourceFieldName] != DBNull.Value)
						{
							Type orderdetailtype = item.GetType();
							PropertyInfo propertyInfo = orderdetailtype.GetProperty(field.FieldName);
							propertyInfo.SetValue(item, Convert.ChangeType(row[field.SourceFieldName], propertyInfo.PropertyType), null);
						}
						else if (!string.IsNullOrEmpty(defval))
						{
							Type orderdetailtype = item.GetType();
							PropertyInfo propertyInfo = orderdetailtype.GetProperty(field.FieldName);
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
                       			 
            var orderdetails  = this.Query(new OrderDetailQuery().Withfilter(filters)).Include(p => p.Order).Include(p => p.Product).OrderBy(n=>n.OrderBy(sort,order)).Select().ToList();
            
                        var datarows = orderdetails .Select(  n => new { OrderCustomer = (n.Order==null?"": n.Order.Customer) ,ProductName = (n.Product==null?"": n.Product.Name) , Id = n.Id , ProductId = n.ProductId , Qty = n.Qty , Price = n.Price , Amount = n.Amount , OrderId = n.OrderId }).ToList();
           
            return ExcelHelper.ExportExcel(typeof(OrderDetail), datarows);

        }
    }
}



