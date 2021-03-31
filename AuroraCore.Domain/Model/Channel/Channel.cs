using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public class Channel
    {
        public Channel(string name, string description)
        {
            Validation.NotNullOrWhiteSpace(name, "Name is required");
            Validation.NotNullOrWhiteSpace(description, "Description is required");

            Id = new Guid();
            Name = name;
            Description = description;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public string ImageURL { get; }

        public IEnumerable<Minidoc> Minidocs { get; }

        public IEnumerable<Serie> Series { get; }

        public IEnumerable<Topic> Topics { get; private set; }

        public void SetTopics(IEnumerable<Topic> topics)
        {
            Topics = topics;
        }
    }
}
