using System;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Please enter : 部门")]
        [Display(Name = "部门")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Display(Name = "经理")]
        [MaxLength(20)]
        public string Manager { get; set; }

        [Required(ErrorMessage = "Please enter : 所在公司")]
        [Display(Name = "所在公司")]
        public int CompanyId { get; set; }

    }
}
