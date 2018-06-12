using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class NotificationViewModel
    {
        public int Group { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime Time { get; set; }
        public int TimeAgo { get; set; }

    }
}