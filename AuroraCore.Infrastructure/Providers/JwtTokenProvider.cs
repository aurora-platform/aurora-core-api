using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using System;
using System.Collections.Generic;

namespace AuroraCore.Infrastructure.Providers
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private const string Secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

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

        private string CreateToken(IDictionary<string, object> claims)
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Secret)
                .AddClaims(claims)
                .Encode();
        }

        public string CreateAccessToken(Guid userId)
        {
            return CreateToken(BuildAccessTokenClaims(userId));
        }

        public string CreateRefreshToken(Guid userId)
        {
            return CreateToken(BuildRefreshTokenClaims(userId));
        }

        public IDictionary<string, object> Decode(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();

            IJwtDecoder decoder = new JwtDecoder(
                serializer,
                new JwtValidator(serializer, new UtcDateTimeProvider()),
                new JwtBase64UrlEncoder(),
                new HMACSHA256Algorithm()
            );

            return decoder.DecodeToObject<IDictionary<string, object>>(token, Secret, verify: true);
        }

        public bool IsValid(string token)
        {
            try
            {
                Decode(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Tuple<string, string> CreateTokens(Guid userId)
        {
            string accessToken = CreateToken(BuildAccessTokenClaims(userId));
            string refreshToken = CreateToken(BuildRefreshTokenClaims(userId));
            return new Tuple<string, string>(accessToken, refreshToken);
        }
    }
}