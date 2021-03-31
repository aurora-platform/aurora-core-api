using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public class Minidoc
    {
        public Minidoc(string title, string description, IEnumerable<Topic> topics)
        {
            Id = new Guid();
            Title = title;
            Description = description;
            Topics = topics;
        }

        public Guid Id { get; }

        public string Title { get; }

        public string Description { get; }

        public IEnumerable<Topic> Topics { get; }

        public IEnumerable<MinidocCategory> Categories { get; }
    }
}
