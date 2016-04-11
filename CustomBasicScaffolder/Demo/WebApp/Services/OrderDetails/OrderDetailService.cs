
             
           
 

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
                foreach (DataColumn col in datatable.Columns)
                {
                    var sourcefieldname = col.ColumnName;
                    var mapping = _mappingservice.FindMapping("OrderDetail", sourcefieldname);
                    if (mapping != null && row[sourcefieldname] != DBNull.Value)
                    {
                        
                        Type orderdetailtype = item.GetType();
						PropertyInfo propertyInfo = orderdetailtype.GetProperty(mapping.FieldName);
						propertyInfo.SetValue(item, Convert.ChangeType(row[sourcefieldname], propertyInfo.PropertyType), null);
                        //orderdetailtype.GetProperty(mapping.FieldName).SetValue(item, row[sourcefieldname]);
                    }

                }
                
                this.Insert(item);
               

            }
        }
    }
}



