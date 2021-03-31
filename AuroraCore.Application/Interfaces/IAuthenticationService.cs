using AuroraCore.Application.DTOs;

namespace AuroraCore.Application.Interfaces
{
    public interface IAuthenticationService
    {
        AuthTokens AuthenticateWithPassword(string username, string password);

        AuthTokens AuthenticateWithRefreshToken(string refreshToken);

        AuthTokens SignUp(string username, string email, string password);
    }
}
