using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuroraCore.Domain.Model
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public ImageReference Image { get; private set; }
        public string AboutMe { get; private set; }
        public bool IsActivated { get; private set; }
        public bool IsConfigured { get; private set; }
        public IEnumerable<Topic> LikedTopics { get; private set; }

        public User() { }

        public User(string username, string email, string name)
        {
            Validation.NotNullOrWhiteSpace(username, "Username is required");
            Validation.NotNullOrWhiteSpace(email, "Email is required");
            Validation.NotNullOrWhiteSpace(name, "Name is required");

            if (!Regex.IsMatch(email, @"\S+@\S+\.\S+"))
                throw new ValidationException("Invalid email");

            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            Name = name;
        }

        public User(
            string name,
            string username,
            string password,
            string email,
            string phone,
            ImageReference image,
            string about,
            bool isActivated,
            bool isConfigured,
            IEnumerable<Topic> likedTopics
        )
        {
            Validation.NotNullOrWhiteSpace(username, "Username is required");
            Validation.NotNullOrWhiteSpace(email, "Email is required");

            if (!Regex.IsMatch(email, @"\S+@\S+\.\S+"))
                throw new ValidationException("Invalid email");

            Id = Guid.NewGuid();
            Name = name;
            Username = username;
            Password = password;
            Phone = phone;
            Email = email;
            Image = image;
            AboutMe = about;
            IsActivated = isActivated;
            IsConfigured = isConfigured;
            LikedTopics = likedTopics;
        }

        public bool HasLikedTopics() => LikedTopics != null && LikedTopics.Any();

        public void SetAsConfigured() => IsConfigured = true;

        public void SetAsActive() =>  IsActivated = true;

        public void SetLikedTopics(IEnumerable<Topic> topics) => LikedTopics = topics;

        public void SetPassword(string hashedPassword) => Password = hashedPassword;

        public void SetEmail(string email)
        {
            Validation.NotNullOrWhiteSpace(email, "Email is required");
            Email = email;
        }

        public void SetName(string name)
        {
            Validation.NotNullOrWhiteSpace(name, "Name is required");
            Name = name;
        }

        public void SetUsername(string username)
        {
            Validation.NotNullOrWhiteSpace(username, "Username is required");
            Username = username;
        }

        public void SetPhone(string phone)
        {
            Phone = phone;
        }

        public void SetAboutMe(string about)
        {
            AboutMe = about;
        }

        public void Validate()
        {
            if (!IsConfigured || !IsActivated)
            {
                throw new ValidationException("Invalid user");
            }
        }

        public bool IsValid()
        {
            return IsConfigured && IsActivated;
        }

        public static bool operator ==(User obj1, User obj2)
        {
            if (obj1 is null)
                return obj2 is null;

            return obj1.Equals(obj2);
        }

        public static bool operator !=(User obj1, User obj2)
        {
            if (obj1 is null)
                return false;

            return !obj1.Equals(obj2);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;

            var user = (User)obj;
            return user.Id == Id;
        }
    }
}
