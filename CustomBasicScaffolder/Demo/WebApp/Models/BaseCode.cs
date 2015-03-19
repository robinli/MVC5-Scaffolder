using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    //Please RegisterDbSet in DbContext.cs
    //public DbSet<BaseCode> BaseCodes { get; set; }
    //public Entity.DbSet<CodeItem> CodeItems { get; set; }
    public partial class BaseCode:Entity
    {
        public BaseCode()
        {
            CodeItems = new HashSet<CodeItem>();
        }
        [Key]
        public int Id { get; set; }
        [MaxLength(20),Display( Name="代码名称"),Required(ErrorMessage="必填"),Index(IsUnique=true)]
        public string CodeType { get; set; }
        [MaxLength(50), Display(Name = "描述"), Required(ErrorMessage = "必填")]
        public string Description { get; set; }

        public ICollection<CodeItem> CodeItems { get; set; }

    }

    public partial class CodeItem : Entity
    {
        [Key]
        public int Id { get; set; }
         [MaxLength(20), Display(Name = "值"), Required(ErrorMessage = "必填"), Index(IsUnique = true)]
        public string Code { get; set; }
        [MaxLength(20), Display(Name = "显示"), Required(ErrorMessage = "必填")]
        public string Text { get; set; }

        public int BaseCodeId { get; set; }
        [ForeignKey("BaseCodeId")]
        public BaseCode BaseCode { get; set; }
    }
}