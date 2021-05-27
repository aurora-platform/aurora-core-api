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
        private readonly IObjectMapper _mapper;

        public MinidocService(
          IChannelRepository channelRepository,
          ITopicRepository topicRepository,
          IMinidocRepository minidocRepository,
          IUserRepository userRepository,
          IObjectMapper mapper
        )
        {
            _channelRepository = channelRepository;
            _topicRepository = topicRepository;
            _minidocRepository = minidocRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public void Create(Guid ownerId, MinidocCreationParams creationParams)
        {
            User owner = _userRepository.FindById(ownerId);
            Channel channel = _channelRepository.FindById(creationParams.ChannelId);

            IEnumerable<Topic> existingTopics = _topicRepository.GetByIds(creationParams.Topics);
            IEnumerable<MinidocCategory> existingCategories = _minidocRepository.GetCategoriesByIds(creationParams.Categories);

            _minidocRepository.Store(new Minidoc(
                creationParams.Title,
                creationParams.Description,
                channel,
                existingTopics,
                existingCategories
            ));
        }
    
        public void Edit(Guid ownerId, MinidocEditionParams editionParams)
        {
            User owner = _userRepository.FindById(ownerId);
            Minidoc minidoc = _minidocRepository.FindById(editionParams.MinidocId);

            if (!minidoc.Channel.HasOwner(owner))
                throw new ValidationException("The user is not the owner");

            IEnumerable<Topic> existingTopics = _topicRepository.GetByIds(editionParams.Topics);
            IEnumerable<MinidocCategory> existingCategories = _minidocRepository.GetCategoriesByIds(editionParams.Categories);

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

            if (!minidoc.Channel.HasOwner(owner))
                throw new ValidationException("The user is not the owner");

            _minidocRepository.Delete(minidocId);
        }

        public IEnumerable<MinidocResource> GetByChannel(Guid channelId)
        {
            IEnumerable<Minidoc> minidocs = _minidocRepository.FindByChannel(channelId);
            return _mapper.Map<IEnumerable<MinidocResource>>(minidocs);
        }

        public MinidocResource Get(Guid minidocId)
        {
            Minidoc minidoc = _minidocRepository.FindById(minidocId);
            return _mapper.Map<MinidocResource>(minidoc);
        }
    }
}