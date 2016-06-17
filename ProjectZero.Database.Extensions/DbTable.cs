using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProjectZero.Database.Extensions
{
    public class DbTable
    {
        #region Virtual Methods 

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

        public virtual string BuildSelectRowQuery(int id, out Dictionary<string, object> queryParams)
        {
            return DoBuildSelectQuery(this, new List<int> {id}, null, out queryParams);
        }

        public virtual string BuildSelectAllRowsQuery(out Dictionary<string, object> queryParams)
        {
            return DoBuildSelectQuery(this, new List<int>(), null, out queryParams);
        }

        public virtual string BuildSelectedRowsQuery(List<int> ids, out Dictionary<string, object> queryParams)
        {
            return DoBuildSelectQuery(this, ids, null, out queryParams);
        }

        public virtual string BuildSelectTopNRowsQuery(int topN, out Dictionary<string, object> queryParams)
        {
            return DoBuildSelectQuery(this, new List<int>(), topN, out queryParams);
        }

        #endregion




        #region Public Methods

        /// <summary>
        /// Create query string for inserting DTOs that use the Table and TableField attributes into a SQL database.
        /// </summary>
        /// <param name="targetObj">Object to insert.</param>
        /// <param name="queryParams">SQL parameters for parameterized queries.</param>
        /// <param name="schema">Schema prefix, if required.</param>
        /// <returns></returns>
        public static string DoBuildInsertQuery(object targetObj, out Dictionary<string, object> queryParams,
            string schema = null)
        {
            string fieldList;
            string paramList;
            var tableName = GetTableName(targetObj.GetType(), schema);
            BuildInsertFieldsAndValues(targetObj, out queryParams, out fieldList, out paramList);

            return
                $"INSERT INTO {(string.IsNullOrEmpty(schema) ? "" : $"[{schema}.]")}{tableName} ({fieldList}) VALUES ({paramList})";
        }

        /// <summary>
        /// Create query string for selecting DTOs that use the Table and TableField attributes from a SQL database.
        /// </summary>
        /// <param name="targetObj">Object to select.</param>
        /// <param name="ids">DB identities to select. Empty list signifies SELECT ALL. List is ignored when selecting TOP N.</param>
        /// <param name="top">Value when selecting TOP N results, otherwise pass null.</param>
        /// <param name="queryParams">SQL parameters for parameterized queries.</param>
        /// <param name="schema">Schema prefix, if required.</param>
        /// <returns></returns>
        public static string DoBuildSelectQuery(object targetObj, List<int> ids, int? top,
            out Dictionary<string, object> queryParams, string schema = null)
        {
            string identityColumn;
            queryParams = new Dictionary<string, object>();
            var tableName = GetTableName(targetObj.GetType(), schema);
            var fieldList = BuildFieldList(targetObj.GetType(), out identityColumn);

            if (top != null && top > 0)
            {
                queryParams = new Dictionary<string, object>();
                return $"SELECT TOP {top} {fieldList} FROM {tableName} ORDER BY {identityColumn} DESC";
            }

            if (ids.Count == 0)
            {
                queryParams = new Dictionary<string, object>();
                return $"SELECT {fieldList} FROM {tableName}";
            }

            if (ids.Count == 1)
            {
                queryParams = new Dictionary<string, object> {{"@Id", ids[0]}};
                return $"SELECT {fieldList} FROM {tableName} WHERE {identityColumn} = @Id";
            }

            var paramCount = 1;

            foreach (var id in ids)
            {
                queryParams[$"@Id_{paramCount}"] = id;
                paramCount++;
            }

            return
                $"SELECT {fieldList} FROM {tableName} WHERE {identityColumn} IN ({string.Join(",", queryParams.Keys.ToList())})";
        }

        /// <summary>
        /// Create query string for updating DTOs that use the Table and TableField attributes in a SQL database.
        /// </summary>
        /// <param name="targetObj">Object to update</param>
        /// <param name="queryParams">SQL parameters for parameterized queries.</param>
        /// /// <param name="schema">Schema prefix, if required.</param>
        /// <returns></returns>
        public static string DoBuildUpdateQuery(object targetObj, out Dictionary<string, object> queryParams,
            string schema = null)
        {
            queryParams = new Dictionary<string, object>();
            var modList = new StringBuilder();
            var whereClause = new StringBuilder();

            string tableName = GetTableName(targetObj.GetType(), schema);
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
                    whereClause.Append($"[{fieldName}]={paramName}");
                }
                else
                {
                    modList.Append($"[{fieldName}]={paramName},");
                }

            }

            if (whereClause.Length == 0)
            {
                throw new DbTableException($"No primary key fields specified in table {tableName}");
            }

            return $"UPDATE {tableName} SET {modList.ToString().TrimEnd(',')} WHERE {whereClause}";
        }

        /// <summary>
        /// Create query string for deleting DTOs that use the Table and TableField attributes from a SQL database.
        /// </summary>
        /// <param name="targetObj">Object to update</param>
        /// <param name="queryParams">SQL parameters for parameterized queries.</param>
        /// /// <param name="schema">Schema prefix, if required.</param>
        /// <returns></returns>
        public static string DoBuildDeleteQuery(object targetObj, out Dictionary<string, object> queryParams,
            string schema = null)
        {
            queryParams = new Dictionary<string, object>();

            var tableName = GetTableName(targetObj.GetType(), schema);
            var whereClause = BuildPkWhereClause(targetObj, queryParams);

            if (whereClause.Length == 0)
            {
                throw new DbTableException(String.Format("No primary key fields specified in table {0}", tableName));
            }
            return $"DELETE FROM {tableName} WHERE {whereClause}";
        }


        /// <summary>
        /// Get the Table Name for DTOs that use the Table and TableField attributes 
        /// </summary>
        /// <param name="targetObj">DTO object</param>
        /// /// <param name="schema">Schema prefix, if required.</param>
        /// <returns></returns>
        public static string GetTableName(object targetObj, string schema = null)
        {
            var targetType = targetObj.GetType();
            return GetTableName(targetType, schema);
        }

        /// <summary>
        /// Get the Table Name for DTOs that use the Table and TableField attributes 
        /// </summary>
        /// <param name="targetType">DTO object</param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static string GetTableName(Type targetType, string schema = null)
        {
            var attributes = targetType.GetCustomAttributes(typeof (TableAttribute), true) as TableAttribute[];
            if (attributes == null || attributes.Length != 1)
            {
                throw new DbTableException("GetTableName requires the class to have a [Table] attribute");
            }

            if (string.IsNullOrEmpty(schema))
            {
                return $"[{attributes[0].TableName}]";
            }

            return $"[{schema}].[{attributes[0].TableName}]";
        }

        /// <summary>
        /// Build a Field lost for a DTOs that use the Table and TableField attributes 
        /// </summary>
        /// <param name="targetType">DTO type</param>
        /// <param name="prefix">prefix if needed, such as table alias in a join statement</param>
        /// <returns></returns>
        public static string BuildFieldList(Type targetType, string prefix = "")
        {
            var fields = new List<string>();
            var propertyInfoList = targetType.GetProperties();
            foreach (var propertyInfo in propertyInfoList)
            {
                var propAtts =
                    propertyInfo.GetCustomAttributes(typeof (TableFieldAttribute), true) as TableFieldAttribute[];

                if (propAtts == null || propAtts.Length == 0)
                {
                    continue;
                }

                fields.Add(propAtts[0].FieldName);
            }

            var sb = new StringBuilder();

            foreach (var field in fields)
            {
                sb.Append($"{(string.IsNullOrEmpty(prefix) ? "" : $"[{prefix}].")}[{field}],");
            }

            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// Build a Field lost for a DTOs that use the Table and TableField attributes.
        /// This method is overloaded to also provide the DTO's Identity column 
        /// </summary>
        /// <param name="targetType">DTO type</param>
        /// <param name="prefix">prefix if needed, such as table alias in a join statement</param>
        /// <returns></returns>
        public static string BuildFieldList(Type targetType, out string identityColumn, string prefix = "")
        {
            identityColumn = string.Empty;

            var fields = new List<string>();
            var propertyInfoList = targetType.GetProperties();
            foreach (var propertyInfo in propertyInfoList)
            {
                var propAtts =
                    propertyInfo.GetCustomAttributes(typeof (TableFieldAttribute), true) as TableFieldAttribute[];

                if (propAtts == null || propAtts.Length == 0)
                {
                    continue;
                }

                fields.Add(propAtts[0].FieldName);

                if (propAtts[0].IsIdentity)
                {
                    identityColumn = $"[{propAtts[0].FieldName}]";
                }
            }

            if (string.IsNullOrEmpty(identityColumn))
            {
                throw new DbTableException(
                    "GetIdentityField requires the class to have a [TableField] attribute set as Identity");
            }

            var sb = new StringBuilder();

            foreach (var field in fields)
            {
                sb.Append($"{(string.IsNullOrEmpty(prefix) ? "" : $"[{prefix}].")}[{field}],");
            }

            return sb.ToString().TrimEnd(',');
        }

        #endregion




        #region Private Methods

        private static void BuildInsertFieldsAndValues(object targetObj, out Dictionary<string, object> queryParams,
            out string fieldList,
            out string paramList)
        {
            queryParams = new Dictionary<string, object>();
            var fieldBuilder = new StringBuilder();
            var paramBuilder = new StringBuilder();

            var propertyInfoList = targetObj.GetType().GetProperties();

            foreach (var propertyInfo in propertyInfoList)
            {
                var propAttr =
                    propertyInfo.GetCustomAttributes(typeof (TableFieldAttribute), true) as TableFieldAttribute[];

                if (propAttr == null || propAttr.Length == 0)
                {
                    continue;
                }

                if (propAttr[0].IsIdentity)
                {
                    continue;
                }

                var paramName = $"@{propertyInfo.Name},";
                paramBuilder.Append(paramName);
                fieldBuilder.Append($"[{propAttr[0].FieldName}],");

                var value = propertyInfo.GetValue(targetObj, null);

                if (propertyInfo.PropertyType == typeof (Guid) && propAttr[0].IsPk)
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

            fieldList = fieldBuilder.ToString().TrimEnd(',');
            paramList = paramBuilder.ToString().TrimEnd(',');
        }

        private static string BuildPkWhereClause(object targetObj, Dictionary<string, object> queryParams)
        {
            var whereClause = new StringBuilder();
            var propertyInfoList = targetObj.GetType().GetProperties();

            var paramCounter = 0;
            foreach (var propertyInfo in propertyInfoList)
            {
                var propAtts = propertyInfo.GetCustomAttributes(
                    typeof (TableFieldAttribute), true) as TableFieldAttribute[];

                if (propAtts == null || propAtts.Length == 0)
                {
                    continue;
                }

                if (!propAtts[0].IsPk)
                {
                    continue;
                }

                var paramName = $"@{propertyInfo.Name}";
                var value = propertyInfo.GetValue(targetObj, null);
                queryParams[paramName] = value;
                paramCounter++;

                // Add the field name and the parameter name to the where clause
                var fieldName = propAtts[0].FieldName;
                if (whereClause.Length > 0)
                {
                    whereClause.Append(" AND ");
                }
                whereClause.Append($"[{fieldName}]={paramName},");
            }
            return whereClause.ToString().TrimEnd(',');
        }
    }

    #endregion

    [Serializable]
    public class DbTableException : Exception
    {
        public DbTableException()
        {
        }

        public DbTableException(string message) : base(message)
        {
        }

        public DbTableException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DbTableException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}