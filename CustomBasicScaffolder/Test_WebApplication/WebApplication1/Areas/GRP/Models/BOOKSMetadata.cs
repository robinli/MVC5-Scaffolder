using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(BOOKSMetadata))]
    public partial class BOOKS
    {
    }

    public partial class BOOKSMetadata
    {
        [Required(ErrorMessage = "Please enter : ISBN")]
        [Display(Name = "ISBN")]
        
        public string ID { get; set; }

        [Required(ErrorMessage = "Please enter : 書名")]
        [Display(Name = "書名")]
        
        public string BOOKNAME { get; set; }

        [Display(Name = "AUTHOR")]
        
        public string AUTHOR { get; set; }

        [Display(Name = "PUBLISH_UTC")]
        
        public DateTime PUBLISH_UTC { get; set; }

        [Display(Name = "VERSION_NUM")]
        
        public int VERSION_NUM { get; set; }

        [Display(Name = "LIST_PRICE")]
        
        public decimal LIST_PRICE { get; set; }

    }
}
