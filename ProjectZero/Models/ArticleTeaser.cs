using System;
using System.Collections.Generic;
using ProjectZero.Database.Dto.Tables;

namespace ProjectZero.Models
{
    public class ArticleTeaser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Teaser { get; set; }
        public List<string> Tags { get; set; }
        public DateTimeOffset Published { get; set; }
        public DateTimeOffset LastEdited { get; set; }

        public ArticleTeaser(ArticleDto article, List<string> tags)
        {
            Id = article.Id;
            Name = article.Name;
            Teaser = article.Teaser;
            Published = article.Published;
            LastEdited = article.LastEdited;
            
        }

    }
}
