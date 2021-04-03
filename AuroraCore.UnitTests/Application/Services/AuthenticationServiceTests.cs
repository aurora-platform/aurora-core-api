using AuroraCore.Application.Services;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.UnitTests.Infrastructure.Repositories;
using Xunit;

namespace AuroraCore.UnitTests.Application.Services
{

    public class AuthenticationServiceTests
    {
        private AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            _authenticationService = new AuthenticationService(new UserRepositoryMock(), new BcryptHashProvider());
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
            var authenticationService = new AuthenticationService(new UserRepositoryMock(), new BcryptHashProvider());

            authenticationService.SignUp("user", "user@email.com", "Password@123");

            Assert.Throws<ValidationException>(() => authenticationService.AuthenticateWithPassword("user", "Password@121"));
        }

        [Fact]
        public void Shouldt_AuthenticateWithPassword_ReturnAuthenticatedUser()
        {
            var authenticationService = new AuthenticationService(new UserRepositoryMock(), new BcryptHashProvider());

            authenticationService.SignUp("user", "user@email.com", "Password@123");

            User authenticatedUser = authenticationService.AuthenticateWithPassword("user", "Password@123");

            Assert.True(authenticatedUser != null);
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
            User authenticatedUser = _authenticationService.SignUp("user", "user@email.com", "Password@123");
            Assert.True(authenticatedUser != null);
        }
    }

}
