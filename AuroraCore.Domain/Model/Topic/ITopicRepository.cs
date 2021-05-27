using System.Collections.Generic;
using System;

namespace AuroraCore.Domain.Model
{
    public interface ITopicRepository
    {
        int Count();
        IEnumerable<Topic> GetAll();
        IEnumerable<Topic> GetByIds(Guid[] ids);
    }
}
