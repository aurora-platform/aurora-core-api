using AuroraCore.Application.Dependencies;
using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using AuroraCore.Application.Providers;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using System;

namespace AuroraCore.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordProvider _passwordProvider;
        private readonly AuthenticationTokenProvider _tokenGenerationService;
        private readonly IJwtTokenProvider _jwtProvider;

        public AuthenticationService(IUserRepository userRepository, IHashProvider hashProvider, IJwtTokenProvider jwtTokenProvider)
        {
            _userRepository = userRepository;
            _passwordProvider = new PasswordProvider(hashProvider);
            _tokenGenerationService = new AuthenticationTokenProvider(jwtTokenProvider);
            _jwtProvider = jwtTokenProvider;
        }

        public AuthTokens AuthenticateWithPassword(string username, string password)
        {
            User user = _userRepository.FindByUsername(username);

            if (user == null)
            {
                throw new ValidationException("User not exists");
            }

            try
            {
                _passwordProvider.Verify(password, user.Password);
            }
            catch (Exception)
            {
                throw new ValidationException("Username or password is incorrect");
            }

            return _tokenGenerationService.GenerateTokens(user.Id);
        }

        public AuthTokens AuthenticateWithRefreshToken(string refreshToken)
        {
            if (!_jwtProvider.IsValid(refreshToken))
            {
                throw new ValidationException("Invalid token");
            }

            var decoded = _jwtProvider.Decode(refreshToken);

            User user = _userRepository.FindByID(new Guid((string)decoded["sub"]));

            if (user == null)
            {
                throw new ValidationException("Invalid token");
            }

            return _tokenGenerationService.GenerateTokens(user.Id);
        }

        private void CheckIfUserExists(string username, string email)
        {
            User existingUser = _userRepository.FindByUsernameOrEmail(username, email);

            if (existingUser != null)
            {
                if (existingUser.Email == email)
                {
                    throw new ValidationException("A user already exists with this email");
                }

                if (existingUser.Username == username)
                {
                    throw new ValidationException("This username already taken");
                }
            }
        }

        public AuthTokens SignUp(string username, string email, string password)
        {
            CheckIfUserExists(username, email);

            var user = new User(username, email);

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ValidationException("Password is required");
            }

            var protectedPassword = _passwordProvider.Protect(password);

            user.SetPassword(protectedPassword);
            user.SetAsActive();

            _userRepository.Store(user);

            return _tokenGenerationService.GenerateTokens(user.Id);
        }
    }
}
