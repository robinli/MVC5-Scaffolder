             
           
 
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
    public class DepartmentService : Service< Department >, IDepartmentService
    {

        private readonly IRepositoryAsync<Department> _repository;
		 private readonly IDataTableImportMappingService _mappingservice;
        public  DepartmentService(IRepositoryAsync< Department> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository=repository;
			_mappingservice = mappingservice;
        }
        
                 public  IEnumerable<Department> GetByCompanyId(int  companyid)
         {
            return _repository.GetByCompanyId(companyid);
         }
                   
        

		public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                 
                Department item = new Department();
				var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "Department").ToList();

                foreach (var field in mapping)
                {
                 
						var defval = field.DefaultValue;
						var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));
						if (contation && row[field.SourceFieldName] != DBNull.Value)
						{
							Type departmenttype = item.GetType();
							PropertyInfo propertyInfo = departmenttype.GetProperty(field.FieldName);
							propertyInfo.SetValue(item, Convert.ChangeType(row[field.SourceFieldName], propertyInfo.PropertyType), null);
						}
						else if (!string.IsNullOrEmpty(defval))
						{
							Type departmenttype = item.GetType();
							PropertyInfo propertyInfo = departmenttype.GetProperty(field.FieldName);
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
                       			 
            var departments  = this.Query(new DepartmentQuery().Withfilter(filters)).Include(p => p.Company).OrderBy(n=>n.OrderBy(sort,order)).Select().ToList();
            
                        var datarows = departments .Select(  n => new { CompanyName = (n.Company==null?"": n.Company.Name) , Id = n.Id , Name = n.Name , Manager = n.Manager , CompanyId = n.CompanyId }).ToList();
           
            return ExcelHelper.ExportExcel(typeof(Department), datarows);

        }
    }
}



