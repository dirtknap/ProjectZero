namespace ProjectZero.Database.Extensions
{ /*
    public interface IBaseTableAccess<T> where T : new()
    {
        string Insert(object item, SqlTransaction txn = null);
        T SelectOne(string query, Dictionary<string, object> parameters, SqlTransaction txn = null);
        List<T> SelectList(string query, Dictionary<string, object> parameters, SqlTransaction txn = null);
        List<T> SelectAll();
        void Update(object item, SqlTransaction txn = null);
        void Delete(object item, SqlTransaction txn = null);
    }

    public abstract class BaseTableAccess<T> : DataSource, IBaseTableAccess<T> where T : new()
    {
        /*
        protected readonly string schema;
        private readonly string connectionString;
        private SqlConnection con;

        protected BaseTableAccess(string connectionString, string schema = null)
        {
            this.connectionString = connectionString;
            this.schema = schema;
        }

        
        public virtual string Insert(object item, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.InsertAndReturnIdent(item, txn);
            }
        }

        public virtual T SelectOne(string query, Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ReadIntoObject<T>(query, parameters);
            }
        }

        public virtual List<T> SelectList(string query, Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ReadIntoList<T>(query, parameters, txn);
            }
        }

        public virtual List<T> SelectAll()
        {
            using (var connection = GetConnection())
            {
                return connection.ReadAll<T>();
            }
        }

        public virtual void Update(object item, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                conn.Update(item, txn);
            }
        }

        public virtual void Delete(object item, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                conn.Delete(item, txn);
            }    
        }

        protected List<T> ReadIntoList(string query, Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ReadIntoList<T>(query, parameters, txn);
            }
        }
         
        protected virtual void ExecuteSpNonQuery(string sproc, Dictionary<string, object> parameters, SqlTransaction txn)
        {
            using (var conn = GetConnection())
            {
                conn.ExecuteSpNonQuery(sproc, parameters, txn);
            }
        }

        protected virtual int ExecuteNonQuery(string query, Dictionary<string, object> parameters, SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ExecuteNonQuery(query, parameters, null);
            }
        }

        protected virtual string ExecuteNonQueryReturnIdent(string query, Dictionary<string, object> parameters,
            SqlTransaction txn = null)
        {
            using (var conn = GetConnection())
            {
                return conn.ExecuteNonQueryReturnIdent(query, parameters, txn);
            }          
        }
        
        
        protected SqlConnection GetConnection()
        {
            return GetConnection(connectionString);
        }
    }
    */
}
