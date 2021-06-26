using System;

namespace AuroraCore.Domain.Model
{
    public class VideoReference
    {
        public VideoReference()
        {
           Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string ExternalId { get; set; }
        public int Duration { get; set;}
        public string Uri { get; set; }
        public string MimeType { get; set; }
        public string Filename { get; set; }
        public long SizeInBytes { get; set; }
        public string Thumbnail { get; set; }
    }
}
