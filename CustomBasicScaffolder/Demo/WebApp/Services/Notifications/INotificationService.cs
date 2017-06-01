

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
using System.IO;

namespace WebApp.Services
{
    public interface INotificationService : IService<Notification>
    {




        void ImportDataTable(DataTable datatable);
        Stream ExportExcel(string filterRules = "", string sort = "Id", string order = "asc");
        /// <summary>
        /// 需求导入通知审核
        /// </summary>
        /// <param name="productNo"></param>
        /// <param name="customCode"></param>
        void AddConfirmRequirementMessage(string productNo, string customCode);
        /// <summary>
        /// 生产计划导入通知
        /// </summary>
        /// <param name="productNo"></param>
        /// <param name="customCode"></param>
        void AddConfirmProductionPlanMessage(string productNo, string customCode);
        /// <summary>
        /// 采购单导入通知
        /// </summary>
        /// <param name="po"></param>
        void AddConfirmPurchaseOrderMessage(string po);
        /// <summary>
        /// 采购单发布通知
        /// </summary>
        /// <param name="po"></param>
        void AddPublishPurchaseOrderMessage(string po);
        /// <summary>
        /// 采购单承诺通知
        /// </summary>
        /// <param name="po"></param>
        /// <param name="linenumber"></param>
        void AddPromisedPurchaseOrderMessage(string po,int linenumber);
        void AddConfirmPurchaseOrderMessage(string po, int linenumber);
        /// <summary>
        /// 采购单发货同志
        /// </summary>
        /// <param name="po"></param>
        /// <param name="billNo"></param>
        /// <param name="vehicle"></param>
        void AddShippedPurchaseOrderMessage(string po,  string billNo,string vehicle);
        /// <summary>
        /// 采购单到货通知
        /// </summary>
        /// <param name="po"></param>
        /// <param name="billNo"></param>
        /// <param name="vehicle"></param>
        void AddDelivereddPurchaseOrderMessage(string po,  string billNo, string vehicle);
        /// <summary>
        /// 采购单收货通知
        /// </summary>
        /// <param name="po"></param>
        /// <param name="billNo"></param>
        /// <param name="vehicle"></param>
        void AddReceivedPurchaseOrderMessage(string po,   string billNo, string vehicle);

        void ReadMessageWithGroup(int group);
    }
}