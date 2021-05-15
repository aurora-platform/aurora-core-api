using System;
using System.Collections.Generic;
using AuroraCore.Application.DTOs;

namespace AuroraCore.Application.Interfaces 
{
    public interface IMinidocService {
      void Create(Guid ownerId, MinidocCreationParams creationParams);
      void Edit(Guid ownerId, MinidocCreationParams creationParams);
      void Delete(Guid ownerId, MinidocCreationParams creationParams);
      MinidocResource GetById(Guid minidocId);
      IEnumerable<MinidocResource> GetByChannelId(Guid channelId);
    }
}