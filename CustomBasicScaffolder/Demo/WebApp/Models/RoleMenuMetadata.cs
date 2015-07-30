﻿
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    [MetadataType(typeof(RoleMenuMetadata))]
    public partial class RoleMenu
    {
    }

    public partial class RoleMenuMetadata
    {
        [Display(Name = "授权菜单")]
        public MenuItem MenuItem { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 角色|用户")]
        [Display(Name = "角色|用户")]
        [MaxLength(20)]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Please enter : 授权菜单")]
        [Display(Name = "授权菜单")]
        public int MenuId { get; set; }

        [Required(ErrorMessage = "Please enter : 是否启用")]
        [Display(Name = "是否启用")]
        public bool IsEnabled { get; set; }

    }
}
