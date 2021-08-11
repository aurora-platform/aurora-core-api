using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.DTOs
{
    public class UserResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ImageURL { get; set; }
        public string AboutMe { get; set; }
        public IEnumerable<Topic> LikedTopics { get; set; }
    }
}