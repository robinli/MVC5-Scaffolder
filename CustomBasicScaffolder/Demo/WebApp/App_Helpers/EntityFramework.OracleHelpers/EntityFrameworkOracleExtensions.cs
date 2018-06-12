using EntityFramework.OracleHelpers.Conventions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.OracleHelpers
{
    //public static class EntityFrameworkOracleExtensions
    //{
    //    /// <summary>
    //    /// Automagically apply all conventions with default values if on Oracle connection, otherwise do nothing
    //    /// </summary>
    //    /// <param name="context">Entity Framework DbContext instance</param>
    //    /// <param name="modelBuilder">Entity Framework DbModelBuilder instance</param>
    //    public static void ApplyAllConventionsIfOracle(this DbContext context, DbModelBuilder modelBuilder)
    //    {
    //        if (context.IsOracle())
    //        {
    //            ApplyAllOracleConventions(modelBuilder);
    //        }
    //    }

    //    /// <summary>
    //    /// Apply all Oracle conventions with default values.
    //    /// </summary>
    //    /// <param name="modelBuilder">Entity Framework DbModelBuilder instance</param>
    //    public static void ApplyAllOracleConventions(this DbModelBuilder modelBuilder)
    //    {
    //        ApplyAllUpperCaseConventions(modelBuilder);
    //        ApplyStringMaxLengthConvention(modelBuilder);
    //    }

    //    /// <summary>
    //    /// Add custom upper case conventions to ModelBuilder instance. Default is to convert to UNDER_SCORE_CASE.
    //    /// </summary>
    //    /// <param name="modelBuilder">Entity Framework DbModelBuilder instance</param>
    //    /// <param name="convertToUnderscore">Convert to UNDER_SCORE_CASE</param>
    //    public static void ApplyAllUpperCaseConventions(this DbModelBuilder modelBuilder, bool convertToUnderscore = true)
    //    {

    //        IConvention[] conventions =
    //        {
    //                new UpperCaseTableNameConvention(convertToUnderscore),
    //                new UpperCaseForeignKeyNameConvention(convertToUnderscore),
    //                new UpperCaseColumnNameConvention(convertToUnderscore)
    //        };

    //        modelBuilder.Conventions.Add(conventions);
    //    }

    //    /// <summary>
    //    /// Add custom String property type convention to set MaxLength so created database column is NVARCHAR2(2000)
    //    /// </summary>
    //    /// <param name="modelBuilder">Entity Framework DbModelBuilder instance</param>
    //    /// <param name="length">Length of column</param>
    //    public static void ApplyStringMaxLengthConvention(this DbModelBuilder modelBuilder, int length = 2000)
    //    {
    //        modelBuilder.Conventions.Add(new StringPropertyMaxLengthConvention(length));
    //    }

    //    /// <summary>
    //    /// Get Provider Invariant Name from current DbContext instance
    //    /// </summary>
    //    /// <param name="context">Entity Framework DbContext instance</param>
    //    /// <returns>String containing Provider Invariant Name</returns>
    //    public static string GetProviderInvariantName(this DbContext context)
    //    {
    //        return DbConfiguration.DependencyResolver.GetService<IProviderInvariantName>(DbProviderServices.GetProviderFactory(context.Database.Connection)).Name;
    //    }

    //    /// <summary>
    //    /// Check if current DbConnection Provider Invariant Name is 'Oracle.ManagedDataAccess.Client'
    //    /// </summary>
    //    /// <param name="context">Entity Framework DbContext instance</param>
    //    /// <returns>True if Provider Invariant Name is Oracle</returns>
    //    public static bool IsOracle(this DbContext context)
    //    {
    //        var providerInvariantName = GetProviderInvariantName(context);

    //        return !string.IsNullOrWhiteSpace(providerInvariantName) &&
    //            providerInvariantName.StartsWith("Oracle.ManagedDataAccess.Client", StringComparison.OrdinalIgnoreCase);
    //    }
    //}

}