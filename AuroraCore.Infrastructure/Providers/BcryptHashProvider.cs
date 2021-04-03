using AuroraCore.Application.Dependencies;
using System;
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
            bool isValid = Bcrypt.Verify(password, hash);

            if (!isValid) {
                throw new Exception("Invalid password");
            }
        }
    }
}
