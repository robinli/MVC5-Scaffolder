             
           
 
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
using WebApp.App_Start;
using Z.EntityFramework.Plus;
namespace WebApp.Services
{
    public class NotificationService : Service< Notification >, INotificationService
    {

        private readonly IRepositoryAsync<Notification> _repository;
		 private readonly IDataTableImportMappingService _mappingservice;
        public  NotificationService(IRepositoryAsync< Notification> repository,IDataTableImportMappingService mappingservice)
            : base(repository)
        {
            _repository=repository;
			_mappingservice = mappingservice;
        }
        
                  
        

		public void ImportDataTable(System.Data.DataTable datatable)
        {
            foreach (DataRow row in datatable.Rows)
            {
                 
                Notification item = new Notification();
				var mapping = _mappingservice.Queryable().Where(x => x.EntitySetName == "Notification").ToList();

                foreach (var field in mapping)
                {
                 
						var defval = field.DefaultValue;
						var contation = datatable.Columns.Contains((field.SourceFieldName == null ? "" : field.SourceFieldName));
						if (contation && row[field.SourceFieldName] != DBNull.Value)
						{
							Type notificationtype = item.GetType();
							PropertyInfo propertyInfo = notificationtype.GetProperty(field.FieldName);
							propertyInfo.SetValue(item, Convert.ChangeType(row[field.SourceFieldName], propertyInfo.PropertyType), null);
						}
						else if (!string.IsNullOrEmpty(defval))
						{
							Type notificationtype = item.GetType();
							PropertyInfo propertyInfo = notificationtype.GetProperty(field.FieldName);
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
                                   var notifications  = this.Query(new NotificationQuery().Withfilter(filters)).OrderBy(n=>n.OrderBy(sort,order)).Select().ToList();
                        var datarows = notifications .Select(  n => new {  Id = n.Id , Title = n.Title , Content = n.Content , Link = n.Link , Read = n.Read , From = n.From , To = n.To , Created = n.Created , Creator = n.Creator }).ToList();
           
            return ExcelHelper.ExportExcel(typeof(Notification), datarows);

        }

        public void AddConfirmRequirementMessage(string productNo, string customCode)
        {
            var item = new Notification();
            item.Title = $"新导入 {productNo} - {customCode} 需求.";
            item.Content = $"新导入 {productNo} - {customCode} 需求.请确认";
            item.Link = "/Requirements/Check";
            item.From = Auth.CurrentUserName;
            item.To = "ALL";
            item.Created = DateTime.Now;
            item.Creator = Auth.CurrentUserName;
            item.Group = 1;
            this.Insert(item);
        }

        


        public void ReadMessageWithGroup(int group) {
            this.Queryable().Where(x => x.Read == 0 && x.Group == group).Update(x => new Notification { Read = 1 });
        }

        public void AddNotify(string title, string message, string to,string from)
        {
            var item = new Notification()
            {
                Title = title,
                Content = message,
                To = to,
                From = from,
                Created = DateTime.Now,
                Read = 0,
                Group=0
            };
            this.Insert(item);
        }
    }
}



