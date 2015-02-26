using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }

    public partial class OrderMetadata
    {
        [Display(Name = "订单明细")]
        public OrderDetail OrderDetails { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 客户")]
        [Display(Name = "客户")]
        [MaxLength(20)]
        public string Customer { get; set; }

        [Display(Name = "发货地址")]
        [MaxLength(50)]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Please enter : 订单日期")]
        [Display(Name = "订单日期")]
        public DateTime OrderDate { get; set; }

    }
}
