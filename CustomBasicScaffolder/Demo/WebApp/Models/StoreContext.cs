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
 

namespace WebApp.Models
{
    public class StoreContext:DataContext
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
                    auditableEntity.Entity.LastModified = currentDateTime;
                    switch (auditableEntity.State)
                    {
                        case EntityState.Added:
                            auditableEntity.Entity.Created = currentDateTime;
                            auditableEntity.Entity.CreatedBy = HttpContext.Current.User.Identity.Name;
                            break;
                        case EntityState.Modified:
                            auditableEntity.Property("Created").IsModified = false;
                            auditableEntity.Property("CreatedBy").IsModified = false;
                            auditableEntity.Entity.LastModified = currentDateTime;
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
                    auditableEntity.Entity.LastModified = currentDateTime;
                    switch (auditableEntity.State)
                    {
                        case EntityState.Added:
                            auditableEntity.Entity.Created = currentDateTime;
                            auditableEntity.Entity.CreatedBy = HttpContext.Current.User.Identity.Name;
                            break;
                        case EntityState.Modified:
                            auditableEntity.Property("Created").IsModified = false;
                            auditableEntity.Property("CreatedBy").IsModified = false;
                            auditableEntity.Entity.LastModified = currentDateTime;
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
    }
}