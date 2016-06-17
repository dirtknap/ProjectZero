using System;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dto.Tables
{
    [Table("Articles")]
    public class ArticleDto
    {
        [TableField("Id", IsPk = true, IsIdentity = true)]
        public int Id { get; set; }
        
        [TableField("Name")]
        public string Name { get; set; }
        
        [TableField("Author")]
        public Guid Author { get; set; }
        
        [TableField("Published")]
        public DateTimeOffset Published { get; set; }
        
        [TableField("LastEdited")]
        public  DateTimeOffset LastEdited { get; set; }

        [TableField("Teaser")]
        public string Teaser { get; set; }

        [TableField("Active")]
        public bool Active { get; set; }


    }
}