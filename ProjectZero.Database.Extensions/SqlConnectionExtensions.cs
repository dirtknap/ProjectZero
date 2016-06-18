using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProjectZero.Database.Extensions
{
    public static class SqlConnectionExtensions
    {
        public static int CommandTimeout { get; set; }

        public static string InsertAndReturnIdent(this SqlConnection conn, object obj, SqlTransaction txn = null)
        {
            Dictionary<string, object> queryParams;
            var query = DbTable.DoBuildInsertQuery(obj, out queryParams);
            return conn.ExecuteNonQueryReturnIdent(query, queryParams, txn);
        }

        public static T ReadIntoObject<T>(this SqlConnection conn, string query, Dictionary<string, object> parameters,
            SqlTransaction txn = null) where T : new()
        {
            T item;
            ReadIntoObject<T>(conn, query, parameters, out item, txn);
            return item;
        }

        public static bool ReadIntoObject<T>(this SqlConnection conn, string query,
            Dictionary<string, object> parameters, out T result, SqlTransaction txn = null) where T : new()
        {
            var wasRead = false;
            result = default(T);
            using (var reader = conn.GetReader(query, parameters, txn))
            {
                wasRead = reader.ReflectRow<T>(out result);
            }
            return wasRead;
        }

        public static List<T> ReadIntoList<T>(this SqlConnection conn, string query,
            Dictionary<string, object> parameters,
            SqlTransaction txn = null) where T : new()
        {
            var results = new List<T>();

            using (var reader = conn.GetReader(query, parameters, txn))
            {
                results = reader.ReflectRows<T>();
                txn?.Commit();
            }
            return results ?? new List<T>();
        }

        public static List<T> ReadAll<T>(this SqlConnection conn, SqlTransaction txn = null) where T : new()
        {
            List<T> results;

            var fields = DbTable.BuildFieldList(typeof(T));
            var fieldList = string.Join(",", fields);
            var table = DbTable.GetTableName(typeof(T));
            var query = $"SELECT {fieldList} FROM {table}";
            using (var reader = conn.GetReader(query, null, txn))
            {
                results = reader.ReflectRows<T>();
                txn?.Commit();
            }
            return results ?? new List<T>();
        }

        public static void Update(this SqlConnection conn, object item, SqlTransaction txn = null)
        {
            Dictionary<string, object> parameters;
            var query = DbTable.DoBuildUpdateQuery(item, out parameters);
            conn.ExecuteNonQuery(query, parameters, txn);
        }

        public static void Delete(this SqlConnection conn, object item, SqlTransaction txn = null)
        {
            Dictionary<string, object> parameters;
            var query = DbTable.DoBuildDeleteQuery(item, out parameters);
            conn.ExecuteNonQuery(query, parameters, txn);
        }







        public static int ExecuteNonQuery(this SqlConnection conn, string query, Dictionary<string, object> parameters,
            SqlTransaction txn = null)
        {
            using (var command = BuildCommand(conn, query, parameters, txn))
            {
                var result = command.ExecuteNonQuery();
                txn?.Commit();
                return result;
            }
        }

        public static string ExecuteNonQueryReturnIdent(this SqlConnection conn, string query,
            Dictionary<string, object> parameters, SqlTransaction txn)
        {
            using (var command = BuildCommand(conn, query, parameters, txn))
            {
                command.ExecuteNonQuery();
                var result = ReadOne(conn, "SELECT @@IDENTITY as \"Ident\"", null, txn);
                txn?.Commit();
                return result;
            }
        }

        public static void ExecuteSpNonQuery(this SqlConnection conn, string sproc,
            Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            var command = BuildCommand(conn, sproc, parameters, txn);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            txn?.Commit();
        }

        public static string ExecuteSpReadOne(this SqlConnection conn, string sproc,
            Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            var command = BuildCommand(conn, sproc, parameters, txn);
            command.CommandType = CommandType.StoredProcedure;
            using (var reader = command.ExecuteReader())
            {
                var result = reader.ReadOne();
                txn?.Commit();
                return result;
            }
        }

        public static string ReadOne(this SqlConnection conn, string query, Dictionary<string, object> parameters,
            SqlTransaction txn)
        {
            using (var reader = conn.GetReader(query, parameters, txn))
            {
                var result = reader.ReadOne();
                txn?.Commit();
                return result;
            }
        }


        public static SqlDataReader GetReader(this SqlConnection conn, string query,
            Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            using (var command = BuildCommand(conn, query, parameters, txn))
            {
                SqlDataReader reader;
                try
                {
                    reader = command.ExecuteReader();
                    txn?.Commit();
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
    }
}