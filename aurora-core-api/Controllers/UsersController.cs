using aurora_core_api.DTOs;
using aurora_core_api.Factories;
using aurora_core_api.Responses;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace aurora_core_api.Controllers
{
    [Authorize]
    [ApiController]
    public class UsersController : BaseAPIController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService accountService)
        {
            _userService = accountService;
        }

        [HttpPost]
        [Route("users/initial-settings")]
        public Response<object> ConfigureAccount([FromBody] AccountInitSettings settings)
        {
            try
            {
                _userService.SetupInitialSettings(GetSubClaim(), settings.Name, settings.LikedTopics);
                return ResponseFactory.Ok(Response, "Configured successfully");
            }
            catch (ValidationException ex)
            {
                return ResponseFactory.Create<object>(Response, HttpStatusCode.BadRequest, ex.Message, null);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Create<object>(Response, HttpStatusCode.InternalServerError, ex.Message, null);
            }
        }

        [HttpPut]
        [Route("users/topics")]
        public Response<object> UpdateTopics([FromBody] IEnumerable<Topic> likedTopics)
        {
            try
            {
                _userService.EditLikedTopics(GetSubClaim(), likedTopics);
                return ResponseFactory.Ok(Response, "Edited successfully");
            }
            catch (ValidationException ex)
            {
                return ResponseFactory.Create<object>(Response, HttpStatusCode.BadRequest, ex.Message, null);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Create<object>(Response, HttpStatusCode.InternalServerError, ex.Message, null);
            }
        }
    }
}
