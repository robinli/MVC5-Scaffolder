using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static EntityFramework.OracleHelpers.Conventions.PascalCaseToUnderscoreCase;

namespace EntityFramework.OracleHelpers.Conventions
{
    public static class PascalCaseToUnderscoreCase
    {
        // regex : http://nlpdotnet.com/SampleCode/CamelCaseStringToWords.aspx
        public static string ToUnderscoreCase(string name)
        {
            var result = Regex.Replace(name, "((?<=[a-z])(?=[A-Z]))|((?<=[A-Z])(?=[A-Z][a-z]))", "_");

            return result;
        }
    }

    /// <summary>
    /// Make all column names upper case. Default is make it UNDER_SCORE_CASE too.
    /// </summary>
    public class UpperCaseColumnNameConvention : Convention
    {
        private bool convertToUnderscore = true;

        public UpperCaseColumnNameConvention() : this(true)
        {
        }

        public UpperCaseColumnNameConvention(bool convertToUnderscore)
        {
            this.convertToUnderscore = convertToUnderscore;
            Properties().Configure(c => c.HasColumnName(GetColumnName(c)));
        }

        private string GetColumnName(ConventionPrimitivePropertyConfiguration type)
        {
            var result = type.ClrPropertyInfo.Name;

            if (convertToUnderscore)
            {
                result = ToUnderscoreCase(result);
            }

            return result.ToUpperInvariant();
        }
    }

    /// <summary>
    /// Make all table names upper case. Default is make it UNDER_SCORE_CASE too.
    /// </summary>
    public class UpperCaseTableNameConvention : IStoreModelConvention<EntitySet>
    {
        private bool convertToUnderscore = true;

        public UpperCaseTableNameConvention() : this(true)
        {
        }

        public UpperCaseTableNameConvention(bool convertToUnderscore)
        {
            this.convertToUnderscore = convertToUnderscore;
        }

        public void Apply(EntitySet item, DbModel model)
        {
            var result = item.Table;

            if (convertToUnderscore)
            {
                result = ToUnderscoreCase(result);
            }

            item.Table = result.ToUpperInvariant();
        }
    }

    /// <summary>
    /// Make all foreign key names upper case. Default is make it UNDER_SCORE_CASE too.
    /// </summary>
    public class UpperCaseForeignKeyNameConvention : IStoreModelConvention<AssociationType>
    {
        private bool convertToUnderscore = true;

        public UpperCaseForeignKeyNameConvention() : this(true)
        {
        }

        public UpperCaseForeignKeyNameConvention(bool convertToUnderscore)
        {
            this.convertToUnderscore = convertToUnderscore;
        }

        public void Apply(AssociationType association, DbModel model)
        {
            if (association.IsForeignKey)
            {
                UpperCaseForeignKeyProperties(association.Constraint.ToProperties);
            }
        }

        private void UpperCaseForeignKeyProperties(IEnumerable<EdmProperty> properties)
        {
            string result;
            foreach (var property in properties)
            {
                result = property.Name;
                if (convertToUnderscore)
                {
                    result = ToUnderscoreCase(result);
                }

                property.Name = result.ToUpperInvariant();
            }
        }
    }
}