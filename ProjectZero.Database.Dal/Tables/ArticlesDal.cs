using System.Collections.Generic;
using System.Data.SqlClient;
using ProjectZero.Database.Dal.Tables.Interfaces;
using ProjectZero.Database.Dto.Tables;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Tables
{
    public class ArticlesDal : BaseTableDal<ArticleDto>, IArticleDal
    {
        public ArticlesDal(string connectionString) : base(connectionString, null)
        {
        }

        public void SetActive(int id, bool active)
        {
            var parameters = new Dictionary<string, object> { { "@Id", id }, { "@Active", active } };

            using (var conn = GetConnection(connectionString))
            {
                conn.ExecuteNonQuery("UPDATE [Articles] SET [Active] = @Active WHERE Id = @Id", parameters);
            }
        }

        public override void Delete(int id, SqlTransaction txn = null)
        {
            var parameter = new Dictionary<string, object> { { "@Id", id } };

            using (var conn = GetConnection(connectionString))
            {
                conn.ExecuteSpNonQuery("sp_DeleteArticle", parameter);
            }
        }
    }
}
