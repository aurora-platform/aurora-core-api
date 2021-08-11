using AuroraCore.Application.DTOs.Minidoc;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuroraCore.Application.Services
{
    public class MinidocService : IMinidocService
    {
        private readonly IUserRepository _userRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IMinidocRepository _minidocRepository;
        private readonly IObjectMapper _mapper;
        private readonly ITranscodingService _transcoder;

        public MinidocService(
            IChannelRepository channelRepository,
            ITopicRepository topicRepository,
            IMinidocRepository minidocRepository,
            IUserRepository userRepository,
            IObjectMapper mapper,
            ITranscodingService transcoder
        )
        {
            _channelRepository = channelRepository;
            _topicRepository = topicRepository;
            _minidocRepository = minidocRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _transcoder = transcoder;
        }

        public async Task<MinidocResource> Create(Guid ownerId, MinidocCreationParams creationParams)
        {
            User owner = _userRepository.FindById(ownerId);
            Channel channel = _channelRepository.FindById(creationParams.ChannelId);

            if (!channel.HasOwner(owner))
                throw new ValidationException("The user is not the owner");

            IList<Topic> existingTopics = _topicRepository.GetByIds(creationParams.Topics);
            IList<MinidocCategory> existingCategories = _minidocRepository.GetCategoriesByIds(creationParams.Categories);

            var minidoc = new Minidoc(
                creationParams.Title,
                creationParams.Description,
                10,
                channel,
                existingTopics,
                existingCategories
            );

            await _transcoder.CreateJob(minidoc.Id.ToString(), "mp4", creationParams.Video);

            _minidocRepository.Store(minidoc);

            return _mapper.Map<MinidocResource>(minidoc);
        }

        public async Task<MinidocResource> Edit(Guid ownerId, Guid minidocId, MinidocEditionParams editionParams)
        {
            User owner = _userRepository.FindById(ownerId);
            Minidoc minidoc = _minidocRepository.FindById(minidocId);
            minidoc.SetChannel(_channelRepository.FindById(minidoc.Channel.Id));

            if (!minidoc.Channel.HasOwner(owner))
                throw new ValidationException("The user is not the owner");

            IList<Topic> existingTopics = _topicRepository.GetByIds(editionParams.Topics);
            IList<MinidocCategory> existingCategories = _minidocRepository.GetCategoriesByIds(editionParams.Categories);

            //if (editionParams.Video != null)
            //{
            //    VideoReference newVideoReference = await _videoStorage.Store(editionParams.Video);
            //    minidoc.SetVideo(newVideoReference);
            //}

            minidoc.SetTitle(editionParams.Title);
            minidoc.SetDescription(editionParams.Description);
            minidoc.SetTopics(existingTopics);
            minidoc.SetCategories(existingCategories);

            _minidocRepository.Update(minidoc);

            return _mapper.Map<MinidocResource>(minidoc);
        }

        public async Task Delete(Guid ownerId, Guid minidocId)
        {
            User owner = _userRepository.FindById(ownerId);
            Minidoc minidoc = _minidocRepository.FindById(minidocId);
            minidoc.SetChannel(_channelRepository.FindById(minidoc.Channel.Id));

            if (!minidoc.Channel.HasOwner(owner))
                throw new ValidationException("The user is not the owner");

            _minidocRepository.Delete(minidocId);

            return;
        }

        public async Task<IEnumerable<MinidocCompact>> GetByChannel(Guid channelId)
        {
            IEnumerable<Minidoc> minidocs = _minidocRepository.FindByChannel(channelId);
            return _mapper.Map<IEnumerable<MinidocCompact>>(minidocs);
        }

        public async Task<MinidocResource> Get(Guid minidocId)
        {
            Minidoc minidoc = _minidocRepository.FindById(minidocId);
            return _mapper.Map<MinidocResource>(minidoc);
        }

        public async Task<IEnumerable<MinidocCategory>> GetAvailableCategories()
        {
            return _minidocRepository.GetAllCategories();
        }

        public async Task UpdateStreamProcessingStatus(Guid minidocId, VideoProcessingStatus status)
        {
            Minidoc minidoc = _minidocRepository.FindById(minidocId);

            if (status == VideoProcessingStatus.Ready)
            {
                minidoc.PrepareToStream();
            }
            else if (status == VideoProcessingStatus.Failed)
            {
                minidoc.MarkAsFailedToStream();
            }

            _minidocRepository.Update(minidoc);
        }
    }
}