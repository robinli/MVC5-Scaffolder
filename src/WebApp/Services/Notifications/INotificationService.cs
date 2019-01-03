

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

        void AddNotify(string title, string message, string to, string from);

        void ReadMessageWithGroup(int group);
    }
}