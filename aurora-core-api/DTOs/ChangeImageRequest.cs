using System;

namespace AuroraCore.Web.DTOs
{
    public class ChangeImageRequest
    {
        public Guid Id { get; set; }
        public string ImageBase64 { get; set; }
    }
}