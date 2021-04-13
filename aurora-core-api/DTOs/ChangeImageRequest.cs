using System;

namespace aurora_core_api.DTOs
{
    public class ChangeImageRequest
    {
        public Guid Id { get; set; }
        public string ImageBase64 { get; set; }
    }
}
