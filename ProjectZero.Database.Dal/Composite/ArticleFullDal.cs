using System;
using System.Collections.Generic;
using ProjectZero.Database.Dal.Composite.Interfaces;
using ProjectZero.Database.Dto.Composite;
using ProjectZero.Database.Dto.Tables;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Composite
{
    public class ArticleFullDal : BaseTableAccess<ArticleFullDto>, IArticleFullDal
    {


        public ArticleFullDal(string connectionString) : base(connectionString, null)
        {
        }

        public int SaveArticle(ArticleFullDto article)
        {
            var id = -1;

            using (var conn = GetConnection())
            {

                var result = Insert(article.GetArticleDto());
                id = int.Parse(result);

                Insert(new ArticleTextDto {ArticleId= id, Text = article.Text});

                foreach (var tag in article.GetTags())
                {
                    var paramters = new Dictionary<string, object> {{"@text", tag}};

                    var tagId = ExecuteNonQueryReturnIdent(InsertTagQuery(), paramters);
                    
                    Insert(new ArticleTagsDto {ArticleId = id, TagId = int.Parse(tagId)});
                }
            }

            return id;
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

        private string BaseQuery()
        {
            return "SELECT a.Id, a.Name, a.Author, a.LastEdited, a.Published, a.Teaser, a.Active, i.Text, " +
                "LEFT JOIN ArticleText i ON i.ArticleId = a.Id " +
                "STUFF((SELECT DISTINCT ','+ t.text FROM Tags " +
                "LEFT JOIN ArticleTags at ON at.ArticleId = a.Id " +
                "LEFT JOIN Tags t ON t.Id = at.TagId " +
                "FOR XML PATH('') ), 1, 1,'') [Tags] " +
                "FROM Articles a";
        }

        private string InsertTagQuery()
        {
            return "IF EXISTS (SELECT 1 FROM Tags WHERE Text = @text) " +
                   "SELECT Id FROM Tags WHERE Text = @text " +
                   "ELSE " +
                   "INSERT INTO Tags (Text) VALUES(@text)";
        }
    }
}