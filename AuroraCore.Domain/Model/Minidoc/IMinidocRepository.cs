using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface IMinidocRepository
    {
        void Store(Minidoc minidoc);
        void Update(Minidoc minidoc);
        void Delete(Guid id);
        Minidoc FindById(Guid id);
        IEnumerable<Minidoc> FindByChannel(Guid channelId);
        IEnumerable<Minidoc> FindByTopics(IEnumerable<Topic> topics);
        IEnumerable<Minidoc> FindByCategory(Guid categoryId);
        IEnumerable<Minidoc> FindByNameOrCreator(string searchParam);
        IEnumerable<MinidocCategory> GetAllCategories();
        IEnumerable<MinidocCategory> GetCategoriesByIds(Guid[] ids);
        int CountCategories();
    }
}
