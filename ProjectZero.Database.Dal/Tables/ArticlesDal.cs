using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectZero.Database.Dal.Tables.Interfaces;
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
