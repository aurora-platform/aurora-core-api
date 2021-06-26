using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;

namespace AuroraCore.Web.DTOs
{
    public class AccountInitSettings
    {
        public string Name { get; set; }

        public IEnumerable<Topic> LikedTopics { get; set; }
    }
}
