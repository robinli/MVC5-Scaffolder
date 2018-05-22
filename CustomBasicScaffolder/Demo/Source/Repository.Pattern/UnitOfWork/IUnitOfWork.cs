using System;
using System.Data;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using System.Collections.Generic;
using TrackableEntities;

namespace Repository.Pattern.UnitOfWork
{
    public interface IUnitOfWork 
    {
        int SaveChanges();
        int ExecuteSqlCommand(string sql, params object[] parameters);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, ITrackable;
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