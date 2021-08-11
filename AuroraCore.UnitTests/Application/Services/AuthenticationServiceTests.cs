using AuroraCore.Application.DTOs;
using AuroraCore.Application.Services;
using AuroraCore.Domain.Shared;
using AuroraCore.Infrastructure.Factories;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.UnitTests.Infrastructure.Repositories;
using System;
using Xunit;

namespace AuroraCore.UnitTests.Application.Services
{
    public class AuthenticationServiceTests : IDisposable
    {
        private AuthenticationService _authenticationService;
        private UserService _userService;

        public AuthenticationServiceTests()
        {
            _authenticationService = new AuthenticationService(new UserRepositoryMock(), new BcryptHashProvider(), MapperFactory.Create());
            _userService = new UserService(new UserRepositoryMock(), MapperFactory.Create(), new BcryptHashProvider());
        }

        public void Dispose()
        {
            _authenticationService = null;
        }

        /**
         * AuthenticateWithPassword
         */

        [Fact]
        public void AuthenticateWithPassword_ThrowsValidationException_WithoutPassword()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateWithPassword("user", ""));
        }

        [Fact]
        public void AuthenticateWithPassword_ThrowsValidationException_WithoutUsername()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateWithPassword("", "Password@123"));
        }

        [Fact]
        public void AuthenticateWithPassword_ThrowsValidationException_WhenUserNotExists()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateWithPassword("user", "Password@123"));
        }

        [Fact]
        public void AuthenticateWithPassword_ThrowsValidationException_WhenPasswordIsWrong()
        {
            _userService.Create("user", "user@email.com", "Password@123");
            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateWithPassword("user", "Password@121"));
        }

        [Fact]
        public void AuthenticateWithPassword_AuthenticatedUser_IsNotNull()
        {
            _userService.Create("user", "user@email.com", "Password@123");
            UserResource authenticatedUser = _authenticationService.AuthenticateWithPassword("user", "Password@123");
            Assert.True(authenticatedUser != null);
        }
    }
}