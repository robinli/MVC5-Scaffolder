using System;
using System.Collections.Generic;
using System.Linq;
 
namespace Happy.Scaffolding.MVC.Models
{
    [Serializable]
    public class MetaColumnInfo
    {
        public string strDateType { get; set; }
        public euColumnType DataType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Nullable { get; set; }
        public int MaxLength { get; set; }
        public int RangeMin { get; set; }
        public int RangeMax { get; set; }

        public string MetaAttribute
        {
            get
            {
                switch (this.DataType)
                {
                    case euColumnType.stringCT:
                        if (this.MaxLength > 0)
                            return string.Format("[MaxLength({0})]", this.MaxLength);
                        else
                            break;
                    case euColumnType.intCT:
                    case euColumnType.decimalCT:
                        if (this.RangeMin > 0 && this.RangeMax > 0)
                            return string.Format("[Range({0}, {1})]", this.RangeMin, this.RangeMax);
                        else
                            break;
                    default:
                            break;
                }
                return string.Empty;
            }
        }


        public MetaColumnInfo() { }

        //public MetadataFieldinfo(Happy.Scaffolding.MVC.UI.MetadataFieldViewModel c1)
        //{
        //    this.Name =c1.Name;
        //    DisplayName = c1.DisplayName;
        //    Nullable = c1.Nullable;
        //}
        public MetaColumnInfo(Microsoft.AspNet.Scaffolding.Core.Metadata.PropertyMetadata p1)
        {
            this.strDateType = p1.ShortTypeName;
            this.DataType = GetColumnType(this.strDateType);
            this.Name = p1.PropertyName;
            DisplayName = p1.PropertyName;
            Nullable = (p1.IsPrimaryKey ? false : true);
        }

        private euColumnType GetColumnType(string shortTypeName)
        {
            return ParseEnum<euColumnType>(shortTypeName + "CT");
        }
        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
