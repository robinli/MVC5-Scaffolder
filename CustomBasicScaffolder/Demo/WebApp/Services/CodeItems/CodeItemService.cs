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
    public class CodeItemService : Service<CodeItem>, ICodeItemService
    {

        private readonly IRepositoryAsync<CodeItem> _repository;
        private readonly IRepositoryAsync<BaseCode> _baseCodeRepository;
        private readonly IDataTableImportMappingService _mappingservice;
        public CodeItemService(IRepositoryAsync<BaseCode> _baseCodeRepository,IRepositoryAsync<CodeItem> repository, IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository = repository;
            _mappingservice = mappingservice;
            this._baseCodeRepository = _baseCodeRepository;
        }

        public IEnumerable<CodeItem> GetByBaseCodeId(int basecodeid)
        {
            return _repository.GetByBaseCodeId(basecodeid);
        }



        public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {

                CodeItem item = new CodeItem();
                var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "CodeItem" && x.IsEnabled==true).ToList();
                foreach (var field in mapping)
                {

                    var defval = field.DefaultValue;
                    var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));

                    if (contation && row[field.SourceFieldName] != DBNull.Value)
                    {
                        Type codeitemtype = item.GetType();
                        PropertyInfo propertyInfo = codeitemtype.GetProperty(field.FieldName);
                        propertyInfo.SetValue(item, Convert.ChangeType(row[field.SourceFieldName], propertyInfo.PropertyType), null);
                    }
                    else if (!string.IsNullOrEmpty(defval))
                    {
                        Type codeitemtype = item.GetType();
                        PropertyInfo propertyInfo = codeitemtype.GetProperty(field.FieldName);
                        if (defval.ToLower() == "now" && propertyInfo.PropertyType == typeof(DateTime))
                        {
                            propertyInfo.SetValue(item, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(item, Convert.ChangeType(defval, propertyInfo.PropertyType), null);
                        }
                    }



                    if (field.SourceFieldName == "类型") {
                        var codeType = row[field.SourceFieldName].ToString();
                        var baseCode = _baseCodeRepository.Queryable().Where(x => x.CodeType == codeType).FirstOrDefault();
                        if (baseCode == null) {
                            baseCode = new BaseCode();
                            baseCode.CodeType = codeType;
                            baseCode.Description = codeType;
                            _baseCodeRepository.Insert(baseCode);
                        }
                        else
                        {
                            item.BaseCodeId = baseCode.Id;

                        }
                    }
                }


                this.Insert(item);


            }
        }

        public Stream ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);

            var codeitems = this.Query(new CodeItemQuery().Withfilter(filters)).Include(p => p.BaseCode).OrderBy(n => n.OrderBy(sort, order)).Select().ToList();

            var datarows = codeitems.Select(n => new { BaseCodeCodeType = (n.BaseCode == null ? "" : n.BaseCode.CodeType), Id = n.Id, Code = n.Code, Text = n.Text, BaseCodeId = n.BaseCodeId }).ToList();

            return ExcelHelper.ExportExcel(typeof(CodeItem), datarows);

        }
    }
}



