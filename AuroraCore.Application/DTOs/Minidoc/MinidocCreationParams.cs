using System;
using System.IO;

namespace AuroraCore.Application.DTOs.Minidoc
{
    public class MinidocCreationParams
    {
        public Guid ChannelId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public MemoryStream Video { get; set; }
        public Guid[] Topics { get; set; }
        public Guid[] Categories { get; set; }
    }
}