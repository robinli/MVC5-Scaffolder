using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public class QueryBooks_QueryFormViewModel
    {
        [Display(Name="查詢書名")]
        public string queryBookName { get; set; }

        public IEnumerable<QueryBooks_Result> Result { get; set; }
    }


    [MetadataType(typeof(QueryBooks_ResultMetadata))]
    public partial class QueryBooks_Result
    {
    }

    public partial class QueryBooks_ResultMetadata
    {
        [Display(Name = "ISBN")]
        public string ID { get; set; }

        [Display(Name = "書名")]
        public string BOOKNAME { get; set; }

        [Display(Name = "作者")]
        public string AUTHOR { get; set; }

        [Display(Name = "出版日期")]
        public Nullable<System.DateTime> PUBLISH_UTC { get; set; }

        [Display(Name = "版次")]
        public Nullable<int> VERSION_NUM { get; set; }

        [Display(Name = "售價")]
        public Nullable<decimal> LIST_PRICE { get; set; }
    }

}