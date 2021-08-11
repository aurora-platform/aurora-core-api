using AuroraCore.Domain.Shared;
using System;

namespace AuroraCore.Domain.Model
{
    public class Channel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string About { get; private set; }
        public User Owner { get; private set; }
        public ImageReference Image { get; private set; }

        public Channel()
        {
        }

        public Channel(User owner, string name, string about = null, ImageReference image = null)
        {
            Validate.NotNull(owner, "Owner is required");
            Validate.NotNullOrWhiteSpace(name, "Name is required");

            if (!owner.IsValid())
                throw new ValidationException("Owner is not valid");

            Id = Guid.NewGuid();
            Owner = owner;
            Name = name;
            Image = image;
            About = about;
        }

        public void SetImage(ImageReference image)
        {
            Image = image;
        }

        public void ChangeImage(ImageReference image)
        {
            Image.ChangeReference(image);
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetAbout(string about)
        {
            About = about;
        }

        public void SetOwner(User owner)
        {
            Owner = owner;
        }

        public bool HasOwner(User owner)
        {
            Validate.NotNull(owner, "Owner is required");

            if (!owner.IsValid())
                throw new ValidationException("The owner is invalid");

            return Owner == owner;
        }
    }
}