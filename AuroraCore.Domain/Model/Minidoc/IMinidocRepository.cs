using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface IMinidocRepository : IRepository<Minidoc>
    {
        IEnumerable<Minidoc> FindByTopics(IEnumerable<Topic> topics);

        IEnumerable<Minidoc> FindByCategory(Guid categoryId);

        IEnumerable<Minidoc> FindByNameOrCreator(string searchParam);
    }
}
