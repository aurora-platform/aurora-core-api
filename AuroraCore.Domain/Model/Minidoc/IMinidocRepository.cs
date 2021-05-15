using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface IMinidocRepository
    {
        void Store(Minidoc minidoc);
        IEnumerable<Minidoc> FindByTopics(IEnumerable<Topic> topics);

        IEnumerable<Minidoc> FindByCategory(Guid categoryId);

        IEnumerable<Minidoc> FindByNameOrCreator(string searchParam);
    }
}
