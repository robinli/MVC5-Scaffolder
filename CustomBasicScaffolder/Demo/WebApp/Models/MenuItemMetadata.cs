
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    [MetadataType(typeof(MenuItemMetadata))]
    public partial class MenuItem
    {
    }

    public partial class MenuItemMetadata
    {
        [Display(Name = "上级菜单")]
        public MenuItem Parent { get; set; }

        [Display(Name = "子菜单")]
        public MenuItem SubMenus { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 菜单名称")]
        [Display(Name = "菜单名称")]
        [MaxLength(20)]
        public string Title { get; set; }

        [Display(Name = "描述")]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter : 代码")]
        [Display(Name = "代码")]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please enter : Url")]
        [Display(Name = "Url")]
        [MaxLength(100)]
        public string Url { get; set; }

        [Display(Name = "上级菜单")]
        public int ParentId { get; set; }
        [Display(Name = "是否启用")]
        public bool IsEnabled { get; set; }

    }
}
