using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectZero.Database.Extensions.Utils;

namespace ProjectZero.Database.Extensions
{
    public class DataSource
    {
        // to prevent spamming the DB should there be an issue
        protected bool offline = true;
        private CircuitBreaker cBreaker = new CircuitBreaker(1 * 60 * 1000);

        protected DataSource()
        {
            
        }

        protected SqlConnection GetConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                if (!cBreaker.IsBroken)
                {
                    connection.Open();
                    offline = false;
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is SqlException)
                {
                    //TODO: Add Logging
                    offline = true;
                    cBreaker.Break();
                }
                else
                {
                    throw ex;
                }
            }
            return connection;
        }
    }
}
