// <copyright file="EmployeeService.cs" company="neozhu/SmartCode-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>7/23/2018 10:20:54 AM </date>
// <summary>
//  根据需求定义实现具体的业务逻辑,通过依赖注入降低模块之间的耦合度
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
    public class EmployeeService : Service<Employee>, IEmployeeService
    {

        private readonly IRepositoryAsync<Employee> _repository;
        private readonly IDataTableImportMappingService _mappingservice;
        public EmployeeService(IRepositoryAsync<Employee> repository, IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository = repository;
            _mappingservice = mappingservice;
        }

        public IEnumerable<Employee> GetByCompanyId(int companyid)
        {
            return _repository.GetByCompanyId(companyid);
        }



        private int getCompanyIdByName(string name)
        {
            var companyRepository = this._repository.GetRepository<Company>();
            var company = companyRepository.Queryable().Where(x => x.Name == name).FirstOrDefault();
            if (company == null)
            {
                throw new Exception("not found ForeignKey:CompanyId with " + name);
            }
            else
            {
                return company.Id;
            }
        }

        public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                Employee item = new Employee();
                var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "Employee" && ((x.IsEnabled == true) || (x.IsEnabled == false && !(x.DefaultValue == null || x.DefaultValue.Equals(string.Empty))))).ToList();
                var requiredfield = mapping.Where(x => x.IsRequired == true).FirstOrDefault()?.SourceFieldName;
                if (requiredfield != null && !row.IsNull(requiredfield) && row[requiredfield] != DBNull.Value && Convert.ToString(row[requiredfield]).Trim() != string.Empty)
                {
                    foreach (var field in mapping)
                    {
                        var defval = field.DefaultValue;
                        var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));
                        if (contation && !row.IsNull(field.SourceFieldName) && row[field.SourceFieldName] != DBNull.Value)
                        {
                            Type employeetype = item.GetType();
                            PropertyInfo propertyInfo = employeetype.GetProperty(field.FieldName);
                            switch (field.FieldName)
                            {
                                case "CompanyId":
                                    var name = row[field.SourceFieldName].ToString();
                                    var companyid = this.getCompanyIdByName(name);
                                    propertyInfo.SetValue(item, Convert.ChangeType(companyid, propertyInfo.PropertyType), null);
                                    break;
                                default:
                                    propertyInfo.SetValue(item, Convert.ChangeType(row[field.SourceFieldName], propertyInfo.PropertyType), null);
                                    break;
                            }
                        }
                        else if (!string.IsNullOrEmpty(defval))
                        {
                            Type employeetype = item.GetType();
                            PropertyInfo propertyInfo = employeetype.GetProperty(field.FieldName);
                            if (defval.ToLower() == "now" && propertyInfo.PropertyType == typeof(DateTime))
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
        }

        public Stream ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);

            var employees = this.Query(new EmployeeQuery().Withfilter(filters)).Include(p => p.Company).OrderBy(n => n.OrderBy(sort, order)).Select().ToList();

            var datarows = employees.Select(n => new
            {

                CompanyName = (n.Company == null ? "" : n.Company.Name),
                Id = n.Id,
                Name = n.Name,
                Title = n.Title,
                Sex = n.Sex,
                Age = n.Age,
                Brithday = n.Brithday,
                IsDeleted = n.IsDeleted,
                CompanyId = n.CompanyId,
                CreatedDate = n.CreatedDate,
                CreatedBy = n.CreatedBy,
                LastModifiedDate = n.LastModifiedDate,
                LastModifiedBy = n.LastModifiedBy
            }).ToList();

            return ExcelHelper.ExportExcel(typeof(Employee), datarows);

        }
    }
}



