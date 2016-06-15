using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace ProjectZero.Database.Extensions
{
    public static class SqlConnectionExtensions
    {
        public static int CommandTimeout { get; set; }

        public static List<T> ReadAll<T>(this SqlConnection conn, SqlTransaction transaction = null) where T : new()
        {
            List<T> results;
            SqlDataReader reader = null;
            var fields = DbTable.BuildFieldList(typeof(T));
            var fieldList = string.Join(",", fields);
            var table = DbTable.GetTableName(typeof(T));
            var query = $"SELECT {fieldList} FROM {table}";
            try
            {
                reader = conn.GetReader(query, null, transaction);
                results = reader.ReflectRows<T>();
            }
            finally
            {
                DisposeIfNotNull(reader);
            }
            results = results ?? new List<T>();
            return results;
        }

        public static bool ReadIntoObject<T>(this SqlConnection conn, string query,
            Dictionary<string, object> parameters, out T result, SqlTransaction txn = null) where T: new()
        {    
            SqlDataReader reader = null;
            var wasRead = false;
            result = default(T);
            try
            {
                reader = conn.GetReader(query, parameters, txn);
                wasRead = reader.ReflectRow<T>(out result);
            }
            finally
            {
                DisposeIfNotNull(reader);
            }
            return wasRead;
        }

        public static string InsertAndReturnIdent(this SqlConnection conn, object obj, SqlTransaction txn = null)
        {
            Dictionary<string, object> queryParams;
            var query = DbTable.DoBuildInsertQuery(obj, out queryParams);
            return conn.ExecuteNonQueryReturnIdent(query, queryParams, txn);
        }

        public static int ExecuteNonQuery(this SqlConnection conn, string query, Dictionary<string, object> parameters,
            SqlTransaction txn = null)
        {
            using (var command = BuildCommand(conn, query, parameters, txn))
            {
                return command.ExecuteNonQuery();
            }
        }

        public static string ExecuteNonQueryReturnIdent(this SqlConnection conn, string query, Dictionary<string, object> parameters, SqlTransaction txn)
        {
            using (var command = BuildCommand(conn, query, parameters, txn))
            {
                command.ExecuteNonQuery();
                return ReadOne(conn, "SELECT @@IDENTITY as \"Ident\"", null, txn);
            }
        }

        public static string ReadOne(this SqlConnection conn, string query, Dictionary<string, object> parameters,
            SqlTransaction txn)
        {
            string result = "";
            SqlDataReader reader = null;
            try
            {
                reader = conn.GetReader(query, parameters, txn);
                result = reader.ReadOne();
            }
            finally
            {
                DisposeIfNotNull(reader);
            }
            return result;
        }


        public static SqlDataReader GetReader(this SqlConnection conn, string query,
            Dictionary<string, object> parameters, SqlTransaction transaction = null)
        {
            using (var command = BuildCommand(conn, query, parameters, transaction))
            {
                SqlDataReader reader;
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return reader;
            }
        }

        private static SqlCommand BuildCommand(SqlConnection conn, string query, Dictionary<string, object> parameters,
            SqlTransaction transaction)
        {
            SqlCommand command = conn.CreateCommand();
            if (CommandTimeout > 0)
            {
                command.CommandTimeout = CommandTimeout;
            }

            command.CommandText = query;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            if (parameters != null)
            {
                foreach (var param in parameters.Keys)
                {
                    SqlParameter sqlParameter = new SqlParameter(param, parameters[param] ?? DBNull.Value);
                    command.Parameters.Add(sqlParameter);
                }
            }
            return command;
        }

        private static void DisposeIfNotNull(IDisposable disposable)
        {
            disposable?.Dispose();
        }

    }
}