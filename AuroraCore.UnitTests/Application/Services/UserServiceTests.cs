using AuroraCore.Application.Services;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using AuroraCore.UnitTests.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
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
    }
}
