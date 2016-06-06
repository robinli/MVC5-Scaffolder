
             
           
 

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
                foreach (DataColumn col in datatable.Columns)
                {
                    var sourcefieldname = col.ColumnName;
                    var mapping = _mappingservice.FindMapping("Product", sourcefieldname);
                    if (mapping != null && row[sourcefieldname] != DBNull.Value)
                    {
                        
                        Type producttype = item.GetType();
						PropertyInfo propertyInfo = producttype.GetProperty(mapping.FieldName);
						propertyInfo.SetValue(item, Convert.ChangeType(row[sourcefieldname], propertyInfo.PropertyType), null);
                        //producttype.GetProperty(mapping.FieldName).SetValue(item, row[sourcefieldname]);
                    }

                }
                
                this.Insert(item);
               

            }
        }


        public FileInfo ExportExcel(string fileName,string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var products = this.Query(new ProductQuery().Withfilter(filters)).Include(p => p.Category).OrderBy(n => n.OrderBy(sort, order)).Select().ToList();
            return ExcelHelper.ExportExcel<Product>(products, fileName);

        }
    }
}



