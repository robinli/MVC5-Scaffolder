using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    [MetadataType(typeof(WorkMetadata))]
    public partial class Work
    {
    }

    public partial class WorkMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 任务")]
        [Display(Name = "任务")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter : 状态")]
        [Display(Name = "状态")]
        [UIHint("BaseCode")]
        [MaxLength(10)]
        public string Status { get; set; }

        [Required(ErrorMessage = "Please enter : 开始日期")]
        [Display(Name = "开始日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束日期")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please enter : 是否启用")]
        [Display(Name = "是否启用")]
        public bool Enableed { get; set; }

        [Required(ErrorMessage = "Please enter : 用时(H)")]
        [Display(Name = "用时(H)")]
        [Range(1, 99)]
        public int Hour { get; set; }

        [Required(ErrorMessage = "Please enter : 优先级")]
        [Display(Name = "优先级")]
        [Range(1, 9)]
        public int Priority { get; set; }

        [Required(ErrorMessage = "Please enter : 分数")]
        [Display(Name = "分数")]
        [Range(1, 100)]
        public decimal Score { get; set; }

    }
}
