using AuroraCore.Application.DTOs;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.Interfaces
{
    public interface IChannelService
    {
        void Create(Guid ownerId, ChannelCreationParams channelCreation);

        void Edit(ChannelEditionParams channel);

        void ChangeImage(Guid channelId, string imageBase64);

        void Delete(Guid ownerId);

        IEnumerable<ChannelResource> GetAllOwnedBy(Guid ownerId);

        ChannelResource GetOne(Guid channelId);
    }
}
