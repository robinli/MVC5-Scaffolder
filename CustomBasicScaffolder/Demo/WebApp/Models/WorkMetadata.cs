using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    //[MetadataType(typeof(WorkMetadata))]
    public partial class Work
    {
    }

    public partial class WorkMetadata
    {
        [Required(ErrorMessage = "Please enter : Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "StartDate")]
        public DateTime StartDate { get; set; }

        [Display(Name = "EndDate")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Enableed")]
        public bool Enableed { get; set; }

        [Display(Name = "Hour")]
        public int Hour { get; set; }

        [Display(Name = "Priority")]
        public int Priority { get; set; }

        [Display(Name = "Score")]
        public decimal Score { get; set; }

    }




	public class WorkChangeViewModel
    {
        public IEnumerable<Work> inserted { get; set; }
        public IEnumerable<Work> deleted { get; set; }
        public IEnumerable<Work> updated { get; set; }
    }

}
