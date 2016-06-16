using System;
using System.Collections.Generic;
using ProjectZero.Database.Dto.Composite;

namespace ProjectZero.Database.Dal.Composite.Interfaces
{
    public interface IArticleTeaserDal
    {
        List<ArticleTeaserDto> GetAllTeasers();
        List<ArticleTeaserDto> GetArticleTeasers(List<int> idList);
        List<ArticleTeaserDto> GetArticleTeasersForDateRange(DateTimeOffset start, DateTimeOffset end);
        List<ArticleTeaserDto> GetLastNArticleTeasers(int number);
        ArticleTeaserDto GetArticleTeaser(int id); 
    }
}