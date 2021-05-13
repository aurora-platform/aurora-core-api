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

        public ChannelResource Create(Guid ownerId, ChannelCreationParams creationParams)
        {
            User owner = _userRepository.FindByID(ownerId);
            var channel = new Channel(owner, creationParams.Name, creationParams.About);

            if (!string.IsNullOrEmpty(creationParams.ImageBase64))
            {
                ImageReference imageReference = _imageStorageService.Store("test", creationParams.ImageBase64);
                channel.SetImage(imageReference);
            }

            _channelRepository.Store(channel);
            
            return _mapper.Map<ChannelResource>(channel);
        }

        public void Edit(Guid ownerId, ChannelEditionParams editionParams)
        {
            User owner = _userRepository.FindByID(ownerId);
            Channel channel = _channelRepository.FindByID(editionParams.Id);

            if (channel is null)
                throw new ValidationException("The specified channel was not found");

            if (!channel.HasOwner(owner))
                throw new ValidationException("This user is not the owner of this channel");

            channel.SetName(editionParams.Name);
            channel.SetAbout(editionParams.About);
            
            _channelRepository.Update(channel);
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

        public void ChangeImage(Guid ownerId, Guid channelId, string imageBase64)
        {
            User owner = _userRepository.FindByID(ownerId);
            Channel channel = _channelRepository.FindByID(channelId);

            if (channel == null)
                throw new ValidationException("The specified channel was not found");

            if (!channel.HasOwner(owner))
                throw new ValidationException("This user is not the owner of this channel");

            if (string.IsNullOrWhiteSpace(imageBase64))
                throw new ValidationException("The image is required");

            _imageStorageService.Delete(channel.Image);
            ImageReference newImage = _imageStorageService.Store("test", imageBase64);

            channel.ChangeImage(newImage);
            _channelRepository.Update(channel);
        }
    }
}
