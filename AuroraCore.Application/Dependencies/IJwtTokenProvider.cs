using System;
using System.Collections.Generic;

namespace AuroraCore.Application.Dependencies
{
    public interface IJwtTokenProvider
    {
        string CreateToken(IDictionary<string, object> claims);

        bool IsValid(string token);

        IDictionary<string, object> Decode(string token);
    }
}
