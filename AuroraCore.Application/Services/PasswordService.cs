using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Shared;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuroraCore.Application.Services
{
    public class PasswordService
    {
        private readonly IHashProvider _hashProvider;

        public PasswordService(IHashProvider hashProvider)
        {
            _hashProvider = hashProvider;
        }

        private static void Validate(string plainTextPassword)
        {
            if (plainTextPassword.Length < 6)
                throw new InvalidPasswordException("Password must be at least 6 characters");

            if (!plainTextPassword.Any(char.IsUpper))
                throw new InvalidPasswordException("Password must have at least 1 upper case letter");

            if (!plainTextPassword.Any(char.IsNumber))
                throw new InvalidPasswordException("Password must have at least 1 number");

            var regex = new Regex("[^a-zA-Z0-9]");

            if (!regex.IsMatch(plainTextPassword))
                throw new InvalidPasswordException("Password must have at least 1 special character");
        }

        public string Protect(string plainTextPassword)
        {
            Validate(plainTextPassword);
            return _hashProvider.HashPassword(plainTextPassword);
        }

        public void Verify(string password, string hash)
        {
            if (!_hashProvider.IsEqual(password, hash))
                throw new InvalidPasswordException("Invalid password");
        }

        public class InvalidPasswordException : ValidationException
        {
            public InvalidPasswordException(string message) : base(message)
            {
            }
        }
    }
}