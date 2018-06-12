using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApp.Models
{
    [MetadataType(typeof(MessageMetadata))]
    public partial class Message
    {
    }

    public partial class MessageMetadata
    {
         

    }




	public class MessageChangeViewModel
    {
        public IEnumerable<Message> inserted { get; set; }
        public IEnumerable<Message> deleted { get; set; }
        public IEnumerable<Message> updated { get; set; }
    }

}
