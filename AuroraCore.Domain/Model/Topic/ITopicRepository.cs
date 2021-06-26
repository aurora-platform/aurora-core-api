using System.Collections.Generic;
using System;

namespace AuroraCore.Domain.Model
{
    public interface ITopicRepository
    {
        int Count();
        IList<Topic> GetAll();
        IList<Topic> GetByIds(Guid[] ids);
    }
}
