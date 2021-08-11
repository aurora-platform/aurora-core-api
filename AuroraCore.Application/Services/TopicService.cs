using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using System.Collections.Generic;

namespace AuroraCore.Application.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public void CreateTopic(Topic topic)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Topic> GetAvailableTopics()
        {
            return _topicRepository.GetAll();
        }
    }
}