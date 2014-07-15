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
        [Required(ErrorMessage = "Please enter : ID")]
        [Display(Name = "ID")]
        public string ID { get; set; }

        [Display(Name = "BOOKNAME")]
        public string BOOKNAME { get; set; }

        [Display(Name = "AUTHOR")]
        public string AUTHOR { get; set; }

        [Display(Name = "PUBLISH_UTC")]
        public DateTime PUBLISH_UTC { get; set; }

        [Display(Name = "VERSION_NUM")]
        public int VERSION_NUM { get; set; }

        [Display(Name = "VERSION_NUM2")]
        public long VERSION_NUM2 { get; set; }

        [Display(Name = "LIST_PRICE")]
        public decimal LIST_PRICE { get; set; }

        [Display(Name = "ISOK")]
        public bool ISOK { get; set; }

    }
}
