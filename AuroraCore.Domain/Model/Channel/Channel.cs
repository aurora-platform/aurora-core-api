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

        public Channel() { }

        public Channel(User owner, string name, string about = null, ImageReference image = null)
        {
            if (owner == null) throw new ValidationException("The owner not exists");

            owner.Validate();

            Validation.NotNullOrWhiteSpace(name, "Name is required");

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
    }
}
