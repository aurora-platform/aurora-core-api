using AuroraCore.Domain.Model;

namespace AuroraCore.Application.Interfaces
{
    public interface IAuthenticationService
    {
        User AuthenticateWithPassword(string username, string password);

        User SignUp(string username, string email, string password);
    }
}
