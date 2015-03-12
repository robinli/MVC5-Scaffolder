using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Company
    {
        public Company()
        {
            Departments = new HashSet<Department>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address {get;set;}
        public string City { get; set; }
        public string Province { get; set; }
        public DateTime RegisterDate { get; set; }
        public int Employees { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

    }
}