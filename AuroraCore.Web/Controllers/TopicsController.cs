using aurora_core_api.Responses;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace aurora_core_api.Controllers
{
    [ApiController]
    public class TopicsController : ApiControllerBase
    {
        private readonly ITopicService _topicsService;

        public TopicsController(ITopicService topicsService)
        {
            _topicsService = topicsService;
        }

        [HttpGet]
        [Route("topics")]
        public Response<IEnumerable<Topic>> GetAllTopics()
        {
            IEnumerable<Topic> topics = _topicsService.GetAvailableTopics();

            return Ok(topics);
        }
    }
}
 