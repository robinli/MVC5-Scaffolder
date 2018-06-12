using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    [MetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail
    {
    }

    public partial class OrderDetailMetadata
    {
        [Display(Name = "订单")]
        public Order Order { get; set; }

        [Display(Name = "产品")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "产品")]
        public int ProductId { get; set; }

        [Display(Name = "数量")]
        public int Qty { get; set; }

        [Display(Name = "单价")]
        public decimal Price { get; set; }

        [Display(Name = "金额")]
        public decimal Amount { get; set; }

        [Display(Name = "订单")]
        public int OrderId { get; set; }

    }




	public class OrderDetailChangeViewModel
    {
        public IEnumerable<OrderDetail> inserted { get; set; }
        public IEnumerable<OrderDetail> deleted { get; set; }
        public IEnumerable<OrderDetail> updated { get; set; }
    }

}
