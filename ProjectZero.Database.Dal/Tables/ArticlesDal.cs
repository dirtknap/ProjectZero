using ProjectZero.Database.Dto.Tables;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Tables
{
    public class ArticlesDal : BaseTableAccess<ArticleDto>, IArticlesDal
    {
        public ArticlesDal(string connectionString, string schema = null) : base(connectionString, schema)
        {
        }
    }
}
