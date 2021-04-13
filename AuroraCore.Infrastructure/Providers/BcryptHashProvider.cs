using AuroraCore.Application.Interfaces;
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

        public bool IsEqual(string password, string hash)
        {
            return Bcrypt.Verify(password, hash);
        }
    }
}
