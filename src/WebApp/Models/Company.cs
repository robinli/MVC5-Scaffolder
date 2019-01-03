using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Company : Entity
    {
        public Company()
        {
            Departments = new HashSet<Department>();
            Employees = new HashSet<Employee>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "名称",Description = "名称")]
        [MaxLength(50)]
        [Required]
        [Index(IsUnique =true)]
        public string Name { get; set; }
        [Display(Name = "组织代码", Description = "组织代码")]
        [MaxLength(12)]
        [Index(IsUnique = true)]
        [Required]
        public string Code { get; set; }
        [Display(Name = "地址", Description = "地址")]
        [MaxLength(50)]
        [DefaultValue("-")]
        public string Address {get;set;}
        [Display(Name = "联系人", Description = "联系人")]
        [MaxLength(12)]
        public string Contect { get; set; }
        [Display(Name = "联系电话", Description = "联系电话")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [Display(Name = "注册日期", Description = "注册日期")]
        [DefaultValue("now")]
        public DateTime RegisterDate { get; set; }
        [Display(Name = "雇员人数", Description = "雇员人数")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int EmployeeNumber { get => this.Employees.Count; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }


    public class CompanyViewModel
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int Type { get; set; }
    }
}