using ProjectZero.Database.Dal.Tables.Interfaces;
using ProjectZero.Database.Dto.Tables;

namespace ProjectZero.Database.Dal.Tables
{
    public class ArticlesDal : BaseTableDal<ArticleDto>
    {
        public ArticlesDal(string connectionString, string schema = null) : base(connectionString, schema)
        {
        }
    }
}
