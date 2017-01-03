using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Happy.Scaffolding.MVC.Scaffolders
{
    public static class  ModelDisplayExtensions
    {
        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            Type type = typeof(TModel);
            IEnumerable<string> propertyList;

            //unless it's a root property the expression NodeType will always be Convert
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expression.Body as UnaryExpression;
                    propertyList = (ue != null ? ue.Operand : null).ToString().Split(".".ToCharArray()).Skip(1); //don't use the root property
                    break;
                default:
                    propertyList = expression.Body.ToString().Split(".".ToCharArray()).Skip(1);
                    break;
            }

            //the propert name is what we're after
            string propertyName = propertyList.Last();
            //list of properties - the last property name
            string[] properties = propertyList.Take(propertyList.Count() - 1).ToArray();

            Expression expr = null;
            foreach (string property in properties)
            {
                PropertyInfo propertyInfo = type.GetProperty(property);
                expr = Expression.Property(expr, type.GetProperty(property));
                type = propertyInfo.PropertyType;
            }

            DisplayAttribute attr = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            // Look for [MetadataType] attribute in type hierarchy
            // http://stackoverflow.com/questions/1910532/attribute-isdefined-doesnt-see-attributes-applied-with-metadatatype-class
            if (attr == null)
            {
                MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                    {
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                    }
                }
            }
            //To support translations call attr.GetName() instead of attr.Name
            return (attr != null) ? attr.GetName() : String.Empty;
        }
    }



    public static class AttributeHelper
    {
        public static DisplayAttribute GetDisplayAttribute(object obj, string propertyName) {
            if (obj == null) return null;
            return GetDisplayAttribute(obj.GetType(), propertyName);
        }

        public static DisplayAttribute GetDisplayAttribute(Type type, string propertyName)
        {
          
            var property = type.GetProperty(propertyName);
            if (property == null) return null;

            return GetDisplayAttribute(property);
        }

        public static DisplayAttribute GetDisplayAttribute(PropertyInfo property)
        {
           var atts= property.GetCustomAttributes(
                typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), true);

           if (atts.Length == 0)
           {
               
               var metaattr = GetMetaDisplayAttribute(property);
               return metaattr;
           }
           else {
               return atts[0] as System.ComponentModel.DataAnnotations.DisplayAttribute;
           }
        }

        private static DisplayAttribute GetMetaDisplayAttribute(PropertyInfo property)
        {
            var atts = property.DeclaringType.GetCustomAttributes(
                typeof(MetadataTypeAttribute), true);
            if (atts.Length == 0)
                return null;

            var metaAttr = atts[0] as MetadataTypeAttribute;
            var metaProperty =
                metaAttr.MetadataClassType.GetProperty(property.Name);
            if (metaProperty == null)
                return null;
            return GetDisplayAttribute(metaProperty);
        }


        public static string GetDisplayName(object obj, string propertyName)
        {
            if (obj == null) return null;
            return GetDisplayName(obj.GetType(), propertyName);

        }

        public static string GetDisplayName(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null) return null;

            return GetDisplayName(property);
        }

        public static string GetDisplayName(PropertyInfo property)
        {
            var attrName = GetAttributeDisplayName(property);
            if (!string.IsNullOrEmpty(attrName))
                return attrName;

            var metaName = GetMetaDisplayName(property);
            if (!string.IsNullOrEmpty(metaName))
                return metaName;

            return property.Name.ToString();
        }

        private static string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), true);
            if (atts.Length == 0)
                return null;
            return (atts[0] as System.ComponentModel.DataAnnotations.DisplayAttribute).Name;
        }
        
        private static string GetMetaDisplayName(PropertyInfo property)
        {
            var atts = property.DeclaringType.GetCustomAttributes(
                typeof(MetadataTypeAttribute), true);
            if (atts.Length == 0)
                return null;

            var metaAttr = atts[0] as MetadataTypeAttribute;
            var metaProperty =
                metaAttr.MetadataClassType.GetProperty(property.Name);
            if (metaProperty == null)
                return null;
            return GetAttributeDisplayName(metaProperty);
        }
        //---------------------------------------------------------------------
        public static bool GetRequired(object obj, string propertyName)
        {
            if (obj == null) return false;
            return GetRequired(obj.GetType(), propertyName);

        }

        public static bool GetRequired(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null) return false;

            return GetRequired(property);
        }
        public static bool GetRequired(PropertyInfo property)
        {
            var required = GetAttributeRequired(property);
            if (required)
                return required;

            required = GetMetaRequired(property);
            return required;
            
        }
        private static bool GetAttributeRequired(PropertyInfo property) {

            var atts = property.GetCustomAttributes(
                typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true);
            if (atts.Length == 0)
            {
                if (property.PropertyType==typeof(int) ||
                    property.PropertyType==typeof(decimal) ||
                    property.PropertyType==typeof(DateTime) ||
                    property.PropertyType==typeof(float) ||
                    property.PropertyType==typeof(double) 
                   )
                    return true;
                else
                    return false;
            }
            return true;
        }
        private static bool GetMetaRequired(PropertyInfo property) {
            var atts = property.DeclaringType.GetCustomAttributes(
                    typeof(MetadataTypeAttribute), true);
            if (atts.Length == 0)
                return false;

            var metaAttr = atts[0] as MetadataTypeAttribute;
            var metaProperty =
                metaAttr.MetadataClassType.GetProperty(property.Name);
            if (metaProperty == null)
                return false;
            return GetAttributeRequired(metaProperty);
        }

        internal static string GetMaxLenght(Type type, string p)
        {
            var property = type.GetProperty(p);
            if (property == null) return "";

            return GetMaxLenght(property);
        }

        private static string GetMaxLenght(PropertyInfo property)
        {
            var str = GetAttributeMaxLength(property);
            if (str!=string.Empty)
                return str;

            str = GetMetaMaxLenght(property);
            return str;
        }

        private static string GetMetaMaxLenght(PropertyInfo property)
        {
            var atts = property.DeclaringType.GetCustomAttributes(
                    typeof(MetadataTypeAttribute), true);
            if (atts.Length == 0)
                return "";

            var metaAttr = atts[0] as MetadataTypeAttribute;
            var metaProperty =
                metaAttr.MetadataClassType.GetProperty(property.Name);
            if (metaProperty == null)
                return "";
            return GetAttributeMaxLength(metaProperty);
        }

        private static string GetAttributeMaxLength(PropertyInfo property)
        {
            string min = "";
            string max = "";
            if (property.PropertyType == typeof(string))
            {
                var atts0 = property.GetCustomAttributes(
                    typeof(System.ComponentModel.DataAnnotations.MinLengthAttribute), true);
                var atts1 = property.GetCustomAttributes(
                    typeof(System.ComponentModel.DataAnnotations.MaxLengthAttribute), true);
                if (atts0.Length == 0)
                    min = "0";
                else
                    min = (atts0[0] as System.ComponentModel.DataAnnotations.MinLengthAttribute).Length.ToString();

                if (atts1.Length == 0)
                    max = "50";
                else
                    max = (atts1[0] as System.ComponentModel.DataAnnotations.MaxLengthAttribute).Length.ToString();

                return string.Format(",validType:'length[{0},{1}]'", min, max);
            }
            else if (property.PropertyType == typeof(int) ||
                     property.PropertyType == typeof(float) ||
                     property.PropertyType == typeof(decimal) ||
                     property.PropertyType == typeof(double)
                     )
            {
                var atts = property.GetCustomAttributes(
                    typeof(System.ComponentModel.DataAnnotations.RangeAttribute), true);

                if (atts.Length == 0)
                {
                    if (property.PropertyType == typeof(float) ||
                     property.PropertyType == typeof(decimal) ||
                     property.PropertyType == typeof(double))
                    
                        return string.Format(",precision:2");
                    else
                        return string.Format(",precision:0");
                }
                else
                {
                    min = (atts[0] as System.ComponentModel.DataAnnotations.RangeAttribute).Minimum.ToString();
                    max = (atts[0] as System.ComponentModel.DataAnnotations.RangeAttribute).Maximum.ToString();
                }
                if (property.PropertyType == typeof(float) ||
                    property.PropertyType == typeof(decimal) ||
                    property.PropertyType == typeof(double))
                    return string.Format(",min:{0},max:{1},precision:2", min, max);
                else
                    return string.Format(",min:{0},max:{1},precision:0", min, max);


            }
            else {
                return string.Empty;
            }
            
        }
    }

    [Serializable]
    public class DisplayAttributeViewModel {
        
       public string EntityTypeName { get; set; }
       public string FieldName { get; set; }
       //public DisplayAttribute DisplayAttribute { get; set; }
       public bool AutoGenerateField { get; set; }
       public bool AutoGenerateFilter { get; set; }
       public string Description { get; set; }
       public string GroupName { get; set; }
       public string Name { get; set; }
       public int Order { get; set; }
       public string Prompt { get; set; }
       public string ShortName { get; set; }
       
    }
}
