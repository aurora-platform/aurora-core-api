﻿using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
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

namespace AuroraCore.UnitTests.Application.Services
{
    public class UserServiceTests
    {
        public static IUserService GetUserService(UserRepositoryMock repository)
        {
            return new UserService(repository, MapperFactory.Create(), new BcryptHashProvider());
        }

        public static IAuthenticationService GetAuthService(UserRepositoryMock repository)
        {
            return new AuthenticationService(repository, new BcryptHashProvider());
        }

        /**
         * SetupInitialSettings
         */

        [Fact]
        public void ShouldNot_SetupInitialSettings_WhenUserNotExists()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            repository.Store(new User("username", "user@email.com"));

            var likedTopics = new List<Topic>();

            Assert.Throws<ValidationException>(() => userService.SetupInitialSettings(Guid.NewGuid(), "User Name", likedTopics));
        }

        [Fact]
        public void ShouldNot_SetupInitialSettings_WithoutLikedTopics()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();

            repository.Store(user);

            var likedTopics = new List<Topic>();

            Assert.Throws<ValidationException>(() => userService.SetupInitialSettings(user.Id, "User Name", likedTopics));
        }

        [Fact]
        public void ShouldNot_SetupInitialSettings_WhenIsNotActivated()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");

            repository.Store(user);

            var likedTopics = new List<Topic>
            {
                new Topic("Science")
            };

            Assert.Throws<ValidationException>(() => userService.SetupInitialSettings(user.Id, "User Name", likedTopics));
        }

        [Fact]
        public void ShouldNot_SetupInitialSettings_WhenIsAlreadyConfigured()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();
            user.SetAsConfigured();

            repository.Store(user);

            var likedTopics = new List<Topic>
            {
                new Topic("Science")
            };

            Assert.Throws<ValidationException>(() => userService.SetupInitialSettings(user.Id, "User Name", likedTopics));
        }

        [Fact]
        public void ShouldNot_SetupInitialSettings_WithoutName()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();

            repository.Store(user);

            var likedTopics = new List<Topic>
            {
                new Topic("Science")
            };

            Assert.Throws<ValidationException>(() => userService.SetupInitialSettings(user.Id, "", likedTopics));
        }

        [Fact]
        public void Should_SetupInitialSettings_Successfully()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();

            repository.Store(user);

            var likedTopics = new List<Topic>
            {
                new Topic("Science")
            };

            userService.SetupInitialSettings(user.Id, "User Name", likedTopics);
        }

        /**
         * EditLikedTopics 
         */

        [Fact]
        public void ShouldNot_EditLikedTopics_WhenUserNotExists()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var likedTopics = new List<Topic>
            {
                new Topic("Science")
            };

            Assert.Throws<ValidationException>(() => userService.EditLikedTopics(Guid.NewGuid(), likedTopics));
        }

        [Fact]
        public void ShouldNot_EditLikedTopics_WhenUserIsInvalid()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();

            repository.Store(user);

            var likedTopics = new List<Topic>
            {
                new Topic("Science")
            };

            Assert.Throws<ValidationException>(() => userService.EditLikedTopics(user.Id, likedTopics));
        }

        [Fact]
        public void ShouldNot_EditLikedTopics_WhenTopicsIsEmpty()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();
            user.SetAsConfigured();

            repository.Store(user);

            var likedTopics = new List<Topic>();

            Assert.Throws<ValidationException>(() => userService.EditLikedTopics(user.Id, likedTopics));
        }

        [Fact]
        public void ShouldNot_EditLikedTopics_WhenTopicsIsNull()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();
            user.SetAsConfigured();

            repository.Store(user);

            Assert.Throws<ValidationException>(() => userService.EditLikedTopics(user.Id, null));
        }

        [Fact]
        public void Should_EditLikedTopics_ChangeTopics()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");

            user.SetLikedTopics(new List<Topic>
            {
                new Topic("Science"),
                new Topic("Technology"),
                new Topic("Art")
            });
            user.SetAsActive();
            user.SetAsConfigured();

            repository.Store(user);

            var likedTopics = new List<Topic>
            {
                new Topic("Science"),
                new Topic("Technology")
            };

            userService.EditLikedTopics(user.Id, likedTopics);

            bool same = !user.LikedTopics.Except(likedTopics).Any() && !likedTopics.Except(user.LikedTopics).Any();

            Assert.True(same);
        }

        /**
         * GetProfile 
         */

        [Fact]
        public void ShouldNot_GetProfile_WhenUserNotExists()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();
            user.SetAsConfigured();

            repository.Store(user);

            Assert.Throws<ValidationException>(() => userService.GetProfile(Guid.NewGuid()));
        }

        [Fact]
        public void ShouldNot_GetProfile_WhenUserIsInvalid()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();

            repository.Store(user);

            Assert.Throws<ValidationException>(() => userService.GetProfile(user.Id));
        }

        [Fact]
        public void Should_GetProfile_ReturnsProfile()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();
            user.SetAsConfigured();

            repository.Store(user);

            UserProfile userProfile = userService.GetProfile(user.Id);

            Assert.True(userProfile != null);
        }

        /**
         * EditProfile 
         */

        [Fact]
        public void ShouldNot_EditProfile_ChangeEmail()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();
            user.SetAsConfigured();

            repository.Store(user);

            var userProfile = new UserProfile(user)
            {
                Email = "user2@email.com"
            };

            userService.GetProfile(user.Id);

            User updatedUser = repository.FindByID(user.Id);

            Assert.True(updatedUser.Email == user.Email);
        }


        [Fact]
        public void ShouldNot_EditProfile_WhenUserNotExists()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid()
            };

            Assert.Throws<ValidationException>(() => userService.EditProfile(userProfile));
        }

        [Fact]
        public void Should_EditProfile_Successfully()
        {
            var repository = new UserRepositoryMock();
            var userService = GetUserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();
            user.SetAsConfigured();

            repository.Store(user);

            var userProfile = new UserProfile(user)
            {
                Username = "username2"
            };

            userService.EditProfile(userProfile);

            User updatedUser = repository.FindByID(user.Id);

            Assert.True(updatedUser.Username == userProfile.Username);
        }

        /**
         * ChangePassword 
         */

        [Fact]
        public void ShouldNot_ChangePassword_WhenConfirmationAndNewAreNotEqual()
        {
            var repository = new UserRepositoryMock();
            var authService = GetAuthService(repository);
            var userService = GetUserService(repository);

            User createdUser = authService.SignUp("user", "user@email.com", "Password@123");
            createdUser.SetAsConfigured();
            repository.Update(createdUser);

            Assert.Throws<ValidationException>(() => userService.ChangePassword(createdUser.Id, "Password@123", "Password2@123", "Password3@123"));
        }

        [Fact]
        public void ShouldNot_ChangePassword_WhenCurrentAndNewAreEqual()
        {
            var repository = new UserRepositoryMock();
            var authService = GetAuthService(repository);
            var userService = GetUserService(repository);

            User createdUser = authService.SignUp("user", "user@email.com", "Password@123");
            createdUser.SetAsConfigured();
            repository.Update(createdUser);

            Assert.Throws<ValidationException>(() => userService.ChangePassword(createdUser.Id, "Password@123", "Password@123", "Password@123"));
        }

        [Fact]
        public void Should_ChangePassword_Successfully()
        {
            var repository = new UserRepositoryMock();
            var authService = GetAuthService(repository);
            var userService = GetUserService(repository);

            User createdUser = authService.SignUp("user", "user@email.com", "Password@123");
            string oldPassword = createdUser.Password;
            createdUser.SetAsConfigured();
            repository.Update(createdUser);

            userService.ChangePassword(createdUser.Id, "Password@123", "Password2@123", "Password2@123");

            User updatedUser = repository.FindByID(createdUser.Id);

            Assert.True(updatedUser.Password != oldPassword);
        }
    }
}
