using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class QueryBooks2_QueryFormViewModel
    {
        [Display(Name="queryBookName")]
        [MaxLength(3)]
        public string queryBookName { get; set; }

        public bool DoExport { get; set; }

        public List<QueryBooks2_Result> Result { get; set; }
    }

    [MetadataType(typeof(QueryBooks2_ResultMetadata))]
    public partial class QueryBooks2_Result
    {
    }

    public partial class QueryBooks2_ResultMetadata
    {
        [Display(Name = "ISBN")]
        public string ID { get; set; }

        [Display(Name = "BOOK NAME")]
        public string BOOKNAME { get; set; }

        [Display(Name = "AUTHOR name")]
        public string AUTHOR { get; set; }

        [Display(Name = "PUBLISH")]
        public System.DateTime? PUBLISH_UTC { get; set; }

        [Display(Name = "VERSION")]
        public int? VERSION_NUM { get; set; }

        [Display(Name = "PRICE")]
        public decimal? LIST_PRICE { get; set; }

    }
}
