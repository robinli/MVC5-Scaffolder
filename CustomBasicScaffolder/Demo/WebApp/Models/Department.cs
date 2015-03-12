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
}