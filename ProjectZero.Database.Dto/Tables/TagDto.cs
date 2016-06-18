using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dto.Tables
{
    [Table("Tags")]
    public class TagDto : DbTable
    {
        [TableField("Id", IsPk = true, IsIdentity = true)]
        public int Id { get; set; }

        [TableField("Text")]
        public string Text { get; set; }

    }
}
