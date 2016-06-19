using System.Collections.Generic;
using System.Data.SqlClient;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Tables.Interfaces
{
    public abstract class BaseTableDal<T> : DataSource, ISimpleCrudDal<T> where T : DbTable, new ()
    {
        protected readonly string schema;
        protected readonly string connectionString;

        protected BaseTableDal(string connectionString, string schema = null)
        {
            this.connectionString = connectionString;
            this.schema = schema;
        }

        public int Insert(T item, SqlTransaction txn = null)
        {
            using (var conn = GetConnection(connectionString))
            {
                return int.Parse(conn.InsertAndReturnIdent(item, txn));
            }
        }

        public T Get(int id, SqlTransaction txn = null)
        {
            var parameters = new Dictionary<string, object>();
            var query = new T().BuildSelectRowQuery(id, out parameters);

            using (var conn = GetConnection(connectionString))
            { 
                return conn.ReadIntoObject<T>(query, parameters, txn);
            }
        }

        public List<T> GetSelected(List<int> idList, SqlTransaction txn = null)
        {
            var parameters = new Dictionary<string, object>();
            var query = new T().BuildSelectedRowsQuery(idList, out parameters);

            using (var conn = GetConnection(connectionString))
            {
                return conn.ReadIntoList<T>(query, parameters, txn);
            }
        }

        public List<T> GetNewestN(int number, SqlTransaction txn = null)
        {
            var parameters = new Dictionary<string, object>();
            var query = new T().BuildSelectTopNRowsQuery(number, out parameters);

            using (var conn = GetConnection(connectionString))
            {
                return conn.ReadIntoList<T>(query, parameters, txn);
            }
        }

        public List<T> GetAll(SqlTransaction txn = null)
        {
            var parameters = new Dictionary<string, object>();
            var query = new T().BuildSelectAllRowsQuery(out parameters);

            using (var conn = GetConnection(connectionString))
            {
                return conn.ReadIntoList<T>(query, parameters, txn);
            }
        }

        public void Update(T item, SqlTransaction txn = null)
        {
            using (var conn = GetConnection(connectionString))
            {
                conn.Update(item, txn);
            }
        }

        public virtual void Delete(int id, SqlTransaction txn = null)
        {
            var parameters = new Dictionary<string, object>();
            var query = new T().BuildDeleteQuery(out parameters);

            using (var conn = GetConnection(connectionString))
            {
                conn.ExecuteNonQuery(query, parameters, txn);
            }
        }
    }
}
