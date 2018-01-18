using System;
using System.Data;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using System.Collections.Generic;

namespace Repository.Pattern.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();

        void SetAutoDetectChangesEnabled(bool enabled);

        void BulkSaveChanges();
        void BulkInsert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IObjectState;
        void BulkUpdate<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IObjectState;
        void BulkDelete<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IObjectState;
        void BulkMerge<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, IObjectState;
    }
}