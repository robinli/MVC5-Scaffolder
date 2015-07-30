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
        [StringLength(20)]
        [Required]
        [Index("IX_menuTitle", 1, IsUnique = true)]
        public string Title { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [StringLength(20)]
        [Required]
        [Index("IX_menuCode", 1, IsUnique = true)]
        public string Code { get; set; }
        [StringLength(100)]
        [Required]
        [Index("IX_menuUrl", 1, IsUnique = true)]
        public string Url { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<MenuItem> SubMenus { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public MenuItem Parent { get; set; }
    }
}