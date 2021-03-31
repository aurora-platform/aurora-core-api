using aurora_core_api.DTOs;
using aurora_core_api.Factories;
using aurora_core_api.Responses;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace aurora_core_api.Controllers
{
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _accountService;

        public UsersController(IUserService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("users/initial-settings")]
        public Response<object> ConfigureAccount([FromBody] AccountInitSettings settings)
        {
            try
            {
                _accountService.SetupInitialSettings(settings.UserId, settings.Name, settings.LikedTopics);
                return ResponseFactory.Ok(Response, "Account created successfully");
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
