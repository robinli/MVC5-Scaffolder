using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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

        [Required(ErrorMessage = "Please enter : 产品目录")]
        [Display(Name = "产品目录")]
        [MaxLength(30)]
        public string Name { get; set; }

    }




	public class CategoryChangeViewModel
    {
        public IEnumerable<Category> inserted { get; set; }
        public IEnumerable<Category> deleted { get; set; }
        public IEnumerable<Category> updated { get; set; }
    }

}
