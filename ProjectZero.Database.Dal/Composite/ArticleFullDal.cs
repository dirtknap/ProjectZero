using System;
using System.Collections.Generic;
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

                    var dbTag = conn.ReadIntoObject<TagDto>(getTagByText, parameters);

                    if (dbTag == null)
                    {
                        tagId = int.Parse(conn.InsertAndReturnIdent(new TagDto {Text = tag}));
                    }
                    else
                    {
                        tagId = dbTag.Id;
                    }                
                    
                    conn.InsertAndReturnIdent(new ArticleTagsDto {ArticleId = id, TagId = tagId});
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

        private readonly string getTagByText = "SELECT [Id],[Text] FROM [Tags] WHERE [Text] = @Text";
    }
}