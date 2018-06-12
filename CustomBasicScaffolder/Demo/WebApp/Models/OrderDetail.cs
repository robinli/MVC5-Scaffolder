using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class OrderDetail:Entity
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "必选")]
        [Display(Name ="商品", Description ="商品",Order =2)]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [Display(Name = "商品", Description = "商品", Order = 3)]
        public Product Product { get; set; }
        [Required(ErrorMessage="必填")]
        [Range(1,9999)]
        [Display(Name = "数量", Description = "需求数量", Order = 4)]
        public int Qty { get; set; }
        [Required(ErrorMessage = "必填")]
        [Range(1, 9999)]
        [Display(Name = "单价", Description = "单价", Order = 5)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "必填")]
        [Range(1, 9999)]
        [Display(Name = "金额", Description = "金额(数量x单价)", Order = 6)]
        public decimal Amount { get; set; }
        [Display(Name = "订单号", Description = "订单号", Order = 1)]
        public int OrderId { get; set; }
        //关联订单表头
        [ForeignKey("OrderId")]
        [Display(Name = "订单号", Description = "订单号", Order = 1)]
        public Order Order { get; set; }

    }
}