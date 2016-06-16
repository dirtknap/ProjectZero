using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using ProjectZero.Database.Dal.Composite.Interfaces;
using ProjectZero.Database.Dto.Composite;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Composite
{
    public class ArticleFullDal : BaseTableAccess<ArticleFullDto>, IArticleFullDal
    {
        public ArticleFullDal(string connectionString, string schema = null) : base(connectionString, schema)
        {
        }

        public List<ArticleFullDto> GetAllArticles()
        {
            throw new NotImplementedException();
        }

        public ArticleFullDto GetArticle(int id)
        {
            throw new NotImplementedException();
        }

        public List<ArticleFullDto> GetArticleArticles(List<int> idList)
        {
            throw new NotImplementedException();
        }

        public List<ArticleFullDto> GetArticlesForDateRange(DateTimeOffset start, DateTimeOffset end)
        {
            throw new NotImplementedException();
        }

        public List<ArticleFullDto> GetLastNArticles(int number)
        {
            throw new NotImplementedException();
        }
    }
}