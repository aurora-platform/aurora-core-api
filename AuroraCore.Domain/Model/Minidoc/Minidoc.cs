using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public class Minidoc
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        public Channel Channel { get; }
        public IEnumerable<Topic> Topics { get; }
        public IEnumerable<MinidocCategory> Categories { get; }

        public Minidoc(string title, string description, Channel channel)
        {
            Id = new Guid();
            Title = title;
            Description = description;
            Channel = channel;
        }
    }
}
