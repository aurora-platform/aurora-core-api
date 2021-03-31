using AuroraCore.Domain.Model;
using System.Collections.Generic;

namespace AuroraCore.Application.Interfaces
{
    public interface ITopicService
    {
        IEnumerable<Topic> GetAvailableTopics();

        void CreateTopic(Topic topic);
    }
}
