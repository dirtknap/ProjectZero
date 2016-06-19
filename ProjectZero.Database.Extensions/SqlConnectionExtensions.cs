using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ProjectZero.Database.Extensions
{
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Modify the SQL Command Timeout
        /// </summary>
        public static int CommandTimeout { get; set; }

        /// <summary>
        /// Insert an object that uses the Table and TableField attributes into a SQL database
        /// </summary>
        /// <param name="conn">SQL connnection</param>
        /// <param name="obj">Object to insert</param>
        /// <param name="txn">SQL transaction if required</param>
        /// <returns>Identity column value in string format</returns>
        public static string InsertAndReturnIdent(this SqlConnection conn, object obj, SqlTransaction txn = null)
        {
            Dictionary<string, object> queryParams;
            var query = DbTable.DoBuildInsertQuery(obj, out queryParams);
            return conn.ExecuteNonQueryReturnIdent(query, queryParams, txn);
        }

        /// <summary>
        /// Read and return an object that uses Table and TableField attributes from a SQL database 
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="conn">SQL connection</param>
        /// <param name="query">SQL query</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="txn">Transaction if required</param>
        /// <returns>object from database</returns>
        public static T ReadIntoObject<T>(this SqlConnection conn, string query, Dictionary<string, object> parameters,
            SqlTransaction txn = null) where T : new()
        {
            var item = default(T);
            using (var reader = conn.GetReader(query, parameters, txn))
            {
                reader.ReflectRow<T>(out item);
            }
            return item;
        }

        /// <summary>
        /// Read and return a list of selected objects that use the Table and TableField attribute from a SQL database 
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="conn">SQL connection</param>
        /// <param name="query">SQL query</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="txn">Transaction if required</param>
        /// <returns>Seleceted list of objects</returns>
        public static List<T> ReadIntoList<T>(this SqlConnection conn, string query, Dictionary<string, object> parameters,
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

        /// <summary>
        /// Read and return a list of all objects that use the Table and TableField in a SQL database table
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="conn">SQL connection</param>
        /// <param name="txn">Transaction if required</param>
        /// <returns>List of all objects in table</returns>
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

        /// <summary>
        /// Update the database entry for an object that uses the Table and TableField attributes
        /// </summary>
        /// <param name="conn">SQL connnection</param>
        /// <param name="obj">Object to update</param>
        /// <param name="txn">SQL transaction if required</param>
        public static void Update(this SqlConnection conn, object item, SqlTransaction txn = null)
        {
            Dictionary<string, object> parameters;
            var query = DbTable.DoBuildUpdateQuery(item, out parameters);
            conn.ExecuteNonQuery(query, parameters, txn);
        }

        /// <summary>
        /// Delete the database entry for an object that uses the Table and TableField attributes
        /// </summary>
        /// <param name="conn">SQL connnection</param>
        /// <param name="obj">Object to delete</param>
        /// <param name="txn">SQL transaction if required</param>
        public static void Delete(this SqlConnection conn, object item, SqlTransaction txn = null)
        {
            Dictionary<string, object> parameters;
            var query = DbTable.DoBuildDeleteQuery(item, out parameters);
            conn.ExecuteNonQuery(query, parameters, txn);
        }

        /// <summary>
        /// Execute a SQL Query.  If the query returns a integer value Identity, that value will be returned
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="query">SQL query</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="txn">Transaction if required</param>
        /// <returns>Integer retrun value from query</returns>
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

        /// <summary>
        /// Execute SQL query and return @@IDENTITY from the action
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="query">SQL query</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="txn">Transaction if required</param>
        /// <returns>SQL @@IDENTITY for query</returns>
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

        /// <summary>
        /// Run stored procedure in SQL database
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="sproc">name of stored procedure</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="txn">Transaction if required</param>
        public static void ExecuteSpNonQuery(this SqlConnection conn, string sproc,
            Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            var command = BuildCommand(conn, sproc, parameters, txn);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            txn?.Commit();
        }

        /// <summary>
        /// Run stored procedure in SQL database and return a value select in the procedure
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="sproc">name of stored procedure</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="txn">Transaction if required</param>
        /// <returns>string value selected in stored procedure</returns>
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

        /// <summary>
        /// Get first result from the associate SQL Reader of a SQL command
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="query">SQL query</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="txn">Transaction if required</param>
        /// <returns></returns>
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

        /// <summary>
        /// Construct SQL Command and return the associated reader
        /// </summary>
        /// <param name="conn">SQL connection</param>
        /// <param name="query">SQL query</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="txn">Transaction if required</param>
        /// <returns></returns>
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