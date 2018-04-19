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
    public class CodeItemService : Service< CodeItem >, ICodeItemService
    {

        private readonly IRepositoryAsync<CodeItem> _repository;
		 private readonly IDataTableImportMappingService _mappingservice;
        public  CodeItemService(IRepositoryAsync< CodeItem> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository=repository;
			_mappingservice = mappingservice;
        }
        
                 
                   
        

		public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                 
                CodeItem item = new CodeItem();
				var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "CodeItem" &&  x.IsEnabled==true).ToList();

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
                       			 
            var codeitems  = this.Query(new CodeItemQuery().Withfilter(filters)).OrderBy(n=>n.OrderBy(sort,order)).Select().ToList();
            
                        var datarows = codeitems .Select(  n => new {  Id = n.Id , Code = n.Code , Text = n.Text , Description = n.Description , IsDisabled = n.IsDisabled  }).ToList();
           
            return ExcelHelper.ExportExcel(typeof(CodeItem), datarows);

        }

        public void UpdateJavascript(string filename )
        {
            StringWriter sw = new StringWriter();
            var list = GetCodeItems();
            foreach (var item in list.Select(x => new { CodeType = x.CodeType, Description = x.Description }).Distinct()
                )
            {
                sw.WriteLine($"//-------{item.Description}---------//");
                var filtername = item.CodeType.ToLower() + "filtersource";
                var datasourcename = item.CodeType.ToLower() + "datasource";
                var codetype = item.CodeType.ToLower();
                var description = item.Description;
                sw.WriteLine($"var {filtername} = [{{ value: '', text: 'All'}}];");
                sw.WriteLine($"var {datasourcename} = [];");
                foreach (var data in list.Where(x => x.CodeType == item.CodeType))
                {
                    sw.WriteLine($"{filtername}.push({{ value: '{data.Code}',text:'{data.Text}'  }});");
                    sw.WriteLine($"{datasourcename}.push({{ value: '{data.Code}',text:'{data.Text}'  }});");
                }
                sw.WriteLine($"//for datagrid {item.CodeType} field  formatter");
                sw.WriteLine($"function {item.CodeType.ToLower()}formatter(value, row, index) {{ ");
                sw.WriteLine($"     if (value === null || value === '' || value === undefined) ");
                sw.WriteLine($"     {{ ");
                sw.WriteLine($"         return \"\";");
                sw.WriteLine($"     }} ");
                sw.WriteLine($"     for (var i = 0; i < {datasourcename}.length; i++) {{");
                sw.WriteLine($"      var item = {datasourcename}[i];");
                sw.WriteLine($"     if (item.value === value.toString())");
                sw.WriteLine($"     {{");
                sw.WriteLine($"         return item.text;");
                sw.WriteLine($"     }}");
                sw.WriteLine($"     }};");
                sw.WriteLine($" return value;");
                sw.WriteLine($" }} ");

                sw.WriteLine($"//for datagrid   {item.CodeType}  field filter ");
                sw.WriteLine($"$.extend($.fn.datagrid.defaults.filters, {{");
                sw.WriteLine($"{item.CodeType.ToLower() }filter: {{");
                sw.WriteLine($"     init: function(container, options) {{");
                sw.WriteLine($"        var input = $('<input type=\"text\">').appendTo(container);");
                sw.WriteLine($"        var myoptions = {{");
                sw.WriteLine($"             panelHeight: \"auto\",");
                sw.WriteLine($"             data: {filtername}");
                sw.WriteLine($"         }}");
                sw.WriteLine($"         $.extend(options, myoptions);");
                sw.WriteLine($"         input.combobox(options);");
                sw.WriteLine($"         return input;");
                sw.WriteLine($"      }},");
                sw.WriteLine($"     destroy: function(target) {{");
                sw.WriteLine($"         $(target).combobox('destroy');");
                sw.WriteLine($"     }},");
                sw.WriteLine($"     getValue: function(target) {{");
                sw.WriteLine($"         return $(target).combobox('getValue');");
                sw.WriteLine($"     }},");
                sw.WriteLine($"     setValue: function(target, value) {{");
                sw.WriteLine($"         $(target).combobox('setValue', value);");
                sw.WriteLine($"     }},");
                sw.WriteLine($"     resize: function(target, width) {{");
                sw.WriteLine($"         $(target).combobox('resize', width);");
                sw.WriteLine($"     }}");
                sw.WriteLine($"   }}");
                sw.WriteLine($"}});");

                sw.WriteLine($"//for datagrid   { item.CodeType }   field  editor ");
                sw.WriteLine($"$.extend($.fn.datagrid.defaults.editors, {{");
                sw.WriteLine($"{item.CodeType.ToLower()}editor: {{");
                sw.WriteLine($"     init: function(container, options) {{");
                sw.WriteLine($"        var input = $('<input type=\"text\">').appendTo(container);");
                sw.WriteLine($"        var myoptions = {{");
                sw.WriteLine($"         panelHeight: \"auto\",");
                sw.WriteLine($"         data: {datasourcename},");
                sw.WriteLine($"         valueField: 'value',");
                sw.WriteLine($"         textField: 'text'");
                sw.WriteLine($"     }}");
                sw.WriteLine($"    $.extend(options, myoptions);");
                sw.WriteLine($"           input.combobox(options);");
                sw.WriteLine($"           return input;");
                sw.WriteLine($"       }},");
                sw.WriteLine($"     destroy: function(target) {{");
                sw.WriteLine($"         $(target).combobox('destroy');");
                sw.WriteLine($"        }},");
                sw.WriteLine($"     getValue: function(target) {{");
                sw.WriteLine($"        return $(target).combobox('getValue');");
                sw.WriteLine($"        }},");
                sw.WriteLine($"     setValue: function(target, value) {{");
                sw.WriteLine($"         $(target).combobox('setValue', value);");
                sw.WriteLine($"         }},");
                sw.WriteLine($"     resize: function(target, width) {{");
                sw.WriteLine($"         $(target).combobox('resize', width);");
                sw.WriteLine($"        }}");
                sw.WriteLine($"  }}  ");
                sw.WriteLine($"}});");


            }

            File.WriteAllText(filename, sw.ToString());
        }
        private IEnumerable<CodeItem> GetCodeItems() {
            var result = this.Queryable().Where(x => x.IsDisabled == 0).OrderBy(x=>x.CodeType).ThenBy(x=>x.Code).ToList();
            return result;

        }
    }
}



