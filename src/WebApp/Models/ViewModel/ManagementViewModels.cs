
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

 
namespace WebApp.Models
{
    public class RoleViewModel
    {
        [Required]
        [DisplayName("角色")]
        public string Name { get; set; }
    }
    public class ManagementViewModel
    {
        public ICollection<ApplicationRole> Roles { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
    public class AccountRecordViewModel
    {
        public ApplicationUser User { get; set; }
        public ICollection<ApplicationRole> Roles { get; set; }
    }
    public class AttachRoleViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
    public class AccountViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "密码长度(6-100)。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Role { get; set; }
    }
}