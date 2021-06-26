using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface IChannelRepository
    {
        void Store(Channel channel);
        void Update(Channel channel);
        int Delete(Guid id);
        Channel FindById(Guid id);
        IEnumerable<Channel> FindAllByOwnerId(Guid ownerId);
    }
}
