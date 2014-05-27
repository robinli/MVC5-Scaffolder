using System;
using System.Collections.Generic;
using System.Linq;
 
namespace Microsoft.AspNet.Scaffolding.WebForms.Models
{
    [Serializable]
    public class MetaColumnInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool Nullable { get; set; }


        public MetaColumnInfo() { }

        //public MetadataFieldinfo(Microsoft.AspNet.Scaffolding.WebForms.UI.MetadataFieldViewModel c1)
        //{
        //    this.Name =c1.Name;
        //    DisplayName = c1.DisplayName;
        //    Nullable = c1.Nullable;
        //}
        public MetaColumnInfo(Microsoft.AspNet.Scaffolding.Core.Metadata.PropertyMetadata p1)
        {
            this.Name = p1.PropertyName;
            DisplayName = p1.PropertyName;
            Nullable = (p1.IsPrimaryKey ? false : true);
        }
    }
}
