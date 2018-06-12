using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Repository.Pattern.Ef6;

namespace WebApp.Models
{
    public partial class Notification:Entity
    {
        public Notification()
        {

        }
        [Key]
        public int Id { get; set; }
        [Display( Name ="主题")]
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Display(Name = "消息")]
        [MaxLength(255)]
        public string Content { get; set; }
        [Display(Name = "链接")]
        [MaxLength(255)]
        public string Link { get; set; }
        [Display(Name = "已读")]
        public int Read { get; set; }
        [Display(Name = "From")]
        public string From { get; set; }
        [Display(Name = "To")]
        public string To { get; set; }
        [Display(Name = "分组")]
        public int Group { get; set; }

        public DateTime Created { get; set; }
        public string Creator { get; set; }
       

    }
}