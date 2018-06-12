using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using TrackableEntities;
using System.Collections.Generic;

namespace Repository.Pattern.Ef6
{
    public abstract class Entity : ITrackable, IAuditable
    {
        [NotMapped]
        public TrackingState TrackingState { get; set; }
        [NotMapped]
        public ICollection<string> ModifiedProperties { get; set; }
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