using System;
using System.IO;

namespace AuroraCore.Application.DTOs.Minidoc
{
    public class MinidocEditionParams
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public MemoryStream Video { get; set; }
        public Guid[] Topics { get; set; }
        public Guid[] Categories { get; set; }
    }
}