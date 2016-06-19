using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace ProjectZero.Database.Extensions
{
    public static class SqlReaderExtensions
    {
        /// <summary>
        /// Read first value from SQL data reader and return as string
        /// </summary>
        /// <param name="reader">SQL data reader</param>
        /// <returns>string value form data reader</returns>
        public static string ReadOne(this SqlDataReader reader)
        {
            string result = "";
            if (reader.HasRows)
            {
                reader.Read();
                result = reader.GetValue(0).ToString();
            }
            return result;
        }

        /// <summary>
        /// Reflect first row of SQL Data Reader into an object that uses the Table and TableField attributes
        /// </summary>
        /// <typeparam name="T">Object type in table</typeparam>
        /// <param name="reader">SQL Data Reader</param>
        /// <returns>object from data reader</returns>
        public static bool ReflectRow<T>(this SqlDataReader reader, out T result) where T : new()
        {
            var target = typeof(T);
            var fieldToMemberMap = ReflectType(target);
            if (reader.Read())
            {
                var newT = new T();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.IsDBNull(i)) continue;
                    var name = reader.GetName(i);
                    var value = reader.GetValue(i);

                    if (fieldToMemberMap.ContainsKey(name))
                    {
                        var memeber = fieldToMemberMap[name];
                        var pi = target.GetProperty(memeber.Name);
                        pi.SetValue(newT, value, null);
                    }
                }
                result = newT;
                return true;
            }
            result = default(T);
            return false;
        }

        /// <summary>
        /// Reflect rows of SQL Data Reader into a list of objects that use the Table and TableField attributes
        /// </summary>
        /// <typeparam name="T">Object type in table</typeparam>
        /// <param name="reader">SQL Data Reader</param>
        /// <returns>List of objects from data reader</returns>
        public static List<T> ReflectRows<T>(this SqlDataReader reader) where T : new()
        {
            var results = new List<T>();

            var targetType = typeof(T);

            var fieldToMemberMap = ReflectType(targetType);

            PropertyInfo[] readerIndexToMemberMap = null;

            while (reader.Read())
            {
                if (readerIndexToMemberMap == null)
                {
                    readerIndexToMemberMap = new PropertyInfo[reader.FieldCount];
                    for (var j = 0; j < reader.FieldCount; j++)
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

                var obj = new T();
                results.Add(obj);

                for (var j = 0; j < reader.FieldCount; j++)
                {
                    if (readerIndexToMemberMap[j] != null)
                    {
                        var value = reader.GetValue(j);
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
        /// Create a mapping of properties on a type that uses TableField attribute 
        /// associated with them.
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns>Dictionary of DB Field names as keys and the property as value</returns>
        public static Dictionary<string, MemberInfo> ReflectType(Type targetType)
        {
            var fieldToMemberMap = new Dictionary<string, MemberInfo>();
            var members = targetType.GetMembers();
            foreach (var member in members)
            {
                var tableFieldAttributes = member.GetCustomAttributes(typeof(TableFieldAttribute), true);
                if (tableFieldAttributes.Length > 0)
                {
                    var attribute = tableFieldAttributes[0] as TableFieldAttribute;
                    fieldToMemberMap[attribute.FieldName] = member;
                }
            }
            return fieldToMemberMap;
        }
    }

}
