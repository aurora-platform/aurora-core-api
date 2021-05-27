using System;

namespace AuroraCore.Application.DTOs.Minidoc
{
    public class MinidocCreationParams
    {
        public Guid ChannelId { get; }
        public string Title { get; }
        public string Description { get; }
        public Guid[] Topics { get; }
        public Guid[] Categories { get; }
    }
}