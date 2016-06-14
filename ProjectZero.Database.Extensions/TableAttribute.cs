using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZero.Database.Extensions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public readonly string TableName;

        public DbVersion Version { get; set; }

        public TableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
