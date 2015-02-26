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
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [Required(ErrorMessage="必填")]
        [Range(1,9999)]
        public int Qty { get; set; }
        [Required(ErrorMessage = "必填")]
        [Range(1, 9999)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "必填")]
        [Range(1, 9999)]
        public decimal Amount { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

    }
}