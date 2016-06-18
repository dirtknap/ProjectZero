using System.Collections.Generic;
using System.Text;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Composite.Interfaces
{
    public abstract class BaseCompositeDal<T> : DataSource where T : DbTable, new()
    {
        protected readonly string schema;
        internal readonly string connectionString;

        protected BaseCompositeDal(string connectionString, string schema = null)
        {
            this.connectionString = connectionString;
            this.schema = schema;
        }

        public T Get(int id)
        {
            var parameters = new Dictionary<string, object> { { "@Id", id } };

            using (var conn = GetConnection(connectionString))
            {
                var result = conn.ReadIntoObject<T>($"{BaseSelectQuery()} WHERE Id = @Id", parameters);
                return result ?? new T();
            }
        }

        public List<T> GetSelected(List<int> idList)
        {
            var parameters = new Dictionary<string, object>();
            var sb = new StringBuilder();
            var counter = 1;

            foreach (var id in idList)
            {
                parameters[$"@Id_{counter}"] = id;
                sb.Append($"@Id_{counter},");
                counter++;
            }

            using (var conn = GetConnection(connectionString))
            {
                var result = conn.ReadIntoList<T>($"{BaseSelectQuery()} WHERE Id IN {sb.ToString().TrimEnd(',')}", parameters);
                return result ?? new List<T>();
            }
        }

        public List<T> GetLastN(int number)
        {
            var parameters = new Dictionary<string, object>();

            using (var conn = GetConnection(connectionString))
            {
                var result = conn.ReadIntoList<T>($"{BaseSelectQuery(number)}", parameters);
                return result ?? new List<T>();
            }
        }

        public List<T> GetAll()
        {
            using (var conn = GetConnection(connectionString))
            {
                var result = conn.ReadIntoList<T>(BaseSelectQuery(), new Dictionary<string, object>());
                return result ?? new List<T>();
            }
        }

        protected abstract string BaseSelectQuery(int top = 0);
    }
}
