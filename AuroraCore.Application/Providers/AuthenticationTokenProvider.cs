using AuroraCore.Application.Dependencies;
using AuroraCore.Application.DTOs;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.Providers
{
    public class AuthenticationTokenProvider
    {
        private readonly IJwtTokenProvider _jwtProvider;

        public AuthenticationTokenProvider(IJwtTokenProvider jwtTokenProvider)
        {
            _jwtProvider = jwtTokenProvider;
        }

        private static IDictionary<string, object> BuildAccessTokenClaims(Guid userId)
        {
            return new Dictionary<string, object>
            {
                { "sub", userId },
                { "exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds() }
            };
        }

        private static IDictionary<string, object> BuildRefreshTokenClaims(Guid userId)
        {
            return new Dictionary<string, object>
            {
                { "sub", userId },
                { "exp", DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeSeconds() }
            };
        }

        public AuthTokens GenerateTokens(Guid userId)
        {
            string accessToken = _jwtProvider.CreateToken(BuildAccessTokenClaims(userId));
            string refreshToken = _jwtProvider.CreateToken(BuildRefreshTokenClaims(userId));

            return new AuthTokens(accessToken, refreshToken);
        }
    }
}
