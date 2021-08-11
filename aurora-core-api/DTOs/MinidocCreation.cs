using Microsoft.AspNetCore.Http;
using System;

namespace AuroraCore.Web.DTOs
{
    public class MinidocCreation
    {
        public Guid ChannelId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public Guid[] Topics { get; set; }
        public Guid[] Categories { get; set; }
    }
}