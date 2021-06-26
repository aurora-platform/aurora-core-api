using System;
using System.Collections.Generic;

namespace AuroraCore.Application.DTOs.Channel
{
    public class ChannelCreationParams
    {
        public string Name { get; set; }
        public string ImageBase64 { get; set; }
        public string About { get; set; }
    }
}
