using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;
using Repository.Pattern.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;
using System.Reflection;

using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace WebApp.Models
{
    public class StoreContext : DbContext
    {
        

        public StoreContext()
            : base("Name=DefaultConnection")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public System.Data.Entity.DbSet<WebApp.Models.Company> Companies { get; set; }

        public System.Data.Entity.DbSet<WebApp.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<WebApp.Models.Work> Works { get; set; }

        public System.Data.Entity.DbSet<WebApp.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<WebApp.Models.BaseCode> BaseCodes { get; set; }
        public System.Data.Entity.DbSet<WebApp.Models.CodeItem> CodeItems { get; set; }

        public System.Data.Entity.DbSet<WebApp.Models.MenuItem> MenuItems { get; set; }

        public DbSet<RoleMenu> RoleMenus { get; set; }

        public DbSet<DataTableImportMapping> DataTableImportMappings { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Message> Messages { get; set; }
        //public DbSet<Work1> Work1es { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var currentDateTime = DateTime.Now;
            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
                {
                    //auditableEntity.Entity.LastModifiedDate = currentDateTime;
                    switch (auditableEntity.State)
                    {
                        case EntityState.Added:
                            auditableEntity.Entity.CreatedDate = currentDateTime;
                            auditableEntity.Entity.CreatedBy = HttpContext.Current.User.Identity.Name;
                            break;
                        case EntityState.Modified:
                            auditableEntity.Property("CreatedDate").IsModified = false;
                            auditableEntity.Property("CreatedBy").IsModified = false;
                            auditableEntity.Entity.LastModifiedDate = currentDateTime;
                            auditableEntity.Entity.LastModifiedBy = HttpContext.Current.User.Identity.Name;
                            //if (auditableEntity.Property(p => p.Created).IsModified || auditableEntity.Property(p => p.CreatedBy).IsModified)
                            //{
                            //    throw new DbEntityValidationException(string.Format("Attempt to change created audit trails on a modified {0}", auditableEntity.Entity.GetType().FullName));
                            //}
                            break;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var currentDateTime = DateTime.Now;
            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Entity.LastModifiedDate = currentDateTime;
                    switch (auditableEntity.State)
                    {
                        case EntityState.Added:
                            auditableEntity.Entity.CreatedDate = currentDateTime;
                            auditableEntity.Entity.CreatedBy = HttpContext.Current.User.Identity.Name;
                            break;
                        case EntityState.Modified:
                            auditableEntity.Property("CreatedDate").IsModified = false;
                            auditableEntity.Property("CreatedBy").IsModified = false;
                            auditableEntity.Entity.LastModifiedDate = currentDateTime;
                            auditableEntity.Entity.LastModifiedBy = HttpContext.Current.User.Identity.Name;
                            //if (auditableEntity.Property(p => p.Created).IsModified || auditableEntity.Property(p => p.CreatedBy).IsModified)
                            //{
                            //    throw new DbEntityValidationException(string.Format("Attempt to change created audit trails on a modified {0}", auditableEntity.Entity.GetType().FullName));
                            //}
                            break;
                    }
                }
            }
            return base.SaveChanges();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Oracle 表所有者，（SQL 改成 dbo(默认)，也可删除此设置）
            //this.ApplyAllConventionsIfOracle(modelBuilder);
            //modelBuilder.HasDefaultSchema(DbSchema);
            ////将数据列转换成大写
            //modelBuilder.Conventions.Add<UpperCaseColumnNameConvention>();
            ////将TableName转大写,TableName 指定的除外
            //modelBuilder.Conventions.Add<UpperCaseTableNameConvention>();

            #region 设置特殊格式以及浮点型长度
            //统一设置Decimal 长度（数据库实际位数可以缩短）
            //modelBuilder.Properties<decimal>().Configure(c => c.HasPrecision(18, 5));
            //modelBuilder.Entity<Work>().Property(x => x.Score).HasPrecision(10, 2);
            //modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasPrecision(18, 3);



            #endregion

            ////关闭一对多的级联删除。
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            ////关闭多对多的级联删除。
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            ////移除EF的表名公约  
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            ////移除对MetaData表的查询验证，否则每次都要访问Metadata这个表
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            ////数据库不存在时重新创建数据库
            //Database.SetInitializer<StoreContext>(new CreateDatabaseIfNotExists<WebdbContext>());
            ////每次启动应用程序时创建数据库
            //Database.SetInitializer<StoreContext>(new DropCreateDatabaseAlways<WebdbContext>());
            ////模型更改时重新创建数据库
            //Database.SetInitializer<StoreContext>(new DropCreateDatabaseIfModelChanges<WebdbContext>());
            ////从不创建数据库
            //Database.SetInitializer<StoreContext>(null);

            base.OnModelCreating(modelBuilder);
        }
    }


    
}