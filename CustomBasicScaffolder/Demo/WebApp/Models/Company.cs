using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Company : Entity
    {
        public Company()
        {
            Departments = new HashSet<Department>();
            Employee = new HashSet<Employee>();
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
        public virtual ICollection<Employee> Employee { get; set; }
    }


    public class CompanyViewModel
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int Type { get; set; }
    }
}