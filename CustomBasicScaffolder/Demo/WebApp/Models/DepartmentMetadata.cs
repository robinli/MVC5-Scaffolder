using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department
    {
    }

    public partial class DepartmentMetadata
    {
        [Display(Name = "所在公司")]
        public Company Company { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 部门名称")]
        [Display(Name = "部门名称")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "部门经理")]
        [MaxLength(20)]
        public string Manager { get; set; }

        [Display(Name = "所在公司")]
        public int CompanyId { get; set; }

    }




	public class DepartmentChangeViewModel
    {
        public IEnumerable<Department> inserted { get; set; }
        public IEnumerable<Department> deleted { get; set; }
        public IEnumerable<Department> updated { get; set; }
    }

}
