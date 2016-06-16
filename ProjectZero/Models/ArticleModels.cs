using System;
using System.Collections.Generic;
using ProjectZero.Database.Dto.Tables;

namespace ProjectZero.Models
{
    public class ArticleModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Teaser { get; set; }
        public string Text { get; set; }
        public List<string> Tags { get; set; }
        public DateTimeOffset Published { get; set; }
        public DateTimeOffset LastEdited { get; set; }
    }
}
