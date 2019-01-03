// <copyright file="MenuItemService.cs" company="neozhu/SmartCode-Scaffolder">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>neo.zhu</author>
// <date>9/26/2018 4:20:25 PM </date>
// <summary>
//  根据需求定义实现具体的业务逻辑,通过依赖注入降低模块之间的耦合度
//   
//  
//  
// </summary>
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
    public class MenuItemService : Service<MenuItem>, IMenuItemService
    {
        private readonly IRepositoryAsync<MenuItem> _repository;
        private readonly IDataTableImportMappingService _mappingservice;
        public MenuItemService(IRepositoryAsync<MenuItem> repository, IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository = repository;
            _mappingservice = mappingservice;
        }

        public IEnumerable<MenuItem> GetByParentId(int parentid)
        {
            return _repository.GetByParentId(parentid);
        }
        public IEnumerable<MenuItem> GetSubMenusByParentId(int parentid)
        {
            return _repository.GetSubMenusByParentId(parentid);
        }



        private int getParentIdByTitle(string title)
        {
            var menuitemRepository = this._repository.GetRepository<MenuItem>();
            var menuitem = menuitemRepository.Queryable().Where(x => x.Title == title).FirstOrDefault();
            if (menuitem == null)
            {
                throw new Exception("not found ForeignKey:ParentId with " + title);
            }
            else
            {
                return menuitem.Id;
            }
        }

        public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                var item = new MenuItem();
                var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "MenuItem" && ((x.IsEnabled == true) || (x.IsEnabled == false && !(x.DefaultValue == null || x.DefaultValue.Equals(string.Empty))))).ToList();
                var requiredfield = mapping.Where(x => x.IsRequired == true).FirstOrDefault()?.SourceFieldName;
                if (requiredfield != null && !row.IsNull(requiredfield) && row[requiredfield] != DBNull.Value && Convert.ToString(row[requiredfield]).Trim() != string.Empty)
                {
                    foreach (var field in mapping)
                    {
                        var defval = field.DefaultValue;
                        var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));
                        if (contation && !row.IsNull(field.SourceFieldName) && row[field.SourceFieldName] != DBNull.Value && row[field.SourceFieldName].ToString() != string.Empty)
                        {
                            var menuitemtype = item.GetType();
                            var propertyInfo = menuitemtype.GetProperty(field.FieldName);
                            //关联外键查询获取Id
                            switch (field.FieldName)
                            {
                                case "ParentId":
                                    var title = row[field.SourceFieldName].ToString();
                                    var parentid = getParentIdByTitle(title);
                                    propertyInfo.SetValue(item, Convert.ChangeType(parentid, propertyInfo.PropertyType), null);
                                    break;
                                default:
                                    var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                                    var safeValue = (row[field.SourceFieldName] == null) ? null : Convert.ChangeType(row[field.SourceFieldName], safetype);
                                    propertyInfo.SetValue(item, safeValue, null);
                                    break;
                            }
                        }
                        else if (!string.IsNullOrEmpty(defval))
                        {
                            var menuitemtype = item.GetType();
                            PropertyInfo propertyInfo = menuitemtype.GetProperty(field.FieldName);
                            if (defval.ToLower() == "now" && propertyInfo.PropertyType == typeof(DateTime))
                            {
                                propertyInfo.SetValue(item, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
                            }
                            else
                            {
                                var safetype = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                                var safeValue = Convert.ChangeType(defval, safetype);
                                propertyInfo.SetValue(item, safeValue, null);
                            }
                        }
                    }
                    Insert(item);
                }

            }
        }

        public Stream ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);

            var menuitems = Query(new MenuItemQuery().Withfilter(filters)).Include(p => p.Parent).OrderBy(n => n.OrderBy(sort, order)).Select().ToList();

            var datarows = menuitems.Select(n => new
            {

                ParentTitle = (n.Parent == null ? "" : n.Parent.Title),
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                Code = n.Code,
                Url = n.Url,
                Controller = n.Controller,
                Action = n.Action,
                IconCls = n.IconCls,
                IsEnabled = n.IsEnabled,
                ParentId = n.ParentId
            }).ToList();

            return ExcelHelper.ExportExcel(typeof(MenuItem), datarows);

        }

        public IEnumerable<MenuItem> CreateWithController()
        {
            List<MenuItem> list = new List<MenuItem>();

            Assembly asm = Assembly.GetExecutingAssembly();

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            int i = 1000;
            string[] actions = new string[] { "Index" };
            foreach (var item in controlleractionlist.Where(x => actions.Contains(x.Action)))
            {
                MenuItem menu = new MenuItem();
                menu.Code = (i++).ToString("0000");
                menu.Description = "";
                menu.Title = item.Controller.Replace("Controller", "");
                menu.Url = "/" + item.Controller.Replace("Controller", "") + "/" + item.Action;
                menu.Action = item.Action;
                menu.Controller = item.Controller.Replace("Controller", "");
                menu.IsEnabled = true;
                if (!this.Queryable().Where(x => x.Url == menu.Url).Any())
                {
                    this.Insert(menu);

                }

                list.Add(menu);
            }

            return list;
        }


        public IEnumerable<MenuItem> ReBuildMenus()
        {
            foreach (var item in this.Queryable().ToList())
            {
                this.Delete(item);
            }
            return this.CreateWithController();
        }
    }
}



