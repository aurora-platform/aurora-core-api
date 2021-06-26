using AuroraCore.Web.DTOs;
using AuroraCore.Web.Responses;
using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Shared;
using AuroraCore.Infrastructure.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AuroraCore.Web.Controllers
{
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService, IJwtTokenProvider jwtTokenProvider)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _jwtTokenProvider = jwtTokenProvider;
        }

        [HttpPost]
        [Route("auth")]
        public Response<AuthTokens> AuthenticateWithPassword([FromBody] AuthRequest request)
        {
            try
            {
                UserResource authenticatedUser = _authenticationService.AuthenticateWithPassword(request.Username, request.Password);
                Tuple<string, string> tokens = _jwtTokenProvider.CreateTokens(authenticatedUser.Id);

                return Ok(new AuthTokens(tokens.Item1, tokens.Item2));
            }
            catch (ValidationException ex)
            {
                return BadRequest<AuthTokens>(ex.Message);
            }
        }

        [HttpPost]
        [Route("auth/refresh")]
        public Response<AuthTokens> AuthenticateWithRefreshToken([FromBody] string refreshToken)
        {
            try
            {
                if (!_jwtTokenProvider.IsValid(refreshToken))
                    throw new ValidationException("Invalid token");

                IDictionary<string, object> claims = _jwtTokenProvider.Decode(refreshToken);

                UserResource user = _userService.Get(new Guid((string)claims["sub"]));

                Tuple<string, string> tokens = _jwtTokenProvider.CreateTokens(user.Id);

                return Ok(new AuthTokens(tokens.Item1, tokens.Item2));
            }
            catch (ValidationException ex)
            {
                return BadRequest<AuthTokens>(ex.Message);
            }
        }

        [HttpPost]
        [Route("signup")]
        public Response<AuthTokens> SignUp([FromBody] SignUpRequest request)
        {
            try
            {
                UserResource createdUser = _userService.Create(request.Username, request.Email, request.Password);
                Tuple<string, string> tokens = _jwtTokenProvider.CreateTokens(createdUser.Id);

                return Created(new AuthTokens(tokens.Item1, tokens.Item2));
            }
            catch (ValidationException ex)
            {
                return BadRequest<AuthTokens>(ex.Message);
            }
        }
    }
}
