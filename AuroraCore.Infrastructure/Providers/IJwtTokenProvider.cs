using System;
using System.Collections.Generic;

namespace AuroraCore.Infrastructure.Providers
{
    public interface IJwtTokenProvider
    {
        string CreateAccessToken(Guid userId);

        string CreateRefreshToken(Guid userId);

        Tuple<string, string> CreateTokens(Guid userId);

        bool IsValid(string token);

        IDictionary<string, object> Decode(string token);
    }
}
