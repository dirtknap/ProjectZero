using System.Collections.Generic;
using System.Data.SqlClient;
using ProjectZero.Database.Dal.Tables.Interfaces;
using ProjectZero.Database.Dto.Tables;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Tables
{
    public class TagsDal : BaseTableDal<TagDto>
    {
        public TagsDal(string connectionString) : base(connectionString, null)
        {
        }

        public override void Delete(int id, SqlTransaction txn = null)
        {
            var parameters = new Dictionary<string, object> { {"@Id", id} };

            using (var conn = GetConnection(connectionString))
            {
                conn.ExecuteNonQuery("DELETE FROM [ArticleTags] WHERE [TagId] = @Id", parameters);
                conn.ExecuteNonQuery("DELETE FROM [Tags] WHERE [Id] = @Id", parameters);
            }
        }
    }
}