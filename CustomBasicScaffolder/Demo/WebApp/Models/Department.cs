using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Department:Entity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        [Display(Name = "所在公司")]
        public int CompanyId { get; set; }
         [ForeignKey("CompanyId")]
         [Display(Name = "所在公司")]
        public Company Company { get;set;}
    }

    public partial class Employee:Entity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }
        [MaxLength(30)]
        public string Title { get; set; }
        [MaxLength(10)]
        [Required]
        public string Sex { get; set; }
        public int Age { get; set; }
        public DateTime Brithday { get; set; }
      
        public int IsDeleted { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

    }
}