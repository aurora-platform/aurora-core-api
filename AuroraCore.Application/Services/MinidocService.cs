using System;
using System.Collections.Generic;
using AuroraCore.Application.DTOs.Minidoc;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;

namespace AuroraCore.Application.Services
{
    public class MinidocService : IMinidocService
    {
        private readonly IUserRepository _userRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IMinidocRepository _minidocRepository;
        private readonly IVideoStorageService _videoStorageService;
        private readonly IObjectMapper _mapper;

        public MinidocService(
          IChannelRepository channelRepository,
          ITopicRepository topicRepository,
          IMinidocRepository minidocRepository,
          IUserRepository userRepository,
          IObjectMapper mapper,
          IVideoStorageService videoStorageService
        )
        {
            _channelRepository = channelRepository;
            _topicRepository = topicRepository;
            _minidocRepository = minidocRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _videoStorageService = videoStorageService;
        }

        public void Create(Guid ownerId, MinidocCreationParams creationParams)
        {
            User owner = _userRepository.FindById(ownerId);
            Channel channel = _channelRepository.FindById(creationParams.ChannelId);
            IList<Topic> existingTopics = _topicRepository.GetByIds(creationParams.Topics);
            IList<MinidocCategory> existingCategories = _minidocRepository.GetCategoriesByIds(creationParams.Categories);

            VideoReference videoReference = _videoStorageService.Store("zeruela", creationParams.Video);

            var minidoc = new Minidoc(
                creationParams.Title,
                creationParams.Description,
                videoReference,
                channel,
                existingTopics,
                existingCategories
            );
            
            _minidocRepository.Store(minidoc);
        }
    
        public void Edit(Guid ownerId, Guid minidocId, MinidocEditionParams editionParams)
        {
            User owner = _userRepository.FindById(ownerId);
            Minidoc minidoc = _minidocRepository.FindById(minidocId);
            minidoc.SetChannel(_channelRepository.FindById(minidoc.Channel.Id));

            // TODO: cehck if this validation isnt domain responsability. 
            if (!minidoc.Channel.HasOwner(owner))
                throw new ValidationException("The user is not the owner");

            IList<Topic> existingTopics = _topicRepository.GetByIds(editionParams.Topics);
            IList<MinidocCategory> existingCategories = _minidocRepository.GetCategoriesByIds(editionParams.Categories);

            if (editionParams.Video != null)
            {
                VideoReference newVideoReference = _videoStorageService.Store("zeruela", editionParams.Video);
                minidoc.SetVideo(newVideoReference);
            }

            minidoc.SetTitle(editionParams.Title);
            minidoc.SetDescription(editionParams.Description);
            minidoc.SetTopics(existingTopics);
            minidoc.SetCategories(existingCategories);

            _minidocRepository.Update(minidoc);
        }

        public void Delete(Guid ownerId, Guid minidocId)
        {
            User owner = _userRepository.FindById(ownerId);
            Minidoc minidoc = _minidocRepository.FindById(minidocId);
            minidoc.SetChannel(_channelRepository.FindById(minidoc.Channel.Id));

            if (!minidoc.Channel.HasOwner(owner))
                throw new ValidationException("The user is not the owner");

            _minidocRepository.Delete(minidocId);
        }

        public IEnumerable<MinidocCompact> GetByChannel(Guid channelId)
        {
            IEnumerable<Minidoc> minidocs = _minidocRepository.FindByChannel(channelId);
            return _mapper.Map<IEnumerable<MinidocCompact>>(minidocs);
        }

        public MinidocResource Get(Guid minidocId)
        {
            Minidoc minidoc = _minidocRepository.FindById(minidocId);
            return _mapper.Map<MinidocResource>(minidoc);
        }

        public IEnumerable<MinidocCategory> GetAvailableCategories()
        {
            return _minidocRepository.GetAllCategories();
        }
    }
}