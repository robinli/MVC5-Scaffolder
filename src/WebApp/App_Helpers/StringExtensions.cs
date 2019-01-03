using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp
{
    /// <summary>
    /// Provides utility methods for converting string values to other data types.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Removes dashes ("-") from the given object value represented as a string and returns an empty string ("")
        ///     when the instance type could not be represented as a string.
        ///     <para>
        ///         Note: This will return the type name of given isntance if the runtime type of the given isntance is not a
        ///         string!
        ///     </para>
        /// </summary>
        /// <param name="value">The object instance to undash when represented as its string value.</param>
        /// <returns></returns>
        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }

        /// <summary>
        ///     Removes dashes ("-") from the given string value.
        /// </summary>
        /// <param name="value">The string value that optionally contains dashes.</param>
        /// <returns></returns>
        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }
    }



    public static class ObjectExtensions
    {
        public static bool IsTrue(this object x) {
            var val = x.ToString().ToLower();
            var result = false;
            if (string.IsNullOrEmpty(val))
            {
                return false;
            }
            else if (bool.TryParse(val, out result)) {
                return result;
            }
            else {
                switch (val) {
                    case "y":
                        return true;
                    case "n":
                        return false;
                    case "yes":
                        return true;
                    case "no":
                        return false;
                    case "1":
                        return true;
                    case "0":
                        return false;
                    case "true":
                        return true;
                    case "t":
                        return true;
                    case "false":
                        return false;
                    case "f":
                        return false;
                    default:
                        return false;
                }
            }
        }

        public static bool IsNumeric(this object x) { return (x == null ? false : IsNumeric(x.GetType())); }

        // Method where you know the type of the object
        public static bool IsNumeric(Type type) { return IsNumeric(type, Type.GetTypeCode(type)); }

        // Method where you know the type and the type code of the object
        public static bool IsNumeric(Type type, TypeCode typeCode) { return (typeCode == TypeCode.Decimal || (type.IsPrimitive && typeCode != TypeCode.Object && typeCode != TypeCode.Boolean && typeCode != TypeCode.Char)); }
    }
}