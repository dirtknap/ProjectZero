using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace ProjectZero.Database.Extensions
{
    public static class SqlReaderExtensions
    {

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

        public static bool ReflectRow<T>(this SqlDataReader reader, out T result) where T : new()
        {
            var target = typeof(T);
            var fieldToMemberMap = ReflectType(target);
            if (reader.Read())
            {
                T newT = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.IsDBNull(i)) continue;
                    var name = reader.GetName(i);
                    object value = reader.GetValue(i);

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
