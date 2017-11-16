using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace SqlHelper2 {
    public interface IDatabase {
        int ExecuteNonQuery(string sql, object parameters = null);
        int ExecuteNonQuery(string sql, IEnumerable<object> parameters = null);
        int ExecuteNonQuery(IEnumerable<string> sqllist);
        int ExecuteSPNonQuery(string procedureName, object parameters = null);

        IEnumerable<T> ExecuteDataReader<T>(string sql, object parameters, Func<DbDataReader, T> action);

        void ExecuteDataReader(string sql, object parameters, Action<DbDataReader> action);

        void ExecuteTransaction(Action<IDatabase> action);

        T ExecuteScalar<T>(string sql, object parameters = null);

        void BulkCopy(DataTable table, int batchSize = 100);

        bool HasRow(string sql, object parameters = null);
        [System.Obsolete("Use void  void ExecuteDataSet(string sql, object parameters,Action<DataSet> action)", true)]
        DataSet ExecuteDataSet(string sql, object parameters);
        void ExecuteDataSet(string sql, object parameters,Action<DataSet> action);
        [System.Obsolete("Use void  ExecuteDataTable(string sql, object parameters,Action<DataTable> action)", true)]
        DataTable ExecuteDataTable(string sql, object parameters=null);
        void ExecuteDataTable(string sql, object parameters,Action<DataTable> action);
        [System.Obsolete("Use void ExecuteSpDataSet(string procedureName, object parameters,Action<DataSet> action)", true)]
        DataSet ExecuteSpDataSet(string procedureName, object parameters);
        void ExecuteSpDataSet(string procedureName, object parameters,Action<DataSet> action);
    }
}