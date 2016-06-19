using System;
using System.Collections.Generic;
using ProjectZero.Database.Dto.Composite;

namespace ProjectZero.Database.Dal.Composite.Interfaces
{
    public interface IArticleFullDal
    {
        void SetActive(int id, bool active);

        int SaveArticle(ArticleFullDto article);
        ArticleFullDto Get(int id);
        List<ArticleFullDto> GetSelected(List<int> idList);
        List<ArticleFullDto> GetForDateRange(DateTimeOffset start, DateTimeOffset end);
        List<ArticleFullDto> GetLastN(int number);
        List<ArticleFullDto> GetAll();
        void Update(ArticleFullDto article);
        void Delete(int id);
    }
}