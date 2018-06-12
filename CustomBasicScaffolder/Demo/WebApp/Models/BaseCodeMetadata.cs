using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
     
    public partial class BaseCode
    {
    }

    public partial class BaseCodeMetadata
    {
        

    }




	public class BaseCodeChangeViewModel
    {
        public IEnumerable<BaseCode> inserted { get; set; }
        public IEnumerable<BaseCode> deleted { get; set; }
        public IEnumerable<BaseCode> updated { get; set; }
    }

}
