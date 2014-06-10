using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    [MetadataType(typeof(GRP_BS_CUSTGRADEMetadata))]
    public partial class GRP_BS_CUSTGRADE
    {
    }

    public partial class GRP_BS_CUSTGRADEMetadata
    {
        [Required(ErrorMessage = "Please enter : 客戶級別")]
        [Display(Name = "客戶級別")]
        [MaxLength(1)]
        public string GRADECO { get; set; }

        [Display(Name = "級別名稱")]
        [MaxLength(8)]
        public string GRADENA { get; set; }

        [Display(Name = "折扣金額")]
        [Range(0, 1000)]
        public int DISCOUNT_PRICE { get; set; }

    }
}
