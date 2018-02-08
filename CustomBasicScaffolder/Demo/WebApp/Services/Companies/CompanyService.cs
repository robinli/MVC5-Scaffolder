// <copyright file="CompanyService.cs" company="neozhu/MVC5-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>2/8/2018 2:19:13 PM </date>
// <summary>
//  实现定义的业务逻辑,通过依赖注入降低模块之间的耦合度
//   
//  
//  
// </summary>

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
    public class CompanyService : Service< Company >, ICompanyService
    {

        private readonly IRepositoryAsync<Company> _repository;
		 private readonly IDataTableImportMappingService _mappingservice;
        public  CompanyService(IRepositoryAsync< Company> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository=repository;
			_mappingservice = mappingservice;
        }
        
                         public IEnumerable<Department>   GetDepartmentsByCompanyId (int companyid)
        {
            return _repository.GetDepartmentsByCompanyId(companyid);
        }
                public IEnumerable<Employee>   GetEmployeeByCompanyId (int companyid)
        {
            return _repository.GetEmployeeByCompanyId(companyid);
        }
         
        

		public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                 
                Company item = new Company();
				var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "Company" &&  x.IsEnabled==true).ToList();

                foreach (var field in mapping)
                {
                 
						var defval = field.DefaultValue;
						var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));
						if (contation && row[field.SourceFieldName] != DBNull.Value)
						{
							Type companytype = item.GetType();
							PropertyInfo propertyInfo = companytype.GetProperty(field.FieldName);
							propertyInfo.SetValue(item, Convert.ChangeType(row[field.SourceFieldName], propertyInfo.PropertyType), null);
						}
						else if (!string.IsNullOrEmpty(defval))
						{
							Type companytype = item.GetType();
							PropertyInfo propertyInfo = companytype.GetProperty(field.FieldName);
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
                                   var companies  = this.Query(new CompanyQuery().Withfilter(filters)).OrderBy(n=>n.OrderBy(sort,order)).Select().ToList();
                        var datarows = companies .Select(  n => new {  Id = n.Id , Name = n.Name , Address = n.Address , City = n.City , Province = n.Province , RegisterDate = n.RegisterDate , Employees = n.Employees }).ToList();
           
            return ExcelHelper.ExportExcel(typeof(Company), datarows);

        }
    }
}



