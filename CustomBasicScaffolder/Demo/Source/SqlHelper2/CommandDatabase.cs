using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SqlHelper2
{
    public class CommandDatabase : IDatabase
    {
        public readonly DbCommand Command;

        public readonly DbDataAdapter dataAdapter;
        public CommandDatabase(DbCommand cmd, DbDataAdapter adapter)
        {
            dataAdapter = adapter;
            Command = cmd;
        }

        public CommandDatabase(DbCommand cmd)
        {
            Command = cmd;
        }

        private void PrepareCommand(string sql, object parameters)
        {
            Command.CommandType = CommandType.Text;
            Command.CommandText = sql;
            Command.SetParameters(parameters);
        }

        public int ExecuteNonQuery(string sql, object parameters)
        {
            PrepareCommand(sql, parameters);

            return Command.ExecuteNonQuery();
        }

        public IEnumerable<T> ExecuteDataReader<T>(string sql, object parameters, Func<DbDataReader, T> action)
        {
            PrepareCommand(sql, parameters);

            using (var dr = Command.ExecuteReader())
            {
                while (dr.Read())
                    yield return action.Invoke(dr);
            }
        }

        public void ExecuteDataReader(string sql, object parameters, Action<DbDataReader> action)
        {
            PrepareCommand(sql, parameters);

            using (var dr = Command.ExecuteReader())
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
            var result = Command.ExecuteScalar();
            return (result != null) ? (T)result : default(T);
        }

        public void BulkCopy(DataTable table, int batchSize)
        {
            using (var bulkcopy = new SqlBulkCopy((SqlConnection)Command.Connection))
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
            Command.CommandType = CommandType.Text;
            Command.CommandText = sql;
            Command.SetParameters(parameters);

            using (var dr = Command.ExecuteReader())
            {
                return dr.HasRows;
            }
        }

        public DataSet ExecuteDataSet(string sql, object parameters)
        {
            PrepareCommand(sql, parameters);

            dataAdapter.SelectCommand = Command;

            var ds = new DataSet();
            dataAdapter.Fill(ds);
            return ds;
        }

        public DataTable ExecuteDataTable(string sql, object parameters = null)
        {
            PrepareCommand(sql, parameters);
            dataAdapter.SelectCommand = Command;
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            return (ds.Tables.Count >= 0 ? ds.Tables[0] : null);
        }

        public DataSet ExecuteSpDataSet(string procedureName, object parameters)
        {
            PrepareCommand(procedureName, parameters);
            Command.CommandText = procedureName;
            Command.CommandType = CommandType.StoredProcedure;
            dataAdapter.SelectCommand = Command;
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            return ds;
        }

        public int ExecuteSPNonQuery(string procedureName, object parameters = null)
        {
            PrepareCommand(procedureName, parameters);
            Command.CommandType = CommandType.StoredProcedure;
            return Command.ExecuteNonQuery();
        }

        public int ExecuteNonQuery(string sql, IEnumerable<object> parameters = null)
        {
            var result = 0;
            foreach (var parameter in parameters)
            {
                PrepareCommand(sql, parameter);
                Command.CommandText = sql;
                result += Command.ExecuteNonQuery();
            }
            return result;
        }

        public int ExecuteNonQuery(IEnumerable<string> sqllist)
        {
            var result = 0;
            foreach (var sql in sqllist)
            {
                Command.CommandText = sql;
                result += Command.ExecuteNonQuery();
            }
            return result;
        }
    }
}