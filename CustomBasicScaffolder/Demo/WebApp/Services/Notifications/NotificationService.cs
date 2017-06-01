             
           
 
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

        public void AddConfirmProductionPlanMessage(string productNo, string customCode)
        {
            var item = new Notification();
            item.Title = $"新导入 {productNo} - {customCode} 生产计划.";
            item.Content = $"新导入 {productNo} - {customCode} 生产计划.请确认";
            item.Link = "/ProductionPlans/Check";
            item.From = Auth.CurrentUserName;
            item.To = "ALL";
            item.Created = DateTime.Now;
            item.Creator = Auth.CurrentUserName;
            item.Group = 2;
            this.Insert(item);
        }

        public void AddConfirmPurchaseOrderMessage(string po)
        {
            var item = new Notification();
            item.Title = $"新导入采购单.";
            item.Content = $"新导入 {po}  采购单.请确认";
            item.Link = "/PurchaseOrderHeaders/Index";
            item.From = Auth.CurrentUserName;
            item.To = "ALL";
            item.Created = DateTime.Now;
            item.Creator = Auth.CurrentUserName;
            item.Group = 3;
            this.Insert(item);
        }

        public void AddPublishPurchaseOrderMessage(string po)
        {
            var item = new Notification();
            item.Title = $"新发布采购单.";
            item.Content = $"采购单 {po}  已发布.请确认";
            item.Link = "/DeliveryDetails/Promise";
            item.From = Auth.CurrentUserName;
            item.To = "ALL";
            item.Created = DateTime.Now;
            item.Creator = Auth.CurrentUserName;
            item.Group = 4;
            this.Insert(item);
        }

        public void AddPromisedPurchaseOrderMessage(string po, int linenumber)
        {
            var item = new Notification();
            item.Title = $"新承诺采购单.";
            item.Content = $"采购单 {po}-{linenumber}  已承诺.请确认";
            item.Link = "/DeliveryDetails/Confirm";
            item.From = Auth.CurrentUserName;
            item.To = "ALL";
            item.Created = DateTime.Now;
            item.Creator = Auth.CurrentUserName;
            item.Group = 5;
            this.Insert(item);
        }
        public void AddConfirmPurchaseOrderMessage(string po, int linenumber)
        {
            var item = new Notification();
            item.Title = $"新确认采购单.";
            item.Content = $"采购单 {po}-{linenumber}  已承诺.请确认";
            item.Link = "/DeliveryDetails/Trace";
            item.From = Auth.CurrentUserName;
            item.To = "ALL";
            item.Created = DateTime.Now;
            item.Creator = Auth.CurrentUserName;
            item.Group = 6;
            this.Insert(item);
        }

        public void AddShippedPurchaseOrderMessage(string po, string billNo, string vehicle)
        {
            var item = new Notification();
            item.Title = $"新发货采购单.";
            item.Content = $"采购单 {po}-{billNo}-{vehicle}  已发货.请确认";
            item.Link = "/DeliveryDetails/Expect";
            item.From = Auth.CurrentUserName;
            item.To = "ALL";
            item.Created = DateTime.Now;
            item.Creator = Auth.CurrentUserName;
            item.Group = 7;
            this.Insert(item);
        }

        public void AddDelivereddPurchaseOrderMessage(string po, string billNo, string vehicle)
        {
            var item = new Notification();
            item.Title = $"新到货采购单.";
            item.Content = $"采购单 {po}-{billNo}-{vehicle}  已到达.请确认";
            item.Link = "/DeliveryDetails/Receiving";
            item.From = Auth.CurrentUserName;
            item.To = "ALL";
            item.Created = DateTime.Now;
            item.Creator = Auth.CurrentUserName;
            item.Group = 8;
            this.Insert(item);
        }

        public void AddReceivedPurchaseOrderMessage(string po, string billNo, string vehicle)
        {
            var item = new Notification();
            item.Title = $"新收货完成采购单.";
            item.Content = $"采购单 {po}-{billNo}-{vehicle}  收货完成.请确认";
            item.Link = "/PurchaseOrderHeaders/Index";
            item.From = Auth.CurrentUserName;
            item.To = "ALL";
            item.Created = DateTime.Now;
            item.Creator = Auth.CurrentUserName;
            item.Group = 9;
            this.Insert(item);
        }


        public void ReadMessageWithGroup(int group) {
            this.Queryable().Where(x => x.Read == 0 && x.Group == group).Update(x => new Notification { Read = 1 });
        }

    }
}



