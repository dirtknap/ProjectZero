using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dto.Composite
{
    public class ArticleTeaserDto
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
        public DateTimeOffset LastEdited { get; set; }

        [TableField("Teaser")]
        public string Teaser { get; set; }

        [TableField("Tags")]
        public string Tags { get; set; }

        public List<string> GetTags()
        {
            return Tags.Split(',').ToList();
        }

    }
}
