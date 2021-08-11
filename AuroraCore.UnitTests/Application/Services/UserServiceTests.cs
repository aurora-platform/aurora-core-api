using AuroraCore.Application.DTOs;
using AuroraCore.Application.Services;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using AuroraCore.Infrastructure.Factories;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.UnitTests.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static AuroraCore.Application.Services.PasswordService;

namespace AuroraCore.UnitTests.Application.Services
{
    public class UserServiceTests : IDisposable
    {
        private UserService _userService;
        private AuthenticationService _authenticationService;

        public UserServiceTests()
        {
            var repository = new UserRepositoryMock();
            _userService = new UserService(repository, MapperFactory.Create(), new BcryptHashProvider());
            _authenticationService = new AuthenticationService(repository, new BcryptHashProvider(), MapperFactory.Create());
        }

        public void Dispose()
        {
            _userService = null;
            _authenticationService = null;
        }

        /**
         * SetupInitialSettings
         */

        [Fact]
        public void ShouldNot_SetupInitialSettings_WhenUserNotExists()
        {
            Assert.Throws<ValidationException>(() => _userService.SetupInitialSettings(Guid.NewGuid(), "User Name", new List<Topic>()));
        }

        [Fact]
        public void ShouldNot_SetupInitialSettings_WithoutLikedTopics()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");
            Assert.Throws<ValidationException>(() => _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic>()));
        }

        [Fact]
        public void ShouldNot_SetupInitialSettings_WhenIsAlreadyConfigured()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            var likedTopics = new List<Topic>
            {
                new Topic("Science")
            };

            _userService.SetupInitialSettings(createdUser.Id, "User Name", likedTopics);

            Assert.Throws<ValidationException>(() => _userService.SetupInitialSettings(createdUser.Id, "User Name", likedTopics));
        }

        [Fact]
        public void ShouldNot_SetupInitialSettings_WithoutName()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            var likedTopics = new List<Topic>
            {
                new Topic("Science")
            };

            Assert.Throws<ValidationException>(() => _userService.SetupInitialSettings(createdUser.Id, "", likedTopics));
        }

        [Fact]
        public void Should_SetupInitialSettings_Successfully()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic>
            {
                new Topic("Science")
            });
        }

        /**
         * EditLikedTopics
         */

        [Fact]
        public void ShouldNot_EditLikedTopics_WhenUserNotExists()
        {
            Assert.Throws<ValidationException>(() => _userService.EditLikedTopics(Guid.NewGuid(), new List<Topic>
            {
                new Topic("Science")
            }));
        }

        [Fact]
        public void ShouldNot_EditLikedTopics_WhenUserIsInvalid()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            var likedTopics = new List<Topic>
            {
                new Topic("Science")
            };

            Assert.Throws<ValidationException>(() => _userService.EditLikedTopics(createdUser.Id, likedTopics));
        }

        [Fact]
        public void ShouldNot_EditLikedTopics_WhenTopicsIsEmpty()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic>
            {
                new Topic("Science")
            });

            var likedTopics = new List<Topic>();

            Assert.Throws<ValidationException>(() => _userService.EditLikedTopics(createdUser.Id, likedTopics));
        }

        [Fact]
        public void ShouldNot_EditLikedTopics_WhenTopicsIsNull()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic>
            {
                new Topic("Science")
            });

            Assert.Throws<ValidationException>(() => _userService.EditLikedTopics(createdUser.Id, null));
        }

        [Fact]
        public void Should_EditLikedTopics_ChangeTopics()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic>
            {
                new Topic("Science"),
                new Topic("Technology"),
                new Topic("Art")
            });

            var likedTopics = new List<Topic>
            {
                new Topic("Science"),
                new Topic("Technology")
            };

            _userService.EditLikedTopics(createdUser.Id, likedTopics);

            UserResource editedUser = _userService.Get(createdUser.Id);

            bool same = !editedUser.LikedTopics.Except(likedTopics).Any() && !likedTopics.Except(editedUser.LikedTopics).Any();

            Assert.True(same);
        }

        /**
         * GetProfile
         */

        [Fact]
        public void ShouldNot_GetProfile_WhenUserNotExists()
        {
            Assert.Throws<ValidationException>(() => _userService.Get(Guid.NewGuid()));
        }

        [Fact]
        public void Should_GetProfile_ReturnsProfile()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic>
            {
                new Topic("Science"),
                new Topic("Technology"),
                new Topic("Art")
            });

            UserResource user = _userService.Get(createdUser.Id);

            Assert.True(user != null);
        }

        /**
         * EditProfile
         */

        [Fact]
        public void EditProfile_ThrowsValidationException_WhenUsernameIsNullOrEmpty()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic>
            {
                new Topic("Science"),
                new Topic("Technology"),
                new Topic("Art")
            });

            Assert.Throws<ValidationException>(() => _userService.Edit(createdUser.Id, new UserEditionParams()
            {
                Username = null,
                AboutMe = createdUser.AboutMe,
                Name = createdUser.Name,
                Phone = createdUser.Phone
            }));
        }

        [Fact]
        public void EditProfile_ThrowsValidationException_WhenUserNotExists()
        {
            Assert.Throws<ValidationException>(() => _userService.Edit(Guid.NewGuid(), new UserEditionParams()));
        }

        [Fact]
        public void EditProfile_EditUsername_Successfully()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic> { new Topic("Science") });

            var newUsername = "username2";

            _userService.Edit(createdUser.Id, new UserEditionParams()
            {
                Username = newUsername,
                AboutMe = createdUser.AboutMe,
                Name = createdUser.Name,
                Phone = createdUser.Phone
            });

            UserResource user = _userService.Get(createdUser.Id);

            Assert.True(user.Username == newUsername);
        }

        /**
         * ChangePassword
         */

        [Fact]
        public void ChangePassword_ThrowsValidationException_WhenConfirmationAndNewAreNotEqual()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic> { new Topic("Science") });

            Assert.Throws<ValidationException>(() => _userService.ChangePassword(createdUser.Id, "Password@123", "Password2@123", "Password3@123"));
        }

        [Fact]
        public void ChangePassword_ThrowsValidationException_WhenCurrentAndNewAreEqual()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic> { new Topic("Science") });

            Assert.Throws<ValidationException>(() => _userService.ChangePassword(createdUser.Id, "Password@123", "Password@123", "Password@123"));
        }

        [Fact]
        public void ChangePassword_ThrowsInvalidPasswordException_WhenPasswordIsChanged()
        {
            UserResource createdUser = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(createdUser.Id, "User Name", new List<Topic> { new Topic("Science") });

            _userService.ChangePassword(createdUser.Id, "Password@123", "Password2@123", "Password2@123");

            Assert.Throws<InvalidPasswordException>(() => _authenticationService.AuthenticateWithPassword("username", "Password@123"));
        }

        /**
         * Create
         */

        [Fact]
        public void CreateUser_ThrowsValidationException_WithoutUsername()
        {
            Assert.Throws<ValidationException>(() => _userService.Create("", "user@email.com", "Password@123"));
        }

        [Fact]
        public void CreateUser_ThrowsValidationException_WithoutEmail()
        {
            Assert.Throws<ValidationException>(() => _userService.Create("user", "", "Password@123"));
        }

        [Fact]
        public void CreateUser_ThrowsValidationException_WithoutPassword()
        {
            Assert.Throws<ValidationException>(() => _userService.Create("user", "user@email.com", ""));
        }

        [Fact]
        public void CreateUser_ThrowsValidationException_WithExistentUsername()
        {
            _userService.Create("user", "user@email.com", "Password@123");
            Assert.Throws<ValidationException>(() => _userService.Create("user", "user2@email.com", "Password@123"));
        }

        [Fact]
        public void CreateUser_ThrowsValidationException_WithExistentEmail()
        {
            _userService.Create("user", "user@email.com", "Password@123");
            Assert.Throws<ValidationException>(() => _userService.Create("user2", "user@email.com", "Password@123"));
        }

        [Fact]
        public void CreateUser_ReturnCreatedUser_WhenSuccessFullyCreated()
        {
            UserResource createdUser = _userService.Create("user", "user@email.com", "Password@123");
            Assert.True(createdUser != null);
        }
    }
}