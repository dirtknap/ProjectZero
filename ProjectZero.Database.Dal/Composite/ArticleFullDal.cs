using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using ProjectZero.Database.Dal.Composite.Interfaces;
using ProjectZero.Database.Dal.Tables.Interfaces;
using ProjectZero.Database.Dto.Composite;
using ProjectZero.Database.Dto.Tables;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Composite
{
    public class ArticleFullDal : BaseCompositeDal<ArticleFullDto>, IArticleFullDal
    {


        public ArticleFullDal(string connectionString) : base(connectionString, null)
        {
        }

        public int SaveArticle(ArticleFullDto article)
        {
            var id = -1;

            using (var conn = GetConnection(connectionString))
            {

                var result = conn.InsertAndReturnIdent(article.GetArticleDto());
                id = int.Parse(result);

                conn.InsertAndReturnIdent(new ArticleTextDto {ArticleId= id, Text = article.Text});

                foreach (var tag in article.GetTags())
                {
                    var tagId = -1;
                    var parameters = new Dictionary<string, object> {{"@text", tag}};

                    var dbTag = conn.ExecuteSpReadOne("sp_InsertTag", parameters);

                    tagId = int.Parse(dbTag);           
                    
                    conn.InsertAndReturnIdent(new ArticleTagsDto {ArticleId = id, TagId = tagId});
                }
            }

            return id;
        }

        public List<ArticleFullDto> GetAllArticles()
        {
            using (var conn = GetConnection(connectionString))
            {
                var result = conn.ReadIntoList<ArticleFullDto>(BaseQuery(), new Dictionary<string, object>());
                return result ?? new List<ArticleFullDto>();
            }
        }

        public ArticleFullDto GetArticle(int id)
        {
            var parameters = new Dictionary<string, object> { {"@Id", id} };

            using (var conn = GetConnection(connectionString))
            {
                var result = conn.ReadIntoObject<ArticleFullDto>($"{BaseQuery()} WHERE Id = @Id", parameters);
                return result ?? new ArticleFullDto();
            }
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

        public void UpdateArticle(ArticleFullDto article)
        {
            using (var conn = GetConnection(connectionString))
            {
                var parameters = new Dictionary<string, object> { {"@aid", article.Id} };
                using (var txn = conn.BeginTransaction(IsolationLevel.Serializable))
                {
                    conn.ExecuteNonQuery("DELETE FROM [ArticleTags] WHERE ArticleId = @aid", parameters, txn);
                }

                using (var txn = conn.BeginTransaction(IsolationLevel.Serializable))
                {

                    foreach (var tag in article.GetTags())
                    {
                        var tagId = -1;
                        parameters = new Dictionary<string, object> { { "@text", tag } };

                        var dbTag = conn.ExecuteSpReadOne("sp_InsertTag", parameters);

                        tagId = int.Parse(dbTag);

                        conn.InsertAndReturnIdent(new ArticleTagsDto { ArticleId = isd, TagId = tagId });
                    }
                }
                
            }
        }

        private static string BaseQuery()
        {
            return "SELECT a.Id, a.Name, a.Author, a.LastEdited, a.Published, a.Teaser, a.Active, i.Text, " +
                "LEFT JOIN ArticleText i ON i.ArticleId = a.Id " +
                "STUFF((SELECT DISTINCT ','+ t.text FROM Tags " +
                "LEFT JOIN ArticleTags at ON at.ArticleId = a.Id " +
                "LEFT JOIN Tags t ON t.Id = at.TagId " +
                "FOR XML PATH('') ), 1, 1,'') [Tags] " +
                "FROM Articles a";
        }
    }
}