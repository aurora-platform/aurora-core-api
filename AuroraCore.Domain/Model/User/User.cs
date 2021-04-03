using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AuroraCore.Domain.Model
{
    public class User
    {
        public User() { }

        public User(string username, string email)
        {
            Validation.NotNullOrWhiteSpace(username, "Username is required");
            Validation.NotNullOrWhiteSpace(email, "Email is required");

            if (!Regex.IsMatch(email, @"\S+@\S+\.\S+"))
            {
                throw new ValidationException("Invalid email");
            }

            Id = Guid.NewGuid();
            Username = username;
            Email = email;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public string Email { get; private set; }

        public string Phone { get; private set; }

        public string ImageURL { get; private set; }

        public string AboutMe { get; private set; }

        public bool IsActivated { get; private set; }

        public bool IsConfigured { get; private set; }

        public IEnumerable<Channel> Channels { get; private set; }

        public IEnumerable<Topic> LikedTopics { get; private set; }

        public bool HasLikedTopics()
        {
            return LikedTopics != null && LikedTopics.Any();
        }

        public void SetAsConfigured()
        {
            IsConfigured = true;
        }

        public void SetAsActive()
        {
            IsActivated = true;
        }

        public void SetName(string name)
        {
            Validation.NotNullOrWhiteSpace(name, "Name is required");
            Name = name;
        }

        public void SetLikedTopics(IEnumerable<Topic> topics)
        {
            LikedTopics = topics;
        }

        public void SetPassword(string hashedPassword)
        {
            Password = hashedPassword;
        }

        public bool IsValid()
        {
            return IsConfigured && IsActivated;
        }
    }
}
