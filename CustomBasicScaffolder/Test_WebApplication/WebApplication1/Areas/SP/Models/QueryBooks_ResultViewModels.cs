using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class QueryBooks_QueryFormViewModel
    {
        [Display(Name="filter Book name")]
        public string queryBookName { get; set; }
        [Display(Name="filter Author")]
        public string queryAuthor { get; set; }

        public bool DoExport { get; set; }

        public List<QueryBooks_Result> Result { get; set; }
    }

    [MetadataType(typeof(QueryBooks_ResultMetadata))]
    public partial class QueryBooks_Result
    {
    }

    public partial class QueryBooks_ResultMetadata
    {
        [Display(Name = "ISBN")]
        public string ID { get; set; }

        [Display(Name = "Book name")]
        public string BOOKNAME { get; set; }

        [Display(Name = "Author")]
        public string AUTHOR { get; set; }

        [Display(Name = "Publish date")]
        public System.DateTime? PUBLISH_UTC { get; set; }

        [Display(Name = "Version")]
        public int? VERSION_NUM { get; set; }

        [Display(Name = "List price")]
        public decimal? LIST_PRICE { get; set; }

    }
}
