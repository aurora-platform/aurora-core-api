using System;
using System.Collections.Generic;
using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;

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
      var owner = _userRepository.FindById(ownerId);
      var channel = _channelRepository.FindById(creationParams.ChannelId);

      var minidoc = new Minidoc(creationParams.Title, creationParams.Description, channel);

      _minidocRepository.Store(minidoc);
    }
  }
}