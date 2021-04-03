using AuroraCore.Infrastructure.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;

namespace aurora_core_api.Controllers
{
    public class BaseAPIController : ControllerBase
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public BaseAPIController()
        {
            _jwtTokenProvider = new JwtTokenProvider();
        }

        protected Guid GetSubClaim()
        {
            string token =  Request.Headers[HeaderNames.Authorization];

            IDictionary<string, object> claims = _jwtTokenProvider.Decode(token.Replace("Bearer ", ""));

            return new Guid((string)claims["sub"]);
        }
    }
}
