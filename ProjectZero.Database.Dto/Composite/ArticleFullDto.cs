using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dto.Composite
{
    public class ArticleFullDto : ArticleTeaserDto
    {
        [TableField("Text")]
        public string Text { get; set; }
    }
}
