using System;
using System.Collections.Generic;

namespace ProjectZero.Models
{
    public class ArticleTeaser
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string TeaserText { get; set; }
        public List<string> Tags { get; set; }
        public DateTimeOffset Published { get; set; }
        public DateTimeOffset LastEdited { get; set; }
    }
}
