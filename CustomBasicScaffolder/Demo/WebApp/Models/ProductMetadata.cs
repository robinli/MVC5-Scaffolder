using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
    }

    public partial class ProductMetadata
    {
        [Required(ErrorMessage = "Please enter : Category")]
        [Display(Name = "分类")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "单位")]
        [MaxLength(3)]
        public string Unit { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "单价")]
        [Range(0, 99999)]
        public decimal UnitPrice { get; set; }

        [Display(Name = "库存")]
        [Range(0, 99999)]
        public int StockQty { get; set; }

        [Display(Name = "确认日期")]
        public DateTime ConfirmDateTime { get; set; }

        [Required(ErrorMessage = "Please enter : CategoryId")]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

    }
}
