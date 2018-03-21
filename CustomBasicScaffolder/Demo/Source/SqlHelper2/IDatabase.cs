using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace SqlHelper2 {
    public interface IDatabase {
        int ExecuteNonQuery(string sql, object parameters = null);
    
        int ExecuteSPNonQuery(string procedureName, object parameters = null);
    
        IEnumerable<T> ExecuteDataReader<T>(string sql, object parameters, Func<DbDataReader, T> action);

        void ExecuteDataReader(string sql, object parameters, Action<DbDataReader> action);

        void ExecuteTransaction(Action<IDatabase> action);

        T ExecuteScalar<T>(string sql, object parameters = null);

        void BulkCopy(DataTable table, int batchSize = 100);

        bool HasRow(string sql, object parameters = null);
     
        void ExecuteDataSet(string sql, object parameters,Action<DataSet> action);
      
        
        void ExecuteSpDataSet(string procedureName, object parameters,Action<DataSet> action);
    }
}