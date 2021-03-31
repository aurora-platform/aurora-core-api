using AuroraCore.Domain.Shared;

namespace AuroraCore.Domain.Model
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByUsername(string username);

        User FindByUsernameOrEmail(string username, string email);
    }
}
