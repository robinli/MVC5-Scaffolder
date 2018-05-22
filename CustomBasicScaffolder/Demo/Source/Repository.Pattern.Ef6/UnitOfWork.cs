#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

using Repository.Pattern.DataContext;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using CommonServiceLocator;
using System.Data.Entity;
using TrackableEntities;

#endregion

namespace Repository.Pattern.Ef6
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        #region Private Fields

        private readonly DbContext _context;
        protected DbTransaction Transaction;
        protected Dictionary<string, dynamic> Repositories;

        #endregion Private Fields

        #region Constuctor

        public UnitOfWork(DbContext context)
        {
            string licenseName = "13;100-TEST";//... PRO license name
            string licenseKey = "0D0E8959891B87975F829CA4DDBA76B1";//... PRO license key
            Z.EntityFramework.Extensions.LicenseManager.AddLicense(licenseName, licenseKey);

            _context = context;
            Repositories = new Dictionary<string, dynamic>();
        }
        #endregion
        public void SetAutoDetectChangesEnabled(bool enabled)
        {
            this._context.Configuration.AutoDetectChangesEnabled = enabled;
        }

        public int? CommandTimeout
        {
            get => _context.Database.CommandTimeout;
            set => _context.Database.CommandTimeout = value;
        }

        public virtual int SaveChanges() => _context.SaveChanges();



        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, ITrackable
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            }

            return RepositoryAsync<TEntity>();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public virtual IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, ITrackable
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepositoryAsync<TEntity>>();
            }

            if (Repositories == null)
            {
                Repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (Repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)Repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            Repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, this));

            return Repositories[type];
        }
        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlCommandAsync(sql, cancellationToken, parameters);
        }


        #region Unit of Work Transactions

        public virtual void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            if (objectContext.Connection.State != ConnectionState.Open)
            {
                objectContext.Connection.Open();
            }
            Transaction = objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public virtual bool Commit()
        {
            Transaction.Commit();
            return true;
        }

        public virtual void Rollback()
        {
            Transaction.Rollback();
        }
        #endregion

        #region entityframework-extensions

        public void BulkSaveChanges()
        {
            this._context.BulkSaveChanges();
        }
        public Task BulkSaveChangesAsync()
        {
            return this._context.BulkSaveChangesAsync();
        }

        void IUnitOfWork.BulkInsert<TEntity>(IEnumerable<TEntity> entities)
        {
            this._context.BulkInsert(entities);
        }

        void IUnitOfWork.BulkUpdate<TEntity>(IEnumerable<TEntity> entities)
        {
            this._context.BulkUpdate(entities);
        }

        void IUnitOfWork.BulkDelete<TEntity>(IEnumerable<TEntity> entities)
        {
            this._context.BulkDelete(entities);
        }

        void IUnitOfWork.BulkMerge<TEntity>(IEnumerable<TEntity> entities)
        {
            this._context.BulkMerge(entities);
        }

     

        #endregion
    }
}