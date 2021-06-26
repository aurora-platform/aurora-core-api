using AuroraCore.Web.Responses;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AuroraCore.Web.Controllers
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
            return Ok(_topicsService.GetAvailableTopics());
        }
    }
}
 