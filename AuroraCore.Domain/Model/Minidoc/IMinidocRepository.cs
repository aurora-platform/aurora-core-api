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
        IList<MinidocCategory> GetCategoriesByIds(Guid[] ids);
        IList<MinidocCategory> GetAllCategories();
    }
}
