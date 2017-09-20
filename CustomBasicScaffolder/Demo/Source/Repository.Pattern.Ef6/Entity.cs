using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Infrastructure;
using System;

namespace Repository.Pattern.Ef6
{
    public abstract class Entity : IObjectState,IAuditable
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
        public DateTime? Created { get; set ; }
        public string CreatedBy { get; set ; }
        public DateTime? LastModified { get ; set ; }
        public string LastModifiedBy { get ; set; }
    }
}