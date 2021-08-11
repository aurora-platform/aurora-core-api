using AuroraCore.Domain.Shared;
using System;

namespace AuroraCore.Domain.Model
{
    public class Topic
    {
        public Topic()
        {
        }

        public Topic(string name)
        {
            Validate.NotNullOrWhiteSpace(name, "Name is required");
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string ImageURL { get; private set; }
    }
}