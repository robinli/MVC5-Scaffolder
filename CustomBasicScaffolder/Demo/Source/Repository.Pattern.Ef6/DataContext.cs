using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repository.Pattern.DataContext;
using Repository.Pattern.Infrastructure;
using TrackableEntities;
using TrackableEntities.EF6;

namespace Repository.Pattern.Ef6
{
    [Obsolete("DataContext has been deprecated. Instead use UnitOfWork which uses DbContext.")]
    public class DataContext : DbContext, IDataContextAsync
    {
        #region Private Fields
        private readonly Guid _instanceId;
   
        #endregion Private Fields

        public DataContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            _instanceId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public Guid InstanceId { get { return _instanceId; } }

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChanges"/>
        /// <returns>The number of objects written to the underlying database.</returns>
        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
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
                            //auditableEntity.Entity.CreatedBy = HttpContext.Current.User.Identity.Name;
                            break;
                        case EntityState.Modified:
                            auditableEntity.Property("CreatedDate").IsModified = false;
                            auditableEntity.Property("CreatedBy").IsModified = false;
                            auditableEntity.Entity.LastModifiedDate = currentDateTime;
                            //auditableEntity.Entity.LastModifiedBy = HttpContext.Current.User.Identity.Name;
                            //if (auditableEntity.Property(p => p.Created).IsModified || auditableEntity.Property(p => p.CreatedBy).IsModified)
                            //{
                            //    throw new DbEntityValidationException(string.Format("Attempt to change created audit trails on a modified {0}", auditableEntity.Entity.GetType().FullName));
                            //}
                            break;
                    }
                }
            }
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChangesAsync"/>
        /// <returns>A task that represents the asynchronous save operation.  The 
        ///     <see cref="Task.Result">Task.Result</see> contains the number of 
        ///     objects written to the underlying database.</returns>
        public override async Task<int> SaveChangesAsync()
        {
             
            return await this.SaveChangesAsync(CancellationToken.None);
        }
        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChangesAsync"/>
        /// <returns>A task that represents the asynchronous save operation.  The 
        ///     <see cref="Task.Result">Task.Result</see> contains the number of 
        ///     objects written to the underlying database.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();

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
                            //auditableEntity.Entity.CreatedBy = AppDomain.CurrentDomain.ApplicationIdentity.FullName;
                            break;
                        case EntityState.Modified:
                          
                            auditableEntity.Property("CreatedDate").IsModified = false;
                            auditableEntity.Property("CreatedBy").IsModified = false;
                            auditableEntity.Entity.LastModifiedDate = currentDateTime;
                            //auditableEntity.Entity.LastModifiedBy = AppDomain.CurrentDomain.ApplicationIdentity.FullName;
                            //if (auditableEntity.Property(p => p.Created).IsModified || auditableEntity.Property(p => p.CreatedBy).IsModified)
                            //{
                            //    throw new DbEntityValidationException(string.Format("Attempt to change created audit trails on a modified {0}", auditableEntity.Entity.GetType().FullName));
                            //}
                            break;
                    }
                }
            }

            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, ITrackable
        {
            this.ApplyChanges(entity);
        }

        private void SyncObjectsStatePreCommit()
        {
            var entities = ChangeTracker.Entries().Select(x => x.Entity).OfType<ITrackable>();
            this.ApplyChanges(entities);
        }

        public void SyncObjectsStatePostCommit()
        {
            var entities = ChangeTracker.Entries().Select(x => x.Entity).OfType<ITrackable>();
            this.ApplyChanges(entities);
        }

        

        public void SetAutoDetectChangesEnabled(bool enabled)
        {
            this.Configuration.AutoDetectChangesEnabled = enabled;
        }

        public   void BulkSaveChanges()
        {
            this.BulkSaveChanges(false);
        }

        public   Task BulkSaveChangesAsync()
        {

           return  this.BulkSaveChangesAsync(false, CancellationToken.None);
        }

        void IDataContext.BulkInsert<TEntity>(IEnumerable<TEntity> entities)
        {
            this.BulkInsert(entities);
        }

        void IDataContext.BulkUpdate<TEntity>(IEnumerable<TEntity> entities)
        {
            this.BulkUpdate(entities);
        }

        void IDataContext.BulkDelete<TEntity>(IEnumerable<TEntity> entities)
        {
            this.BulkDelete(entities);
        }

        void IDataContext.BulkMerge<TEntity>(IEnumerable<TEntity> entities)
        {
            this.BulkMerge(entities);
        }
    }
}