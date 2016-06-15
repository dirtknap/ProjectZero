using System;
using System.Collections.Generic;
using ProjectZero.Database.Dal.Composite.Interfaces;
using ProjectZero.Database.Dto.Composite;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Composite
{
    public class ArticleTeaserDal : BaseTableAccess<ArticleTeaserDto>, IArticleTeaserDal
    {
        public ArticleTeaserDal(string connectionString) : base(connectionString, null)
        {
        }

        public List<ArticleTeaserDto> GetAllTeasers()
        {
            return ReadIntoList(BaseQuery(), new Dictionary<string, object>());
        }

        public List<ArticleTeaserDto> GetArticleTeasers(List<int> idList)
        {
            throw new NotImplementedException();
        }

        public List<ArticleTeaserDto> GetArtileTeasersForDateRange(DateTimeOffset start, DateTimeOffset end)
        {
            throw new NotImplementedException();
        }

        public List<ArticleTeaserDto> GetLastNArticleTeasers(int number)
        {
            throw new NotImplementedException();
        }

        public ArticleTeaserDto GetArticleTeaser(int id)
        {
            throw new NotImplementedException();
        }

        private string BaseQuery()
        {
            return "SELECT a.Id, a.Name, a.Author, a.LastEdited, a.Published, a.Teaser, " +
                "STUFF((SELECT DISTINCT ','+ t.text FROM Tags " +
                "LEFT JOIN ArticleTags at ON at.ArticleId = a.Id " +
                "LEFT JOIN Tags t ON t.Id = at.TagId " +
                "FOR XML PATH('') ), 1, 1,'') [Tags] " +
                "FROM Articles a";
        }
    }
}