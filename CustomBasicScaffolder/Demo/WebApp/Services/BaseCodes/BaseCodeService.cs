             
           
 
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
 
using System.IO;

namespace WebApp.Services
{
    public class BaseCodeService : Service< BaseCode >, IBaseCodeService
    {

        private readonly IRepositoryAsync<BaseCode> _repository;
		 private readonly IDataTableImportMappingService _mappingservice;
        public  BaseCodeService(IRepositoryAsync< BaseCode> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository=repository;
			_mappingservice = mappingservice;
        }
        
                         
         
        

		public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                 
                BaseCode item = new BaseCode();
				var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "BaseCode" &&  x.IsEnabled==true).ToList();

                foreach (var field in mapping)
                {
                 
						var defval = field.DefaultValue;
						var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));
						if (contation && row[field.SourceFieldName] != DBNull.Value)
						{
							Type basecodetype = item.GetType();
							PropertyInfo propertyInfo = basecodetype.GetProperty(field.FieldName);
							propertyInfo.SetValue(item, Convert.ChangeType(row[field.SourceFieldName], propertyInfo.PropertyType), null);
						}
						else if (!string.IsNullOrEmpty(defval))
						{
							Type basecodetype = item.GetType();
							PropertyInfo propertyInfo = basecodetype.GetProperty(field.FieldName);
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
                                   var basecodes  = this.Query(new BaseCodeQuery().Withfilter(filters)).OrderBy(n=>n.OrderBy(sort,order)).Select().ToList();
                        var datarows = basecodes .Select(  n => new {  Id = n.Id , CodeType = n.CodeType , Description = n.Description }).ToList();
           
            return ExcelHelper.ExportExcel(typeof(BaseCode), datarows);

        }
    }
}



