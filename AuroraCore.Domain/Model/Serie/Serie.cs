using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public class Serie
    {
        public Serie(string name, string description, IEnumerable<Minidoc> minidocs, IEnumerable<Topic> topics)
        {
            Id = new Guid();
            Name = name;
            Description = description;
            Minidocs = minidocs;
            Topics = topics;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public IEnumerable<Minidoc> Minidocs { get; }

        public IEnumerable<Topic> Topics { get; }
    }
}
