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

#endregion

namespace Repository.Pattern.Ef6
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        #region Private Fields

        private IDataContextAsync _dataContext;
        private bool _disposed;
        private ObjectContext _objectContext;
        private DbTransaction _transaction;
        private Dictionary<string, dynamic> _repositories;

        #endregion Private Fields

        #region Constuctor

        public UnitOfWork(IDataContextAsync dataContext)
        {
            string licenseName = "13;100-TEST";//... PRO license name
            string licenseKey = "0D0E8959891B87975F829CA4DDBA76B1";//... PRO license key
            Z.EntityFramework.Extensions.LicenseManager.AddLicense(licenseName, licenseKey);

            _dataContext = dataContext;
            
            _repositories = new Dictionary<string, dynamic>();
        }

        public void SetAutoDetectChangesEnabled(bool enabled)
        {
            this._dataContext.SetAutoDetectChangesEnabled(enabled);
        }


        #endregion Constuctor/Dispose

        #region Dispose
        //https://msdn.microsoft.com/library/ms244737.aspx

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // NOTE: Leave out the finalizer altogether if this class doesn't 
        // own unmanaged resources itself, but leave the other methods
        // exactly as they are. 
        ~UnitOfWork()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // free other managed objects that implement
                    // IDisposable only

                    try
                    {
                        if (_objectContext != null)
                        {
                            if (_objectContext.Connection.State == ConnectionState.Open)
                                _objectContext.Connection.Close();

                            _objectContext.Dispose();
                            _objectContext = null;
                        }
                        if (_dataContext != null)
                        {
                            _dataContext.Dispose();
                            _dataContext = null;
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // do nothing, the objectContext has already been disposed
                    }

                    if (_repositories != null)
                        _repositories = null;
                }

                _disposed = true;
            }            
        }

        #endregion

        public int SaveChanges()
        {
            return _dataContext.SaveChanges();
        }


      
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            }

            return RepositoryAsync<TEntity>();
        }

        public Task<int> SaveChangesAsync()
        {
            return _dataContext.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _dataContext.SaveChangesAsync(cancellationToken);
        }

        public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IObjectState
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepositoryAsync<TEntity>>();
            }

            if (_repositories == null)
            {
                _repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dataContext, this));

            return _repositories[type];
        }

        #region Unit of Work Transactions

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _objectContext = ((IObjectContextAdapter) _dataContext).ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }

            _transaction = _objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _dataContext.SyncObjectsStatePostCommit();
        }
 
        public void BulkSaveChanges()
        {
            this._dataContext.BulkSaveChanges();
        }
        public Task BulkSaveChangesAsync()
        {
            return this._dataContext.BulkSaveChangesAsync();
        }

        void IUnitOfWork.BulkInsert<TEntity>(IEnumerable<TEntity> entities)
        {
            this._dataContext.BulkInsert(entities);
        }

        void IUnitOfWork.BulkUpdate<TEntity>(IEnumerable<TEntity> entities)
        {
            this._dataContext.BulkUpdate(entities);
        }

        void IUnitOfWork.BulkDelete<TEntity>(IEnumerable<TEntity> entities)
        {
            this._dataContext.BulkDelete(entities);
        }

        void IUnitOfWork.BulkMerge<TEntity>(IEnumerable<TEntity> entities)
        {
            this._dataContext.BulkMerge(entities);
        }

        #endregion
    }
}