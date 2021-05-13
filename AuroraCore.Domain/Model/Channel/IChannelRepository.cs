using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface IChannelRepository : IRepository<Channel>
    {
        IEnumerable<Channel> FindAllByOwnerId(Guid ownerId);
    }
}
