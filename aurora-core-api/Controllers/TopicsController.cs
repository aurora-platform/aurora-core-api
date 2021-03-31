using aurora_core_api.Factories;
using aurora_core_api.Responses;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace aurora_core_api.Controllers
{
    [ApiController]
    public class TopicsController : ControllerBase
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
            try
            {
                IEnumerable<Topic> topics = _topicsService.GetAvailableTopics();
                return ResponseFactory.Ok(Response, "", topics);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Create<IEnumerable<Topic>>(Response, HttpStatusCode.InternalServerError, ex.Message, null);
            }
        }
    }
}
 