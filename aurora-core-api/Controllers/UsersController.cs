using aurora_core_api.DTOs;
using aurora_core_api.Responses;
using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace aurora_core_api.Controllers
{
    [Authorize]
    [ApiController]
    public class UsersController : ApiControllerBase
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
                _userService.SetupInitialSettings(Guid.NewGuid(), settings.Name, settings.LikedTopics);

                return Ok("Configured successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("me/liked-topics")]
        public Response<object> UpdateCurrentUserLikedTopics([FromBody] IEnumerable<Topic> likedTopics)
        {
            try
            {
                _userService.EditLikedTopics(Guid.NewGuid(), likedTopics);

                return Ok("Edited successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("me")]
        public Response<UserProfile> GetCurrentUserProfile()
        {
            try
            {
                UserProfile userProfile = _userService.GetProfile(Guid.NewGuid());
                
                return Ok(userProfile);
            }
            catch (ValidationException ex)
            {
                return BadRequest<UserProfile>(ex.Message);
            }
        }

        [HttpPut]
        [Route("me")]
        public Response<object> EditCurrentUserProfile([FromBody] UserProfile userProfile)
        {
            try
            {
                _userService.EditProfile(userProfile);

                return Ok("Profile edited successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("me/password")]
        public Response<object> ChangeCurrentUserPassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                _userService.ChangePassword(GetCurrentUser().Id, request.Current, request.New, request.Confirmation);

                return Ok("Password changed successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
