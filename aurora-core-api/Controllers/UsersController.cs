using aurora_core_api.DTOs;
using aurora_core_api.Factories;
using aurora_core_api.Responses;
using AuroraCore.Application.DTOs;
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
        [Route("me/initial-setting")]
        public Response<object> ConfigureCurrentUser([FromBody] AccountInitSettings settings)
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
        [Route("me/liked-topics")]
        public Response<object> UpdateCurrentUserLikedTopics([FromBody] IEnumerable<Topic> likedTopics)
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

        [HttpGet]
        [Route("me")]
        public Response<UserProfile> GetCurrentUserProfile()
        {
            try
            {
                UserProfile userProfile = _userService.GetProfile(GetSubClaim());
                return ResponseFactory.Ok(Response, "", userProfile);
            }
            catch (ValidationException ex)
            {
                return ResponseFactory.Create<UserProfile>(Response, HttpStatusCode.BadRequest, ex.Message, null);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Create<UserProfile>(Response, HttpStatusCode.InternalServerError, ex.Message, null);
            }
        }

        [HttpPut]
        [Route("me")]
        public Response<object> EditCurrentUserProfile([FromBody] UserProfile userProfile)
        {
            try
            {
                _userService.EditProfile(userProfile);
                return ResponseFactory.Ok(Response, "Profile edited successfully");
            }
            catch (ValidationException ex)
            {
                return ResponseFactory.Create(Response, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Create(Response, HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
