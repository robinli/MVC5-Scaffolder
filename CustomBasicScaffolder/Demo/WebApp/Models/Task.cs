using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Work : Entity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        [System.ComponentModel.DefaultValue("NOW")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [System.ComponentModel.DefaultValue(true)]
        public bool Enableed { get; set; }
        [System.ComponentModel.DefaultValue(8)]
        public int Hour { get; set; }
        [System.ComponentModel.DefaultValue(10)]
        public int Priority { get; set; }
        public decimal Score { get; set; }
        

        
    }
    
}