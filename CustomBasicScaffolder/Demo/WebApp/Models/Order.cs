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
        public string Customer { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}