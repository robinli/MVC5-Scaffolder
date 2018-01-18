using System;
using Repository.Pattern.Infrastructure;
using System.Collections;
using System.Collections.Generic;

namespace Repository.Pattern.DataContext
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState;
        void SyncObjectsStatePostCommit();



        void SetAutoDetectChangesEnabled(bool enabled);
                     
        void BulkSaveChanges();
        void BulkInsert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IObjectState;
        void BulkUpdate<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IObjectState;
        void BulkDelete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IObjectState;
        void BulkMerge<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IObjectState;


    }
}