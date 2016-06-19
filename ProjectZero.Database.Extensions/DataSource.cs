using System;
using System.Data.SqlClient;
using ProjectZero.Database.Extensions.Utils;

namespace ProjectZero.Database.Extensions
{
    /// <summary>
    /// Provides method for getting SQL connection.  Includes circuit breaker to prevent DB spamming in
    /// the case of DB communication issues
    /// </summary>
    public class DataSource
    {
        protected bool offline = true;
        private CircuitBreaker cBreaker = new CircuitBreaker(1 * 60 * 1000);

        protected DataSource()
        {
            
        }

        /// <summary>
        /// Returns a SQL Connection using the supplied connection string
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
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
                    offline = true;
                    cBreaker.Break();
                }
                throw ex;
            }
            return connection;
        }
    }
}
