using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    [MetadataType(typeof(BaseCodeMetadata))]
    public partial class BaseCode
    {
    }

    public partial class BaseCodeMetadata
    {
        [Display(Name = "CodeItems")]
        public CodeItem CodeItems { get; set; }

        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter : 代码类")]
        [Display(Name = "代码类")]
        [MaxLength(30)]
        public string CodeType { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }

    }




	public class BaseCodeChangeViewModel
    {
        public IEnumerable<BaseCode> inserted { get; set; }
        public IEnumerable<BaseCode> deleted { get; set; }
        public IEnumerable<BaseCode> updated { get; set; }
    }

}
