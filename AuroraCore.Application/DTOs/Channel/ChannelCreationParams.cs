﻿using System;

namespace AuroraCore.Application.DTOs
{
    public class ChannelCreationParams
    {
        public string Name { get; set; }
        public string ImageBase64 { get; set; }
        public string About { get; set; }
    }
}