using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProjectZero.Database.Extensions
{
    public abstract class BaseTableAccess<T> : DataSource where T : new ()
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
                return connection.re
            }
            

        }

       

    }
}