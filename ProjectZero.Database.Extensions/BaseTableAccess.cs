using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectZero.Database.Extensions
{
    public abstract class BaseTableAccess<T> : DataSource where T : new()
    {
        protected readonly string schema;
        private readonly string connectionString;
        private SqlConnection con;

        protected BaseTableAccess(string connectionString, string schema = null)
        {
            this.connectionString = connectionString;
            this.schema = schema;
        }

        public virtual List<T> Get()
        {
            using (var connection = GetConnection(connectionString))
            {
                return connection.ReadAll<T>();
            }
        }

        protected T ReadIntoObject(string query, Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ReadIntoObject<T>(query, parameters);
            }
        }

        protected string Insert(object item, SqlTransaction txn = null)
        {
            string ident;
            using (var conn = GetConnection())
            {
                return ident = conn.InsertAndReturnIdent(item, txn);
            }
        }

        protected int ExecuteNonQuery(string query, Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            using (var connection = GetConnection())
            {
                return connection.ExecuteNonQuery(query, parameters, null);
            }
        }

        protected String ExecuteNonQueryReturnIdent(string query, Dictionary<string, object> parameters,
            SqlTransaction txn = null)
        {

            return txn.Connection.ExecuteNonQueryReturnIdent(query, parameters, txn);
        }



        protected SqlConnection GetConnection()
        {
            return GetConnection(connectionString);
        }
    }
}
