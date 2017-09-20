using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Pattern.Ef6
{
    public abstract class Entity : IObjectState,IAuditable
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
        [Display(Name = "创建时间")]
        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { get; set ; }
        [Display(Name = "创建用户")]
        [MaxLength(20)]
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set ; }
        [Display(Name = "最后更新时间")]
        [ScaffoldColumn(false)]
        public DateTime? LastModifiedDate { get ; set ; }
        [Display(Name = "最后更新用户")]
        [MaxLength(20)]
        [ScaffoldColumn(false)]
        public string LastModifiedBy { get ; set; }
    }
}