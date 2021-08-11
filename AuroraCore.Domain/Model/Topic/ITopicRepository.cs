using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface ITopicRepository
    {
        int Count();

        IList<Topic> GetAll();

        IList<Topic> GetByIds(Guid[] ids);
    }
}