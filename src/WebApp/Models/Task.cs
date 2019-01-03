using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Work : Entity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "任务", Description = "任务")]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "状态", Description = "状态")]
        [MaxLength(10)]
        public string Status { get; set; }
        [System.ComponentModel.DefaultValue("NOW")]
        [Display(Name="开始日期",Description = "开始日期")]
        public DateTime StartDate { get; set; }
        [System.ComponentModel.DefaultValue(null)]
        [Display(Name = "结束日期", Description = "结束日期")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "开工时间", Description = "开工时间")]
        public DateTime? ToDoDateTime { get; set; }
 
        //AddColumn("dbo.Events", "Active", c => c.Boolean(nullable: false, defaultValue: true));
        [Display(Name = "是否启用", Description = "是否启用")]
        [System.ComponentModel.DefaultValue(true)]
        public bool Enableed { get; set; }
        [Display(Name = "是否完成", Description = "是否完成")]
        [DefaultValue(false)]
        public bool? Completed { get; set; }
        [System.ComponentModel.DefaultValue(8)]
        [Display(Name = "工时", Description = "工时")]
           public int Hour { get; set; }
        [System.ComponentModel.DefaultValue(1)]
        [Display(Name = "优先级", Description = "优先级")]
        public int Priority { get; set; }
        [System.ComponentModel.DefaultValue(100)]
        [Display(Name = "打分", Description = "打分")]
        public decimal Score { get; set; }
        

        
    }
    
}