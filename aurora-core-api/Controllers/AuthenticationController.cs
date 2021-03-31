using aurora_core_api.DTOs;
using aurora_core_api.Factories;
using aurora_core_api.Responses;
using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace aurora_core_api.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("auth")]
        public Response<AuthTokens> AuthenticateWithPassword([FromBody] AuthRequest request)
        {
            try
            {
                AuthTokens tokens = _authenticationService.AuthenticateWithPassword(request.Username, request.Password);
                return ResponseFactory.Ok(Response, "Successfully authenticated", tokens);
            }
            catch (ValidationException ex)
            {
                return ResponseFactory.Create<AuthTokens>(Response, HttpStatusCode.BadRequest, ex.Message, null);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Create<AuthTokens>(Response, HttpStatusCode.InternalServerError, ex.Message, null);
            }
        }

        [HttpPost]
        [Route("auth/refresh")]
        public Response<AuthTokens> AuthenticateWithRefreshToken([FromBody] string refreshToken)
        {
            try
            {
                AuthTokens tokens = _authenticationService.AuthenticateWithRefreshToken(refreshToken);
                return ResponseFactory.Ok(Response, "Successfully authenticated", tokens);
            }
            catch (ValidationException ex)
            {
                return ResponseFactory.Create<AuthTokens>(Response, HttpStatusCode.BadRequest, ex.Message, null);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Create<AuthTokens>(Response, HttpStatusCode.InternalServerError, ex.Message, null);
            }
        }

        [HttpPost]
        [Route("signup")]
        public Response<AuthTokens> SignUp([FromBody] SignUpRequest request)
        {
            try
            {
                AuthTokens tokens = _authenticationService.SignUp(request.Username, request.Email, request.Password);
                return ResponseFactory.Ok(Response, "Account created successfully", tokens);
            }
            catch (ValidationException ex)
            {
                return ResponseFactory.Create<AuthTokens>(Response, HttpStatusCode.BadRequest, ex.Message, null);
            }
            catch (Exception ex)
            {
                return ResponseFactory.Create<AuthTokens>(Response, HttpStatusCode.InternalServerError, ex.Message, null);
            }
        }
    }
}
