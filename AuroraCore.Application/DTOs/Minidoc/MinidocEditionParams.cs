using System;

namespace AuroraCore.Application.DTOs.Minidoc
{
    public class MinidocEditionParams
    {
        public Guid MinidocId { get; }
        public Guid ChannelId { get; }
        public string Title { get; }
        public string Description { get; }
        public Guid[] Topics { get; }
        public Guid[] Categories { get; }
    }
}