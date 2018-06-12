using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
    public enum MessageGroup {
        System,
        Operator,
        Intface
    }
    public enum MessageType {
        Information,
        Error,
        Alert
    }

    public partial class Message:Entity
    {
        public Message()
        {
            Created = DateTime.Now;
        }
        public int Id { get; set; }
        [Display(Name = "分组",Description ="系统/业务/接口")]
        public int Group { get; set; }
        [Display(Name = "关键字", Description = "关键字(提供查询)")]
        [MaxLength(50)]
        public string ExtensionKey1 { get; set; }
        [Display(Name = "类型", Description = "信息/异常/警告")]
        public int Type { get; set; }

        [Display(Name = "异常代码", Description = "异常代码")]
        [MaxLength(50)]
        public string Code { get; set; }
        [Display(Name = "内容", Description = "内容")]
  
        public string Content { get; set; }
        [Display(Name = "关键字2", Description = "关键字(提供查询)")]
        [MaxLength(50)]
        public string ExtensionKey2 { get; set; }
        [Display(Name = "标签", Description = "标签")]
        [MaxLength(255)]
        public string Tags { get; set; }
        [Display(Name = "堆栈轨迹", Description = "堆栈轨迹")]
        public string StackTrace { get; set; }
        [Display(Name = "调用方法", Description = "调用方法")]
        [MaxLength(255)]
        public string Method { get; set; }
        [Display(Name = "操作人", Description = "操作人")]
        [MaxLength(20)]
        public string User { get; set; }
        [Display(Name = "记录时间", Description = "记录时间")]
        public DateTime Created { get; set; }
        [Display(Name = "新消息", Description = "新消息")]
        public int IsNew { get; set; }
        [Display(Name = "是否已通知", Description = "是否已通知")]
        public int IsNotification { get; set; }


       

    }

}