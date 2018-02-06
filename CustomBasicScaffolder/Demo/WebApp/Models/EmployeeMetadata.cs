using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
   
    }

    public partial class EmployeeMetadata
    {
        [Display(Name = "所在公司")]
        public Company Company { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "职称")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "性别")]
        public string Sex { get; set; }

        [Display(Name = "年龄")]
        public int Age { get; set; }

        [Display(Name = "生日")]
        public DateTime Brithday { get; set; }

        [Display(Name = "是否已删除")]
        public int IsDeleted { get; set; }

        [Display(Name = "所在公司")]
        public int CompanyId { get; set; }

    }




	public class EmployeeChangeViewModel
    {
        public IEnumerable<Employee> inserted { get; set; }
        public IEnumerable<Employee> deleted { get; set; }
        public IEnumerable<Employee> updated { get; set; }
    }

}
