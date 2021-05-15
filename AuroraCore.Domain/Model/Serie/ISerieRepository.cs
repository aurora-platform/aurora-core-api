using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface ISerieRepository
    {
        IEnumerable<Serie> FindByTopics(IEnumerable<Topic> topics);

        IEnumerable<Serie> FindByCategory(Guid categoryId);

        IEnumerable<Serie> FindByNameOrCreator(string searchParam);
    }
}
