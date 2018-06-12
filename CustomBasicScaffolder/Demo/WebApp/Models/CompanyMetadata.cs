using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    [MetadataType(typeof(CompanyMetadata))]
    public partial class Company
    {
    }

    public partial class CompanyMetadata
    {
        [Display(Name = "部门")]
        public Department Departments { get; set; }

        [Display(Name = "员工")]
        public Employee Employee { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "地址")]
        [MaxLength(60)]
        public string Address { get; set; }

        [Display(Name = "城市")]
        public string City { get; set; }

        [Display(Name = "省份")]
        public string Province { get; set; }

        [Display(Name = "注册日期")]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "员工数")]
        public int Employees { get; set; }

    }




	public class CompanyChangeViewModel
    {
        public IEnumerable<Company> inserted { get; set; }
        public IEnumerable<Company> deleted { get; set; }
        public IEnumerable<Company> updated { get; set; }
    }

}
