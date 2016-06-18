using System;
using System.Collections.Generic;
using ProjectZero.Database.Dal.Composite.Interfaces;
using ProjectZero.Database.Dto.Composite;
using ProjectZero.Database.Extensions;

namespace ProjectZero.Database.Dal.Composite
{
    public class ArticleTeaserDal : BaseCompositeDal<ArticleTeaserDto>, IArticleTeaserDal
    {
        public ArticleTeaserDal(string connectionString) : base(connectionString, null)
        {
        }

        public List<ArticleTeaserDto> GetForDateRange(DateTimeOffset start, DateTimeOffset end)
        {
            var parameters = new Dictionary<string, object> { { "@start", start }, { "@end", end } };

            using (var conn = GetConnection(connectionString))
            {
                var result = conn.ReadIntoList<ArticleTeaserDto>($"{BaseSelectQuery()} WHERE Published > @start AND Published < @end", parameters);
                return result ?? new List<ArticleTeaserDto>();
            }
        }

       protected override string BaseSelectQuery(int top = 0)
        { 
            return $"SELECT {(top > 0 ? $"TOP {top}" : "" )} a.Id, a.Name, a.Author, a.LastEdited, a.Published, a.Teaser, a.Active, " +
                "STUFF((SELECT DISTINCT ','+ t.text FROM Tags " +
                "LEFT JOIN ArticleTags at ON at.ArticleId = a.Id " +
                "LEFT JOIN Tags t ON t.Id = at.TagId " +
                "FOR XML PATH('') ), 1, 1,'') [Tags] " +
                $"FROM Articles a{(top > 0 ? $" ORDER BY Id DESC" : "")}";
        }
    }
}