using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public partial class Work: Entity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Enableed { get; set; }
        public int Hour { get; set; }
        public int Priority { get; set; }
        public decimal Score { get; set; }

        
    }
    public partial class Work1 : Entity
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Enableed { get; set; }
        public int Hour { get; set; }
        public int Priority { get; set; }
        public decimal Score { get; set; }

        #region Auditable
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        #endregion

    }
}