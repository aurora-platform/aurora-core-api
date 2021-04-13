using AuroraCore.Application.DTOs;

namespace AuroraCore.Application.Interfaces
{
    public interface IAuthenticationService
    {
        UserResource AuthenticateWithPassword(string username, string password);
    }
}
