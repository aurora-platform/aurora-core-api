using AuroraCore.Application.Services;
using AuroraCore.Infrastructure.Providers;
using Xunit;
using static AuroraCore.Application.Services.PasswordService;

namespace AuroraCore.UnitTests.Application.Services
{
    public class PasswordServiceTests
    {
        private PasswordService _passwordService;

        public PasswordServiceTests()
        {
            _passwordService = new PasswordService(new BcryptHashProvider());
        }

        [Fact]
        public void ShouldNot_SignUp_WithPasswordLessThan6Characters()
        {
            Assert.Throws<InvalidPasswordException>(() => _passwordService.Protect("pass"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithPasswordWithout1UpperCaseAtLeast()
        {
            Assert.Throws<InvalidPasswordException>(() => _passwordService.Protect("password"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithPasswordWithout1NumberAtLeast()
        {
            Assert.Throws<InvalidPasswordException>(() => _passwordService.Protect("Password"));
        }

        [Fact]
        public void ShouldNot_SignUp_WithPasswordWithout1SpecialCharacterAtLeast()
        {
            Assert.Throws<InvalidPasswordException>(() => _passwordService.Protect("Password123"));
        }
    }
}
