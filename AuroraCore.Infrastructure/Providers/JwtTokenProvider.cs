using AuroraCore.Application.Dependencies;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using System;
using System.Collections.Generic;

namespace AuroraCore.Infrastructure.Providers
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private const string Secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        public string CreateToken(IDictionary<string, object> claims)
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Secret)
                .AddClaims(claims)
                .Encode();
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

    }
}
