using System;
using System.Collections.Generic;
using AuroraCore.Application.DTOs.Minidoc;
using AuroraCore.Domain.Model;

namespace AuroraCore.Application.Interfaces 
{
    public interface IMinidocService {
        void Create(Guid ownerId, MinidocCreationParams creationParams);
        void Edit(Guid ownerId, Guid minidocId, MinidocEditionParams editionParams);
        void Delete(Guid ownerId, Guid minidocId);
        IEnumerable<MinidocCompact> GetByChannel(Guid channelId);
        MinidocResource Get(Guid minidocId);
        IEnumerable<MinidocCategory> GetAvailableCategories();
    }
}