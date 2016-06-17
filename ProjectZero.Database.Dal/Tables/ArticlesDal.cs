using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using ProjectZero.Database.Dal.Tables.Interfaces;
using ProjectZero.Database.Dto.Tables;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Tables
{
    public class ArticlesDal : BaseTableDal<ArticleDto>
    {
        public ArticlesDal(string connectionString, string schema = null) : base(connectionString, schema)
        {
        }
    }
}
