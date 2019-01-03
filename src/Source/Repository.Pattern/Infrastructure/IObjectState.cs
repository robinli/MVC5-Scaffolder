
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Pattern.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }


    public interface IAuditable
    {
        DateTime? CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? LastModifiedDate { get; set; }
        string LastModifiedBy { get; set; }
    }
}