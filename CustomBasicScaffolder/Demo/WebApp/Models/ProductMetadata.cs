using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
    }

    public partial class ProductMetadata
    {
        [Display(Name = "产品类别")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 名称")]
        [Display(Name = "名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "单位")]
        public string Unit { get; set; }

        [Display(Name = "单价")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "库存")]
        public int StockQty { get; set; }

        [Display(Name = "确认日期")]
        public DateTime ConfirmDateTime { get; set; }

        [Required(ErrorMessage = "Please enter : 产品类别")]
        [Display(Name = "产品类别")]
        public int CategoryId { get; set; }

    }




	public class ProductChangeViewModel
    {
        public IEnumerable<Product> inserted { get; set; }
        public IEnumerable<Product> deleted { get; set; }
        public IEnumerable<Product> updated { get; set; }
    }

}
