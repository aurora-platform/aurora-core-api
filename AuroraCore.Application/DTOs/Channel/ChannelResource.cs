using System;

namespace AuroraCore.Application.DTOs.Channel
{
    public class ChannelResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public UserCompact Owner { get; set; }
        public ImageCompact Image { get; set; }
    }
}
