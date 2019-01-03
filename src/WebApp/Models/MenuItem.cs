using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    //Please Register DbSet in DbContext.cs
    //public DbSet<MenuItem> MenuItems { get; set; }
    //public Entity.DbSet<MenuItem> MenuItems { get; set; }
    public partial class MenuItem:Entity
    {
        public MenuItem()
        {
            SubMenus = new HashSet<MenuItem>();
        }
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(20)]
        [Required]
        public string Code { get; set; }
        [MaxLength(100)]
        [Required]
        public string Url { get; set; }
        [MaxLength(100)]
        public string Controller { get; set; }
        [MaxLength(100)]
        public string Action { get; set; }

        [StringLength(50)]
        public string IconCls { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<MenuItem> SubMenus { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public MenuItem Parent { get; set; }
    }
}