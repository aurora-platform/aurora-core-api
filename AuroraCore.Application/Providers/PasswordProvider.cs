using AuroraCore.Application.Dependencies;
using AuroraCore.Domain.Shared;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuroraCore.Application.Providers
{
    public class PasswordProvider
    {
        private IHashProvider _hashProvider { get; }

        public PasswordProvider(IHashProvider hashProvider)
        {
            _hashProvider = hashProvider;
        }

        private static void Validate(string plainTextPassword)
        {
            if (plainTextPassword.Length < 6)
            {
                throw new ValidationException("Password must be at least 6 characters");
            }

            if (!plainTextPassword.Any(char.IsUpper))
            {
                throw new ValidationException("Password must have at least 1 upper case letter");
            }

            if (!plainTextPassword.Any(char.IsNumber))
            {
                throw new ValidationException("Password must have at least 1 number");
            }

            var regex = new Regex("[^a-zA-Z0-9]");

            if (!regex.IsMatch(plainTextPassword))
            {
                throw new ValidationException("Password must have at least 1 special character");
            }
        }

        public string Protect(string plainTextPassword)
        {
            Validate(plainTextPassword);
            return _hashProvider.HashPassword(plainTextPassword);
        }

        public void Verify(string password, string hash)
        {
           _hashProvider.Verify(password, hash);
        }
    }
}
