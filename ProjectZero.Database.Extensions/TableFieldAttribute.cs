using System;

namespace ProjectZero.Database.Extensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableFieldAttribute : Attribute
    {
        /// <summary>
        /// Database Field name the corresponds to this property
        /// </summary>
        public readonly string FieldName;
        
        /// <summary>
        /// Field is primary key
        /// </summary>
        public bool IsPk { get; set; }

        /// <summary>
        /// Field is identity
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Field is foriegn key constraint
        /// </summary>
        public bool IsFk { get { return !string.IsNullOrWhiteSpace(ForeignKey); } }

        /// <summary>
        /// Value of foriegn key constraint
        /// </summary>
        public string ForeignKey { get; set; }

        public TableFieldAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }
    }
}