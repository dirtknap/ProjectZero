using ProjectZero.Database.Dto.Tables;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dto.Composite
{
    public class ArticleFullDto : ArticleTeaserDto
    {
        [TableField("Text")]
        public string Text { get; set; }

        public ArticleDto GetArticleDto()
        {
            return new ArticleDto
            {
                Active = Active,
                Author = Author,
                Id = Id,
                LastEdited = LastEdited,
                Name = Name,
                Published = Published,
                Teaser = Teaser
            };
        }
    }
}
