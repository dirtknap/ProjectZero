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

        public string Insert(object item, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.InsertAndReturnIdent(item, txn);
            }
        }

        public T SelectOne(string query, Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ReadIntoObject<T>(query, parameters);
            }
        }

        public List<T> SelectList(string query, Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ReadIntoList<T>(query, parameters, txn);
            }
        }

        public virtual List<T> SelectAll()
        {
            using (var connection = GetConnection(connectionString))
            {
                return connection.ReadAll<T>();
            }
        }

        public void Update(object item, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                conn.Update(item, txn);
            }
        }

        public void Delete(object item, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                conn.Delete(item, txn);
            }    
        }

        protected void ExecuteSpNonQuery(string sproc, Dictionary<string, object> parameters, SqlTransaction txn)
        {
            using (var conn = GetConnection())
            {
                conn.ExecuteSpNonQuery(sproc, parameters, txn);
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
