using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    //[MetadataType(typeof(NotificationMetadata))]
    public partial class Notification
    {
    }

    


	public class NotificationChangeViewModel
    {
        public IEnumerable<Notification> inserted { get; set; }
        public IEnumerable<Notification> deleted { get; set; }
        public IEnumerable<Notification> updated { get; set; }
    }

}
