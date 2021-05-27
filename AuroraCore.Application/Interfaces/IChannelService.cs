using AuroraCore.Application.DTOs.Channel;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.Interfaces
{
    public interface IChannelService
    {
        ChannelResource Create(Guid ownerId, ChannelCreationParams channelCreation);

        void Edit(Guid ownerId, ChannelEditionParams channelEdition);

        void ChangeImage(Guid ownerId, Guid channelId, string imageBase64);

        void Delete(Guid ownerId);

        IEnumerable<ChannelResource> GetAllOwnedBy(Guid ownerId);

        ChannelResource GetOne(Guid channelId);
    }
}
