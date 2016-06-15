using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectZero.Database.Extensions
{
    public class DbTable
    {
        public virtual string BuildInsertQuery(out Dictionary<string, object> queryParams)
        {
            return DoBuildInsertQuery(this, out queryParams);
        }

        public virtual string BuildUpdateQuery(out Dictionary<string, object> queryParams)
        {
            return DoBuildUpdateQuery(this, out queryParams);
        }

        public virtual string BuildDeleteQuery(out Dictionary<string, object> queryParams)
        {
            return DoBuildDeleteQuery(this, out queryParams);
        }

        public static string DoBuildDeleteQuery(object targetObj, out Dictionary<string, object> queryParams)
        {
            queryParams = new Dictionary<string, object>();

            string tableName = GetTableName(targetObj);
            var whereClause = BuildPkWhereClause(targetObj, queryParams);

            if (whereClause.Length == 0)
            {
                throw new DbTableException(String.Format("No primary key fields specified in table {0}", tableName));
            }
            string queryString = String.Format("DELETE FROM {0} WHERE {1}", tableName, whereClause);
            return queryString;
        }

        public static string GetTableName(object targetObj)
        {
            var targetType = targetObj.GetType();
            return GetTableName(targetType);
        }

        public static string GetTableName(Type targetType)
        {
            var attributes = targetType.GetCustomAttributes(typeof(TableAttribute), true) as TableAttribute[];
            if (attributes == null || attributes.Length != 1)
            {
                throw new DbTableException("BuildInsertQuery requires the class to have a [Table] attribute");
            }

            return attributes[0].TableName;
        }

        public static List<string> BuildFieldList(Type targetType)
        {
            var fields = new List<string>();
            var propertyInfoList = targetType.GetProperties();
            foreach (var propertyInfo in propertyInfoList)
            {
                var propAtts = propertyInfo.GetCustomAttributes(
                    typeof(TableFieldAttribute), true) as TableFieldAttribute[];

                // Skip properties that aren't declared with [TableField]
                if (propAtts.Length == 0)
                {
                    continue;
                }

                fields.Add(propAtts[0].FieldName);
            }
            return fields;
        }

        internal static string DoBuildUpdateQuery(object targetObj, out Dictionary<string, object> queryParams)
        {
            queryParams = new Dictionary<string, object>();  
            var modList = new StringBuilder();
            var whereClause = new StringBuilder();

            string tableName = GetTableName(targetObj);
            var propertyInfoList = targetObj.GetType().GetProperties();

            foreach (var propertyInfo in propertyInfoList)
            {
                var propAttributes =
                    propertyInfo.GetCustomAttributes(typeof (TableFieldAttribute), true) as TableFieldAttribute[];

                if (propAttributes.Length == 0)
                {
                    continue;
                }

                var paramName = $"@{propertyInfo.Name}";
                var value = propertyInfo.GetValue(targetObj, null);
                queryParams[paramName] = value;

                var fieldName = propAttributes[0].FieldName;

                if (propAttributes[0].IsPk)
                {
                    if (whereClause.Length > 0)
                    {
                        whereClause.Append(" AND ");
                    }
                    whereClause.Append($"{fieldName}={paramName}");
                }
                else
                {
                    if (modList.Length > 0)
                    {
                        modList.Append(", ");
                    }
                    modList.Append($"{fieldName}={paramName}");
                }

            }

            if (whereClause.Length == 0)
            {
                throw new DbTableException($"No primary key fields specified in table {tableName}");
            }

            return $"UPDATE {tableName} SET {modList} WHERE {whereClause}";
        }


        internal static string DoBuildInsertQuery(object targetObj, out Dictionary<string, object> queryParams,
            string schema = null)
        {
            StringBuilder fieldList;
            StringBuilder paramList;
            string tableName;
            BuildInsertFieldsAndValues(targetObj, out queryParams, out fieldList, out paramList, out tableName);

            if (!string.IsNullOrEmpty(schema))
            {
                tableName = $"{tableName}.{schema}";
            }

            return $"INSERT INTO {tableName} ({fieldList}) VALUES ({paramList})";
        }

        private static void BuildInsertFieldsAndValues(object targetObj, out Dictionary<string, object> queryParams, out StringBuilder fieldList, 
            out StringBuilder paramList, out string tableName)
        {
            queryParams = new Dictionary<string, object>();
            fieldList = new StringBuilder();
            paramList = new StringBuilder();

            tableName = GetTableName(targetObj);
            var propertyInfoList = targetObj.GetType().GetProperties();

            var isFirst = true;
            foreach (var propertyInfo in propertyInfoList)
            {
                var propAttributes = propertyInfo.GetCustomAttributes(typeof (TableFieldAttribute), true) as TableFieldAttribute[];

                if (propAttributes.Length == 0)
                {
                    continue;
                }

                if (propAttributes[0].IsIdentity)
                {
                    continue;
                }

                if (!isFirst)
                {
                    fieldList.Append(",");
                    paramList.Append(",");
                }
                else
                {
                    isFirst = false;
                }

                var paramName = $"@{propertyInfo.Name}";
                paramList.Append(paramName);
                fieldList.Append($"[{propAttributes[0].FieldName}]");

                var value = propertyInfo.GetValue(targetObj, null);

                if (propertyInfo.PropertyType == typeof (Guid) && propAttributes[0].IsPk)
                {
                    var vGuid = (Guid) value;
                    if (vGuid == Guid.Empty)
                    {
                        value = Guid.NewGuid();
                    }
                    propertyInfo.SetValue(targetObj, value, null);
                }

                queryParams[paramName] = value ?? DBNull.Value;
            }


        }

        private static StringBuilder BuildPkWhereClause(object targetObj, Dictionary<string, object> queryParams)
        {
            var whereClause = new StringBuilder();
            var propertyInfoList = targetObj.GetType().GetProperties();

            var paramCounter = 0;
            foreach (var propertyInfo in propertyInfoList)
            {
                var propAtts = propertyInfo.GetCustomAttributes(
                    typeof(TableFieldAttribute), true) as TableFieldAttribute[];

                // Skip properties that aren't declared with [TableField]
                if (propAtts == null || propAtts.Length == 0)
                {
                    continue;
                }

                if (!propAtts[0].IsPk)
                {
                    continue;
                }

                // Pick an arbitrary parameter name
                var paramName = "@" + propertyInfo.Name;
                var value = propertyInfo.GetValue(targetObj, null);
                queryParams[paramName] = value;
                paramCounter++;

                // Add the field name and the parameter name to the where clause
                var fieldName = propAtts[0].FieldName;
                if (whereClause.Length > 0)
                {
                    whereClause.Append(" AND ");
                }
                whereClause.Append(fieldName + "=" + paramName);
            }
            return whereClause;
        }
    }

    [Serializable]
    public class DbTableException : Exception
    {
        public DbTableException() { }
        public DbTableException(string message) : base(message) { }
        public DbTableException(string message, Exception inner) : base(message, inner) { }
        protected DbTableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}