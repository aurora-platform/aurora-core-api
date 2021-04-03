using aurora_core_api.DTOs;
using aurora_core_api.Factories;
using aurora_core_api.Responses;
using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Infrastructure.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace aurora_core_api.Controllers
{
    [ApiController]
    public class AuthenticationController : BaseAPIController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUserService _userService;

        public AuthenticationController(IAuthenticationService authenticationService, IJwtTokenProvider jwtTokenProvider, IUserService userService)
        {
            _authenticationService = authenticationService;
            _jwtTokenProvider = jwtTokenProvider;
            _userService = userService;
        }

        [HttpPost]
        [Route("auth")]
        public Response<AuthTokens> AuthenticateWithPassword([FromBody] AuthRequest request)
        {
            try
            {
                User authenticatedUser = _authenticationService.AuthenticateWithPassword(request.Username, request.Password);
                Tuple<string, string> tokens = _jwtTokenProvider.CreateTokens(authenticatedUser.Id);

                return ResponseFactory.Ok(Response, "Successfully authenticated", new AuthTokens(tokens.Item1, tokens.Item2));
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
                if (!_jwtTokenProvider.IsValid(refreshToken))
                {
                    throw new ValidationException("Invalid token");
                }

                IDictionary<string, object> claims = _jwtTokenProvider.Decode(refreshToken);

                UserProfile userProfile = _userService.GetProfile(new Guid((string)claims["sub"]));

                Tuple<string, string> tokens = _jwtTokenProvider.CreateTokens(userProfile.Id);

                return ResponseFactory.Ok(Response, "Successfully authenticated", new AuthTokens(tokens.Item1, tokens.Item2));
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
                User createdUser = _authenticationService.SignUp(request.Username, request.Email, request.Password);
                Tuple<string, string> tokens = _jwtTokenProvider.CreateTokens(createdUser.Id);

                return ResponseFactory.Ok(Response, "Account created successfully", new AuthTokens(tokens.Item1, tokens.Item2));
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
