using Repository.Pattern.Ef6;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    [MetadataType(typeof(CompanyMetadata))]
    public partial class Company:Entity
    {
    }

    public partial class CompanyMetadata
    {
        [Display(Name = "部门")]
        public Department Departments { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 公司名称")]
        [Display(Name = "公司名称")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter : 地址")]
        [Display(Name = "地址")]
        [MaxLength(50)]
        public string Address { get; set; }

        [Display(Name = "城市")]
        [MaxLength(20)]
        public string City { get; set; }

        [Display(Name = "省份")]
        [MaxLength(20)]
        public string Province { get; set; }

        [Required(ErrorMessage = "Please enter : 注册日期")]
        [Display(Name = "注册日期")]
        public DateTime RegisterDate { get; set; }

        [Required(ErrorMessage = "Please enter : 员工人数")]
        [Display(Name = "员工人数")]
        [Range(1, 9999)]
        public int Employees { get; set; }

    }
}
