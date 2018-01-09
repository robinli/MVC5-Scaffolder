             
           
 
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
    public class MessageService : Service< Message >, IMessageService
    {

        private readonly IRepositoryAsync<Message> _repository;
		 private readonly IDataTableImportMappingService _mappingservice;
        public  MessageService(IRepositoryAsync< Message> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository=repository;
			_mappingservice = mappingservice;
        }
        
                  
        

		public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                 
                Message item = new Message();
				var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "Message").ToList();

                foreach (var field in mapping)
                {
                 
						var defval = field.DefaultValue;
						var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));
						if (contation && row[field.SourceFieldName] != DBNull.Value)
						{
							Type messagetype = item.GetType();
							PropertyInfo propertyInfo = messagetype.GetProperty(field.FieldName);
							propertyInfo.SetValue(item, Convert.ChangeType(row[field.SourceFieldName], propertyInfo.PropertyType), null);
						}
						else if (!string.IsNullOrEmpty(defval))
						{
							Type messagetype = item.GetType();
							PropertyInfo propertyInfo = messagetype.GetProperty(field.FieldName);
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
                                   var messages  = this.Query(new MessageQuery().Withfilter(filters)).OrderBy(n=>n.OrderBy(sort,order)).Select().ToList();
                        var datarows = messages .Select(  n => new {  Id = n.Id , Group = n.Group , ExtensionKey1 = n.ExtensionKey1 , Type = n.Type , Code = n.Code , Content = n.Content , ExtensionKey2 = n.ExtensionKey2 , Tags = n.Tags , Method = n.Method , Created = n.Created , IsNew = n.IsNew , IsNotification = n.IsNotification }).ToList();
           
            return ExcelHelper.ExportExcel(typeof(Message), datarows);

        }
    }
}



