using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface ITopicRepository
    {
        IEnumerable<Topic> GetAll();
    }
}
