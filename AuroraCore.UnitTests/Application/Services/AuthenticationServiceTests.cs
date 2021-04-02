using AuroraCore.Application.Dependencies;
using AuroraCore.Application.DTOs;
using AuroraCore.Application.Services;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.UnitTests.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AuroraCore.UnitTests.Application.Services
{

    public class AuthenticationServiceTests
    {
        private AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _authenticationService = new AuthenticationService(
                new UserRepositoryMock(),
                new BcryptHashProvider(),
                new JwtTokenProvider()
            );
        }


        /**
         * AuthenticateWithPassword
         */

        [Fact]
        public void ShouldNot_AuthenticateWithPassword_WithoutPassword()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateWithPassword("user", ""));
        }

        [Fact]
        public void ShouldNot_AuthenticateWithPassword_WithoutUsername()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateWithPassword("", "Password@123"));
        }

        [Fact]
        public void ShouldNot_AuthenticateWithPassword_WhenUserNotExists()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateWithPassword("user", "Password@123"));
        }

        [Fact]
        public void ShouldNot_AuthenticateWithPassword_WhenPasswordIsWrong()
        {
            var authenticationService = new AuthenticationService(
                new UserRepositoryMock(),
                new BcryptHashProvider(),
                new JwtTokenProvider()
            );

            authenticationService.SignUp("user", "user@email.com", "Password@123");

            Assert.Throws<ValidationException>(() => authenticationService.AuthenticateWithPassword("user", "Password@121"));
        }

        [Fact]
        public void Shouldt_AuthenticateWithPassword_ReturnAuthTokens()
        {
            var authenticationService = new AuthenticationService(
                new UserRepositoryMock(),
                new BcryptHashProvider(),
                new JwtTokenProvider()
            );

            authenticationService.SignUp("user", "user@email.com", "Password@123");

            AuthTokens tokens = authenticationService.AuthenticateWithPassword("user", "Password@123");

            Assert.True(tokens != null);
        }

        /**
         * AuthenticateWithRefreshToken
         */

        [Fact]
        public void ShouldNot_AuthenticateWithRefreshToken_WithoutRefreshToken()
        {
            var authenticationService = new AuthenticationService(
                new UserRepositoryMock(),
                new BcryptHashProvider(),
                new JwtTokenProvider()
            );

            AuthTokens tokens = authenticationService.SignUp("user", "user@email.com", "Password@123");

            Assert.Throws<ValidationException>(() => authenticationService.AuthenticateWithRefreshToken(""));
        }

        [Fact]
        public void ShouldNot_AuthenticateWithRefreshToken_WhenUserNotExists()
        {
            IJwtTokenProvider jwtTokenProvider = new JwtTokenProvider();

            var authenticationService = new AuthenticationService(
                new UserRepositoryMock(),
                new BcryptHashProvider(),
                jwtTokenProvider
            );

            AuthTokens tokens = authenticationService.SignUp("user", "user@email.com", "Password@123");

            string token = jwtTokenProvider.CreateToken(new Dictionary<string, object>
            {
                { "sub", Guid.NewGuid() },
                { "exp", DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeSeconds() }
            });

            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateWithRefreshToken(token));
        }


        [Fact]
        public void ShouldNot_AuthenticateWithRefreshToken_WhenIsModified()
        {
            IJwtTokenProvider jwtTokenProvider = new JwtTokenProvider();

            var authenticationService = new AuthenticationService(
                new UserRepositoryMock(),
                new BcryptHashProvider(),
                jwtTokenProvider
            );

            AuthTokens tokens = authenticationService.SignUp("user", "user@email.com", "Password@123");

            IDictionary<string, object> body = jwtTokenProvider.Decode(tokens.RefreshToken);

            var newBody = $"{{ \"sub\": \"{Guid.NewGuid()}\", \"exp\": {body["exp"]} }}";

            string encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(newBody));

            string[] parts = tokens.RefreshToken.Split(".");
            parts[1] = encoded.Remove(encoded.Length - 1);

            string modifiedToken = string.Join(".", parts);

            Assert.Throws<ValidationException>(() => _authenticationService.AuthenticateWithRefreshToken(modifiedToken));
        }

        /**
         * SignUp
         */

        [Fact]
        public void ShouldNot_SignUp_WithoutUsername()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.SignUp("", "user@email.com", "Password@123"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithoutEmail()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.SignUp("user", "", "Password@123"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithoutPassword()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.SignUp("user", "user@email.com", ""));
        }

        [Fact]
        public void ShouldNot_SignUp_WithExistentUsername()
        {
            _authenticationService.SignUp("user", "user@email.com", "Password@123");

            Assert.Throws<ValidationException>(() => _authenticationService.SignUp("user", "user2@email.com", "Password@123"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithExistentEmail()
        {
            _authenticationService.SignUp("user", "user@email.com", "Password@123");

            Assert.Throws<ValidationException>(() => _authenticationService.SignUp("user2", "user@email.com", "Password@123"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithPasswordLessThan6Characters()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.SignUp("user", "user@email.com", "passw"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithPasswordWithout1UpperCaseAtLeast()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.SignUp("user", "user@email.com", "password"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithPasswordWithout1NumberAtLeast()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.SignUp("user", "user@email.com", "Password"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithPasswordWithout1SpecialCharacterAtLeast()
        {
            Assert.Throws<ValidationException>(() => _authenticationService.SignUp("user", "user@email.com", "Password123"));
        }

        [Fact]
        public void Should_SignUp_ReturnAuthTokens()
        {
            AuthTokens tokens = _authenticationService.SignUp("user", "user@email.com", "Password@123");
            Assert.True(tokens != null);
        }
    }

}
