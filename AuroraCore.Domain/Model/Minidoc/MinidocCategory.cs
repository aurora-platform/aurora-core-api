using AuroraCore.Domain.Shared;
using System;

namespace AuroraCore.Domain.Model
{
    public class MinidocCategory
    {
        public MinidocCategory() { }

        public MinidocCategory(string name, string imageURL)
        {
            Validate.NotNullOrWhiteSpace(name, "Name is required");
            Validate.NotNullOrWhiteSpace(imageURL, "ImageURL is required");

            Id = new Guid();
            Name = name;
            ImageURL = imageURL;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string ImageURL { get; }
    }
}
