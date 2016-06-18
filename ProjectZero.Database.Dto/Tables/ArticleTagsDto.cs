using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dto.Tables
{
    [Table("ArticleTags")]
    public class ArticleTagsDto : DbTable
    {
        [TableField("Id", IsPk = true, IsIdentity = true)]
        public int Id { get; set; }

        [TableField("ArticleId", ForeignKey = "Articles.Id")]
        public int ArticleId { get; set; }

        [TableField("TagId", ForeignKey = "Tags.Id")]
        public int TagId { get; set; }
    }
}
