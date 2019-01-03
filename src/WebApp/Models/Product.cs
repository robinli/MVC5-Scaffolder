using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Product:Entity
    {
        
        [Key]
        [Display(Name = "Id", Description ="Key", ShortName = "Id", AutoGenerateField = false, AutoGenerateFilter = false, Order = 1, Prompt = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 名称")]
        [Display(Name = "名称", Description = "名称", ShortName = "名称", AutoGenerateField = true, AutoGenerateFilter = true, Order = 3, Prompt = "名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "单位", Description = "单位", ShortName = "单位", AutoGenerateField = true, AutoGenerateFilter = true, Order = 4, Prompt = "单位")]
        [MaxLength(10)]
        public string Unit { get; set; }

        [Display(Name = "单价", Description = "单价",  ShortName = "单价", AutoGenerateField = true, AutoGenerateFilter = true, Order = 5, Prompt = "单价")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "库存", Description = "库存", ShortName = "库存", AutoGenerateField = true, AutoGenerateFilter = true, Order = 6, Prompt = "库存")]
        public int StockQty { get; set; }
        [Display(Name = "是否需要QC", Description = "是否需要QC", ShortName = "QC", AutoGenerateField = true, AutoGenerateFilter = true, Order = 7, Prompt = "是否需要QC")]
        public bool IsRequiredQc { get; set; }

        [Display(Name = "确认日期", Description = "确认日期", ShortName = "确认日期", AutoGenerateField = true, AutoGenerateFilter = true, Order = 8, Prompt = "确认日期")]
        public DateTime ConfirmDateTime { get; set; }

        [Required(ErrorMessage = "Please enter : 产品类别")]
        [Display(Name = "产品类别", Description = "产品类别", Order = 2)]
        public int CategoryId { get; set; }
        [Display(Name = "产品类别")]
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}