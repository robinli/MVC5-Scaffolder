using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Category:Entity
    {
        public Category()
        {
            this.Products = new HashSet<Product>();
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = "产品目录")]
        [MaxLength(30)]
        public string Name { get; set; }
        [Display(Name = "产品信息")]
        public virtual ICollection<Product> Products { get; set; }
    }
}