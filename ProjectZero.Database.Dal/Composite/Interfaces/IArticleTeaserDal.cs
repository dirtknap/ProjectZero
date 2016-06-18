using System;
using System.Collections.Generic;
using ProjectZero.Database.Dto.Composite;

namespace ProjectZero.Database.Dal.Composite.Interfaces
{
    public interface IArticleTeaserDal
    {
        ArticleTeaserDto Get(int id);
        List<ArticleTeaserDto> GetSelected(List<int> idList);
        List<ArticleTeaserDto> GetForDateRange(DateTimeOffset start, DateTimeOffset end);
        List<ArticleTeaserDto> GetLastN(int number);
        List<ArticleTeaserDto> GetAll();
    }
}