using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dto.Tables
{
    [Table("ArticleText")]
    public class ArticleTextDto : DbTable
    {
        [TableField("Id", IsPk = true, IsIdentity = true)]
        public int Id { get; set; }

        [TableField("ArticleId", ForeignKey = "Articles.Id")]
        public int ArticleId { get; set; }

        [TableField("Text")]
        public string Text { get; set; }
    }
}
