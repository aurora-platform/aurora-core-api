using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.DTOs
{
    public class UserProfile
    {
        public UserProfile(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Username = user.Username;
            Email = user.Email;
            Phone = user.Phone;
            ImageURL = user.ImageURL;
            AboutMe = user.AboutMe;
            LikedTopics = user.LikedTopics;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string ImageURL { get; private set; }
        public string AboutMe { get; private set; }
        public IEnumerable<Topic> LikedTopics { get; private set; }
    }
}
