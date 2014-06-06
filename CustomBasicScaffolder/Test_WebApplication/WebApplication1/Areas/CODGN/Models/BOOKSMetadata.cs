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
        [MaxLength(12)]
        public string ID { get; set; }

        [Required(ErrorMessage = "Please enter : Book nmae")]
        [Display(Name = "Book nmae")]
        public string BOOKNAME { get; set; }

        [Display(Name = "Author")]
        public string AUTHOR { get; set; }

        [Display(Name = "Publish date")]
        public DateTime PUBLISH_UTC { get; set; }

        [Display(Name = "Version")]
        public int VERSION_NUM { get; set; }

        [Display(Name = "List price")]
        public decimal LIST_PRICE { get; set; }

    }
}
