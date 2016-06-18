using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Tables.Interfaces
{
    public abstract class BaseTableDal<T> : DataSource, ISimpleCrudDal<T> where T : DbTable, new ()
    {
        protected readonly string schema;
        private readonly string connectionString;

        protected BaseTableDal(string connectionString, string schema = null)
        {
            this.connectionString = connectionString;
            this.schema = schema;
        }

        public int Insert(T item, SqlTransaction txn = null)
        {
            using (var conn = GetConnection(connectionString))
            {
                var result = conn.InsertAndReturnIdent(item, txn);
                if (string.IsNullOrEmpty(result))
                {
                    return -1;
                }
                return int.Parse(result);
            }
        }

        public T Get(int id, SqlTransaction txn = null)
        {
            using (var conn = GetConnection(connectionString))
            {
                var parameters = new Dictionary<string, object>();
                var query = new T().BuildInsertQuery(out parameters);
                
                return conn.ReadIntoObject<T>(query, parameters, txn);
            }
        }

        public List<T> GetAll(SqlTransaction txn = null)
        {
            throw new NotImplementedException();
        }

        public List<T> GetSelected(List<int> idList, SqlTransaction txn = null)
        {
            throw new NotImplementedException();
        }

        public List<T> GetNewestN(int number, SqlTransaction txn = null)
        {
            throw new NotImplementedException();
        }

        public int Update(T item, SqlTransaction txn = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, SqlTransaction txn = null)
        {
            throw new NotImplementedException();
        }
    }
}
