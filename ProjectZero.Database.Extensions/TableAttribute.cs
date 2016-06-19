using System;

namespace ProjectZero.Database.Extensions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Database table name that corresponds to this class
        /// </summary>
        public readonly string TableName;

        /// <summary>
        /// Database version that Table exists in. Not required.
        /// </summary>
        public DbVersion Version { get; set; }

        public TableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
