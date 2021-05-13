using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuroraCore.UnitTests.Infrastructure.Repositories
{
    public class ChannelRepositoryMock : IChannelRepository
    {
        public readonly List<Channel> channels;

        public ChannelRepositoryMock()
        {
            channels = new List<Channel>();
        }

        public int Delete(Guid id)
        {
            var found = channels.Where(channel => channel.Id == id).FirstOrDefault();
            channels.Remove(found);
            return 1;
        }

        public IEnumerable<Channel> FindAllByOwnerId(Guid ownerId)
        {
            return channels.Where(channel => channel.Owner.Id == ownerId);
        }

        public Channel FindByID(Guid id)
        {
            return channels.FirstOrDefault(channel => channel.Id == id);
        }

        public IEnumerable<Channel> GetAll()
        {
            return channels;
        }

        public void Store(Channel entity)
        {
            channels.Add(entity);
        }

        public void Update(Channel entity)
        {
            Channel channel = channels.FirstOrDefault(channel => channel.Id == entity.Id);
            if (channel != null) channel = entity;
        }
    }
}
