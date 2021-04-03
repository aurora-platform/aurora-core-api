using AuroraCore.Application.DTOs;
using AuroraCore.Application.Services;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using AuroraCore.UnitTests.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AuroraCore.UnitTests.Application.Services
{
    public class UserServiceTests
    {
        /**
         * SetupInitialSettings
         */

        [Fact]
        public void ShouldNot_SetupInitialSettings_WhenUserNotExists()
        {
            var repository = new UserRepositoryMock();
            var userService = new UserService(repository);

            repository.Store(new User("username", "user@email.com"));

            var likedTopics = new List<Topic>();

            Assert.Throws<ValidationException>(() => userService.SetupInitialSettings(Guid.NewGuid(), "User Name", likedTopics));
        }

        [Fact]
        public void ShouldNot_SetupInitialSettings_WithoutLikedTopics()
        {
            var repository = new UserRepositoryMock();
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

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
            var userService = new UserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();

            repository.Store(user);

            Assert.Throws<ValidationException>(() => userService.GetProfile(user.Id));
        }

        [Fact]
        public void Should_GetProfile_ReturnsProfile()
        {
            var repository = new UserRepositoryMock();
            var userService = new UserService(repository);

            var user = new User("username", "user@email.com");
            user.SetAsActive();
            user.SetAsConfigured();

            repository.Store(user);

            UserProfile userProfile = userService.GetProfile(user.Id);

            Assert.True(userProfile != null);
        }
    }
}
