using AuroraCore.Application.DTOs.Minidoc;
using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuroraCore.Application.Interfaces
{
    public interface IMinidocService
    {
        Task<MinidocResource> Create(Guid ownerId, MinidocCreationParams creationParams);

        Task<MinidocResource> Edit(Guid ownerId, Guid minidocId, MinidocEditionParams editionParams);

        Task Delete(Guid ownerId, Guid minidocId);

        Task<IEnumerable<MinidocCompact>> GetByChannel(Guid channelId);

        Task<MinidocResource> Get(Guid minidocId);

        Task<IEnumerable<MinidocCategory>> GetAvailableCategories();

        Task UpdateStreamProcessingStatus(string minidocId, VideoProcessingStatus processingStatus);
    }
}