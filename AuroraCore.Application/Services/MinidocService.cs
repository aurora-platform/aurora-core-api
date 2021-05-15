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
    private readonly IMinidocRepository _minidocRepository;

    public MinidocService(IChannelRepository channelRepository, IMinidocRepository minidocRepository, IUserRepository userRepository)
    {
        _channelRepository = channelRepository;
        _minidocRepository = minidocRepository;
        _userRepository = userRepository;
    }

    public void Create(Guid ownerId, MinidocCreationParams creationParams)
    {
      var owner = _userRepository.FindByID(ownerId);
      var channel = _channelRepository.FindByID(creationParams.ChannelId);

      var minidoc = new Minidoc(creationParams.Title, creationParams.Description);

    }

    public void Delete(Guid ownerId, MinidocCreationParams creationParams)
    {
      throw new NotImplementedException();
    }

    public void Edit(Guid ownerId, MinidocCreationParams creationParams)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<MinidocResource> GetByChannelId(Guid channelId)
    {
      throw new NotImplementedException();
    }

    public MinidocResource GetById(Guid minidocId)
    {
      throw new NotImplementedException();
    }
  }
}