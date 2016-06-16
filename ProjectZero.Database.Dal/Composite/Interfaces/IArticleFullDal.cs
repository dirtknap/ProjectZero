using System;
using System.Collections.Generic;
using ProjectZero.Database.Dto.Composite;

namespace ProjectZero.Database.Dal.Composite.Interfaces
{
    public interface IArticleFullDal
    {
        List<ArticleFullDto> GetAllArticles();
        List<ArticleFullDto> GetArticleArticles(List<int> idList);
        List<ArticleFullDto> GetArticlesForDateRange(DateTimeOffset start, DateTimeOffset end);
        List<ArticleFullDto> GetLastNArticles(int number);
        ArticleFullDto GetArticle(int id); 
    }
}