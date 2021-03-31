using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;

namespace aurora_core_api.DTOs
{
    public class AccountInitSettings
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public IEnumerable<Topic> LikedTopics { get; set; }
    }
}
