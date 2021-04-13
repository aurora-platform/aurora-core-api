using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.Services
{
    public class ChannelService : IChannelService
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IImageStorageService _imageStorageService;
        private readonly IUserRepository _userRepository;
        private readonly IObjectMapper _mapper;

        public ChannelService(
            IChannelRepository channelRepository,
            IUserRepository userRepository,
            IImageStorageService imageStorageService,
            IObjectMapper mapper
        )
        {
            _channelRepository = channelRepository;
            _imageStorageService = imageStorageService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public void Create(Guid ownerId, ChannelCreationParams creationParams)
        {
            User user = _userRepository.FindByID(ownerId);

            if (user == null) throw new ValidationException("The user not exists");

            user.Validate();

            var channel = new Channel(user, creationParams.Name, creationParams.About);

            ImageReference imageReference = _imageStorageService.Store("teste", creationParams.ImageBase64);

            channel.SetImage(imageReference);

            _channelRepository.Store(channel);
        }

        public void Edit(ChannelEditionParams channel)
        {
            Channel findedChannel = _channelRepository.FindByID(channel.Id);

            if (findedChannel == null) throw new ValidationException("The specified channel was not found");

            findedChannel.SetName(channel.Name);
            findedChannel.SetAbout(channel.About);

            _channelRepository.Update(findedChannel);
        }

        public ChannelResource GetOne(Guid channelId)
        {
            Channel channel = _channelRepository.FindByID(channelId);

            return _mapper.Map<ChannelResource>(channel);
        }

        public IEnumerable<ChannelResource> GetAllOwnedBy(Guid ownerId)
        {
            IEnumerable<Channel> channels = _channelRepository.FindAllByOwnerId(ownerId);

            return _mapper.Map<IEnumerable<ChannelResource>>(channels);
        }

        public void Delete(Guid id)
        {
            _channelRepository.Delete(id);
        }

        public void ChangeImage(Guid channelId, string imageBase64)
        {
            Channel channel = _channelRepository.FindByID(channelId);

            if (channel == null) throw new ValidationException("The specified channel was not found");

            _imageStorageService.Delete(channel.Image);
            ImageReference newImage = _imageStorageService.Store("test", imageBase64);

            channel.ChangeImage(newImage);

            _channelRepository.Update(channel);
        }
    }
}
