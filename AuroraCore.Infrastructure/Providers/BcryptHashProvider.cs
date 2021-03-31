using AuroraCore.Application.Dependencies;
using Bcrypt = BCrypt.Net.BCrypt;

namespace AuroraCore.Infrastructure.Providers
{
    public class BcryptHashProvider : IHashProvider
    {
        public string HashPassword(string password)
        {
            return Bcrypt.HashPassword(password);
        }

        public void Verify(string password, string hash)
        {
            Bcrypt.Verify(password, hash);
        }
    }
}
