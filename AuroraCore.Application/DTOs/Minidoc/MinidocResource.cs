using AuroraCore.Application.DTOs.Channel;
using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.DTOs.Minidoc
{
    public class MinidocResource
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ChannelCompact Channel { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<MinidocCategory> Categories { get; set; }
    }
}