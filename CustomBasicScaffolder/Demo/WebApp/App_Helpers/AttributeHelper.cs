using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApp
{
    public static class AttributeHelper
    {
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
        private static bool GetAttributeRequired(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true);
            if (atts.Length == 0)
                return false;
            return true;
        }
        private static bool GetMetaRequired(PropertyInfo property)
        {
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
    }
}