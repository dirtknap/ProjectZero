using System;

namespace ProjectZero.Database.Extensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableFieldAttribute : Attribute
    {
        public readonly string FieldName;
        
        // primary key
        public bool IsPk { get; set; }

        // identity field
        public bool IsIdentity { get; set; }

        // foreign key
        public bool IsFk { get { return !string.IsNullOrWhiteSpace(ForeignKey); } }

        // foreign key value
        public string ForeignKey { get; set; }

        public TableFieldAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }
    }
}