using System;
using Microsoft.AspNetCore.Http;

namespace AuroraCore.Web.DTOs
{
    public class MinidocEdition
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public Guid[] Topics { get; set; }
        public Guid[] Categories { get; set; }
    }
}