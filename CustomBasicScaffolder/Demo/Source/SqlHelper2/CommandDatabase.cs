using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SqlHelper2
{
    public class CommandDatabase : IDatabase
    {
        public readonly DbCommand command;

        public readonly DbDataAdapter dataAdapter;
        public CommandDatabase(DbCommand cmd, DbDataAdapter adapter)
        {
            dataAdapter = adapter;
            command = cmd;
        }

        public CommandDatabase(DbCommand cmd)
        {
            command = cmd;
        }

        private void PrepareCommand(string sql, object parameters)
        {
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            command.SetParameters(parameters);
        }

        public int ExecuteNonQuery(string sql, object parameters)
        {
            PrepareCommand(sql, parameters);

            return command.ExecuteNonQuery();
        }

        public IEnumerable<T> ExecuteDataReader<T>(string sql, object parameters, Func<DbDataReader, T> action)
        {
            PrepareCommand(sql, parameters);

            using (var dr = command.ExecuteReader())
            {
                while (dr.Read())
                    yield return action.Invoke(dr);
            }
        }

        public void ExecuteDataReader(string sql, object parameters, Action<DbDataReader> action)
        {
            PrepareCommand(sql, parameters);

            using (var dr = command.ExecuteReader())
            {
                while (dr.Read())
                    action.Invoke(dr);
            }
        }

        public void ExecuteTransaction(Action<IDatabase> action)
        {
            if (action != null)
                action.Invoke(this);
        }

        public T ExecuteScalar<T>(string sql, object parameters)
        {
            PrepareCommand(sql, parameters);
            var result = command.ExecuteScalar();
            return (result != null) ? (T)result : default(T);
        }

        public void BulkCopy(DataTable table, int batchSize)
        {
            using (var bulkcopy = new SqlBulkCopy((SqlConnection)command.Connection))
            {
                if (table != null && table.Rows.Count > 0)
                {
                    bulkcopy.DestinationTableName = table.TableName;
                    bulkcopy.BatchSize = 100;
                    bulkcopy.WriteToServer(table);
                }
            }
        }

        public bool HasRow(string sql, object parameters)
        {
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            command.SetParameters(parameters);

            using (var dr = command.ExecuteReader())
            {
                return dr.HasRows;
            }
        }
 


   

        public int ExecuteSPNonQuery(string procedureName, object parameters = null)
        {
            PrepareCommand(procedureName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            return command.ExecuteNonQuery();
        }


       

        

        public void ExecuteDataSet(string sql, object parameters, Action<DataSet> action)
        {
            PrepareCommand(sql, parameters);
            dataAdapter.SelectCommand = command;
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            action.Invoke(ds);
        }


        public void ExecuteSpDataSet(string procedureName, object parameters, Action<DataSet> action)
        {
            PrepareCommand(procedureName, parameters);
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            dataAdapter.SelectCommand = command;
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            action.Invoke(ds);
        }
    }
}