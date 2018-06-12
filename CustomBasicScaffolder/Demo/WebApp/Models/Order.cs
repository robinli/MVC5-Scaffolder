using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Order:Entity
    {
        public Order() {
            OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="客户名称",Description ="订单所属的客户",Order =1)]
        [MaxLength(30)]
        public string Customer { get; set; }
        [Required]
        [Display(Name = "发货地址", Description = "发货地址", Order = 2)]
        [MaxLength(200)]
        public string ShippingAddress { get; set; }
        [Display(Name = "订单日期", Description = "订单日期默认当天", Order = 3)]
        public DateTime OrderDate { get; set; }
        //关联订单明细 1-*
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}