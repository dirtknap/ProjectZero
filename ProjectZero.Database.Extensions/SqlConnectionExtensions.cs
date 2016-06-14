using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace ProjectZero.Database.Extensions
{
    public static class SqlConnectionExtensions
    {
        public static int CommandTimeout { get; set; }

        public static List<T> ReadAll<T>(this SqlConnection conn, SqlTransaction transaction = null) where T : new()
        {
            List<T> results;
            SqlDataReader reader = null;
            var fields = DbTable.BuildFieldList(typeof (T));
            var fieldList = string.Join(",", fields);
            var table = DbTable.GetTableName(typeof (T));
            var query = $"SELECT {fieldList} FROM {table}";
            try
            {
                reader = conn.GetReader(query, null, transaction);
                results = reader.ReflectRows<T>;
            }
            finally
            {
                DisposeIfNotNull(reader);
            }
            results = results ?? new List<T>();
            return results;
        }

        public static SqlDataReader GetReader(this SqlConnection conn, string query,
            Dictionary<string, object> parameters, SqlTransaction transaction = null)
        {
            using (var command = BuildCommand(conn, query, parameters, transaction))
            {
                SqlDataReader reader;
                try
                {
                    reader = command.ExecuteReader();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return reader;
            }
        }

        private static SqlCommand BuildCommand(SqlConnection conn, string query, Dictionary<string, object> parameters, SqlTransaction transaction)
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

        public static List<T> ReflectRows<T>(this SqlDataReader reader) where T : new()
        {
            // Results of the query will be stowed here
            List<T> results = new List<T>();

            Type targetType = typeof(T);

            // Get a mapping of fields->MemberInfo based on targetType
            var fieldToMemberMap = ReflectType(targetType);

            // We stow the reader field index -> MemberInfo so we only need to 
            // loop through this once.
            PropertyInfo[] readerIndexToMemberMap = null;

            while (reader.Read())
            {

                // First tiem through, we build out member info map based on the
                // fields returned
                if (readerIndexToMemberMap == null)
                {
                    readerIndexToMemberMap = new PropertyInfo[reader.FieldCount];
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        var name = fieldToMemberMap.Where(kvp => kvp.Key.Equals(reader.GetName(j), StringComparison.InvariantCultureIgnoreCase))
                            .Select(x => x.Key).FirstOrDefault();
                        if (name != null)
                        {
                            readerIndexToMemberMap[j] = targetType.GetProperty(
                                fieldToMemberMap[name].Name);
                        }
                        else
                        {
                            readerIndexToMemberMap[j] = null;
                        }
                    }
                }

                T obj = new T();//System.Activator.CreateInstance(targetType);
                results.Add(obj);

                // Loop through the fields and assign the field value to the 
                // appropriate property of the object
                for (int j = 0; j < reader.FieldCount; j++)
                {
                    if (readerIndexToMemberMap[j] != null)
                    {
                        object value = reader.GetValue(j);
                        if (value.GetType() != typeof(DBNull))
                        {
                            readerIndexToMemberMap[j].SetValue(obj, value, null);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Create a mapping of properties on a type that have a TableFieldAttribute 
        /// associated with them.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Dictionary<string, MemberInfo> ReflectType(Type targetType)
        {
            Dictionary<string, MemberInfo> fieldToMemberMap = new Dictionary<string, MemberInfo>();
            var members = targetType.GetMembers();
            foreach (var member in members)
            {
                var tableFieldAttributes = member.GetCustomAttributes(typeof(TableFieldAttribute), true);
                if (tableFieldAttributes.Length > 0)
                {
                    TableFieldAttribute attribute = tableFieldAttributes[0] as TableFieldAttribute;
                    fieldToMemberMap[attribute.FieldName] = member;
                }
            }
            return fieldToMemberMap;
        }
    }
}