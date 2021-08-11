using AuroraCore.Application.DTOs.Channel;
using System;

namespace AuroraCore.Application.DTOs.Minidoc
{
    public class MinidocCompact
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string URI { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public ChannelCompact Channel { get; set; }
    }
}