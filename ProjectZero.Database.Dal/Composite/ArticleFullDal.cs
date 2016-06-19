using System;
using System.Collections.Generic;
using ProjectZero.Database.Dal.Composite.Interfaces;
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

        public void SetActive(int id, bool active)
        {
            var parameters = new Dictionary<string, object> { {"@Id", id}, {"@Active", active} };

            using (var conn = GetConnection(connectionString))
            {
                conn.ExecuteNonQuery("UPDATE [Articles] SET [Active] = @Active WHERE Id = @Id", parameters);
            }
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

        public List<ArticleFullDto> GetForDateRange(DateTimeOffset start, DateTimeOffset end)
        {
            var parameters = new Dictionary<string, object> { { "@start", start }, { "@end", end } };

            using (var conn = GetConnection(connectionString))
            {
                var result = conn.ReadIntoList<ArticleFullDto>($"{BaseSelectQuery()} WHERE Published > @start AND Published < @end", parameters);
                return result ?? new List<ArticleFullDto>();
            }
        }

        public void Update(ArticleFullDto article)
        {
            using (var conn = GetConnection(connectionString))
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@aid", article.Id},
                    {"@Name", article.Name },
                    {"@LastEdited", DateTimeOffset.Now },
                    {"@Teaser", article.Teaser },
                    {"@Active", article.Active },
                    {"@Text", article.Text }
                };

                conn.ExecuteSpNonQuery("sp_UpdateArticle", parameters);

                foreach (var tag in article.GetTags())
                {
                    var tagId = -1;
                    parameters = new Dictionary<string, object> {{"@text", tag}};

                    var dbTag = conn.ExecuteSpReadOne("sp_InsertTag", parameters);

                    tagId = int.Parse(dbTag);

                    conn.InsertAndReturnIdent(new ArticleTagsDto {ArticleId = article.Id, TagId = tagId});
                }
            }
        }

        public void Delete(int id)
        {
            var parameter = new Dictionary<string, object> { {"@Id",id} };

            using (var conn = GetConnection(connectionString))
            {
                conn.ExecuteSpNonQuery("sp_DeleteArticle", parameter);
            } 
        }

        protected override string BaseSelectQuery(int top = 0)
        {
            return $"SELECT {(top > 0 ? $"TOP {top}" : "")} a.Id, a.Name, a.Author, a.LastEdited, a.Published, a.Teaser, a.Active, i.Text, " +
                "LEFT JOIN ArticleText i ON i.ArticleId = a.Id " +
                "STUFF((SELECT DISTINCT ','+ t.text FROM Tags " +
                "LEFT JOIN ArticleTags at ON at.ArticleId = a.Id " +
                "LEFT JOIN Tags t ON t.Id = at.TagId " +
                "FOR XML PATH('') ), 1, 1,'') [Tags] " +
                $"FROM Articles a{(top > 0 ? $" ORDER BY a.Id DESC" : "")}";
        }
    }
}