
             
           
 

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
    public class OrderService : Service< Order >, IOrderService
    {

        private readonly IRepositoryAsync<Order> _repository;
		 private readonly IDataTableImportMappingService _mappingservice;
        public  OrderService(IRepositoryAsync< Order> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository=repository;
			_mappingservice = mappingservice;
        }
        
                         public IEnumerable<OrderDetail>   GetOrderDetailsByOrderId (int orderid)
        {
            return _repository.GetOrderDetailsByOrderId(orderid);
        }
         
        

		public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                 
                Order item = new Order();
                foreach (DataColumn col in datatable.Columns)
                {
                    var sourcefieldname = col.ColumnName;
                    var mapping = _mappingservice.FindMapping("Order", sourcefieldname);
                    if (mapping != null && row[sourcefieldname] != DBNull.Value)
                    {
                        
                        Type ordertype = item.GetType();
						PropertyInfo propertyInfo = ordertype.GetProperty(mapping.FieldName);
						propertyInfo.SetValue(item, Convert.ChangeType(row[sourcefieldname], propertyInfo.PropertyType), null);
                        //ordertype.GetProperty(mapping.FieldName).SetValue(item, row[sourcefieldname]);
                    }

                }
                
                this.Insert(item);
               

            }
        }
    }
}



