using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
    }

    public partial class CategoryMetadata
    {
        [Display(Name = "产品")]
        public Product Products { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "分类")]
        [Required(ErrorMessage = "必填")]
        [MaxLength(30)]
        public string Name { get; set; }

    }
}
