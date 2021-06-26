using AuroraCore.Application.DTOs;
using AuroraCore.Application.DTOs.Channel;
using AuroraCore.Application.Services;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using AuroraCore.Infrastructure.Factories;
using AuroraCore.Infrastructure.Providers;
using AuroraCore.UnitTests.Infrastructure.Repositories;
using AuroraCore.UnitTests.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AuroraCore.UnitTests.Application.Services
{
    public class ChannelServiceTests : IDisposable
    {
        private UserService _userService;
        private ChannelService _channelService;

        public ChannelServiceTests()
        {
            var userRepository = new UserRepositoryMock();
            var mapper = MapperFactory.Create();
            _userService = new UserService(userRepository, mapper, new BcryptHashProvider());
            _channelService = new ChannelService(new ChannelRepositoryMock(), userRepository, new FakeImageStorageService(), mapper);
        }

        public void Dispose()
        {
            _userService = null;
            _channelService = null;
        }

        [Fact]
        public void Create_ThrowsValidationException_WhenOwnerNotExists()
        {
            var parameters = new ChannelCreationParams {
                Name = "Test channel",
                About = "Test channel description",
                ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVQYV2NgYAAAAAMAAWgmWQ0AAAAASUVORK5CYII="
            };

            Assert.Throws<ValidationException>(() => _channelService.Create(Guid.NewGuid(), parameters));
        }

        [Fact]
        public void Create_ThrowsValidationException_WithInvalidUser()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");

            var parameters = new ChannelCreationParams
            {
                Name = "Test channel",
                About = "Test channel description",
                ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVQYV2NgYAAAAAMAAWgmWQ0AAAAASUVORK5CYII="
            };

            Assert.Throws<ValidationException>(() => _channelService.Create(user.Id, parameters));
        }


        [Fact]
        public void Create_ThrowsValidationException_WhenNameIsEmpty()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic>
            {
                new Topic("Science")
            });

            var parameters = new ChannelCreationParams
            {
                Name = "",
                About = "Test channel description",
                ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVQYV2NgYAAAAAMAAWgmWQ0AAAAASUVORK5CYII="
            };

            Assert.Throws<ValidationException>(() => _channelService.Create(user.Id, parameters));
        }

        [Fact]
        public void Create_ThrowsValidationException_WhenNameIsNull()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic>
            {
                new Topic("Science")
            });

            var parameters = new ChannelCreationParams
            {
                About = "Test channel description",
                ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVQYV2NgYAAAAAMAAWgmWQ0AAAAASUVORK5CYII="
            };

            Assert.Throws<ValidationException>(() => _channelService.Create(user.Id, parameters));
        }

        [Fact]
        public void Create_CreateWithImage_WhenImageBase64IsDefined()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");

            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic>
            {
                new Topic("Science")
            });

            ChannelResource channel = _channelService.Create(user.Id, new ChannelCreationParams
            {
                Name = "Test",
                About = "Test channel description",
                ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVQYV2NgYAAAAAMAAWgmWQ0AAAAASUVORK5CYII="
            });

            ChannelResource createdChannel = _channelService.GetOne(channel.Id);
            Assert.NotNull(createdChannel.Image);
        }

        /**
         * Edit
         */

        [Fact]
        public void Edit_ThrowsValidationException_WhenOwnerNotExists()
        {
            Assert.Throws<ValidationException>(() => _channelService.Edit(Guid.NewGuid(), new ChannelEditionParams()));
        }

        [Fact]
        public void Edit_ThrowsValidationException_WhenOwnerIsNotValid()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");
            Assert.Throws<ValidationException>(() => _channelService.Edit(user.Id, new ChannelEditionParams()));
        }

        [Fact]
        public void Edit_ThrowsValidationException_WhenChannelNotExists()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic> { new Topic("Science") });

            var parameters = new ChannelEditionParams { Id = Guid.NewGuid() };

            Assert.Throws<ValidationException>(() => _channelService.Edit(user.Id, parameters));
        }

        [Fact]
        public void Edit_ThrowsValidationException_WhenUserIsNotChannelOwner()
        {
            UserResource user1 = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user1.Id, "User Name", new List<Topic> { new Topic("Science") });

            UserResource user2 = _userService.Create("username2", "user2@email.com", "Password@123");
            _userService.SetupInitialSettings(user2.Id, "User Name", new List<Topic> { new Topic("Science") });

            ChannelResource channel = _channelService.Create(user1.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel"
            });

            var parameters = new ChannelEditionParams { Id = channel.Id };

            Assert.Throws<ValidationException>(() => _channelService.Edit(user2.Id, parameters));
        }

        [Fact]
        public void Edit_ChangeChannel_WhenChangeData()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic> { new Topic("Science") });

            ChannelResource channel = _channelService.Create(user.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel"
            });

            var editionParams = new ChannelEditionParams
            {
                Id = channel.Id,
                Name = "Channel2",
                About = "About2",
            };

            _channelService.Edit(user.Id, editionParams);

            ChannelResource createdChannel = _channelService.GetOne(channel.Id);

            Assert.Equal(createdChannel.Name, editionParams.Name);
            Assert.Equal(createdChannel.About, editionParams.About);
        }

        /**
         * GetOne
         */

        [Fact]
        public void GetOne_ReturnsNull_WhenChannelNotExists()
        {
            ChannelResource foundChannel = _channelService.GetOne(Guid.NewGuid());
            Assert.Null(foundChannel);
        }

        [Fact]
        public void GetOne_ReturnsFoundChannel_WhenChannelExists()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic> { new Topic("Science") });

            ChannelResource createdChannel = _channelService.Create(user.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel"
            });

            ChannelResource foundChannel = _channelService.GetOne(createdChannel.Id);
            Assert.Equal(foundChannel.Id, createdChannel.Id);
        }

        /**
         * GetAllOwnedBy
         */

        [Fact]
        public void GetAllOwnedBy_ReturnsEmptyIEnumerable_WhenOwnerDontHaveChannelsOrNotExists()
        {
            IEnumerable<ChannelResource> channels = _channelService.GetAllOwnedBy(Guid.NewGuid());
            Assert.True(!channels.Any());
        }

        [Fact]
        public void GetAllOwnedBy_ReturnsUserChannels()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic> { new Topic("Science") });

            _channelService.Create(user.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel1"
            });

            _channelService.Create(user.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel2"
            });

            IEnumerable<ChannelResource> channels = _channelService.GetAllOwnedBy(user.Id);
            Assert.True(channels.Count() == 2);
        }

        /**
         * Delete
         */

        [Fact]
        public void Delete_DeleteChannel_WhenChannelExists()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic> { new Topic("Science") });

            _channelService.Create(user.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel1"
            });

            ChannelResource channel = _channelService.Create(user.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel2"
            });

            _channelService.Delete(user.Id, channel.Id);

            Assert.Null(_channelService.GetOne(channel.Id));
        }

        /**
         * ChangeImage
         */

        [Fact]
        public void ChangeImage_ThrowsValidationException_WhenOwnerNotExists()
        {
            Assert.Throws<ValidationException>(() => _channelService.ChangeImage(Guid.NewGuid(), Guid.NewGuid(), ""));
        }

        [Fact]
        public void ChangeImage_ThrowsValidationException_WhenChannelNotExists()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic> { new Topic("Science") });

            Assert.Throws<ValidationException>(() => _channelService.ChangeImage(user.Id, Guid.NewGuid(), ""));
        }

        [Fact]
        public void ChangeImage_ThrowsValidationException_WhenUserIsNotChannelOwner()
        {
            UserResource user1 = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user1.Id, "User Name", new List<Topic> { new Topic("Science") });

            UserResource user2 = _userService.Create("username2", "user2@email.com", "Password@123");
            _userService.SetupInitialSettings(user2.Id, "User Name", new List<Topic> { new Topic("Science") });

            ChannelResource channel = _channelService.Create(user1.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel"
            });

            Assert.Throws<ValidationException>(() => _channelService.ChangeImage(user2.Id, channel.Id, ""));
        }

        [Fact]
        public void ChangeImage_ThrowsValidationException_WhenImageIsNullOrEmpty()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic> { new Topic("Science") });

            ChannelResource channel = _channelService.Create(user.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel"
            });

            Assert.Throws<ValidationException>(() => _channelService.ChangeImage(user.Id, channel.Id, ""));
            Assert.Throws<ValidationException>(() => _channelService.ChangeImage(user.Id, channel.Id, " "));
            Assert.Throws<ValidationException>(() => _channelService.ChangeImage(user.Id, channel.Id, null));
        }

        [Fact]
        public void ChangeImage()
        {
            UserResource user = _userService.Create("username", "user@email.com", "Password@123");
            _userService.SetupInitialSettings(user.Id, "User Name", new List<Topic> { new Topic("Science") });

            ChannelResource channel = _channelService.Create(user.Id, new ChannelCreationParams
            {
                About = "About",
                Name = "Channel",
                ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVQYV2NgYAAAAAMAAWgmWQ0AAAAASUVORK5CYII=",
            });

            var imageBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVQYV2NgYAAAAAMAAWgmWQ0AAAAASUVORK5CYII=";

            _channelService.ChangeImage(user.Id, channel.Id, imageBase64);
            ChannelResource createdChannel = _channelService.GetOne(channel.Id);

            Assert.NotEqual(channel.Image.Uri, createdChannel.Image.Uri);
        }
    }
}
