using System.IO;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;

namespace AuroraCore.Infrastructure.Services
{
    public class GoogleCloudStorage : IVideoStorageService
    {
        public void Delete(VideoReference reference)
        {
            
        }

        public VideoReference Override(string externalId, MemoryStream video)
        {
            return new VideoReference {
                Duration = 10,
                ExternalId = "12345678",
                Filename = "filename",
                MimeType = "video/mp4",
                Thumbnail = "https://i.ytimg.com/vi/qJg3ci4Eluw/hq720.jpg?sqp=-oaymwEcCOgCEMoBSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLChvcjw8GFe_OG_Z8pzYH1abpv7wQ",
                SizeInBytes = 1024,
                Uri = "test mpd",
            };
        }
        
        public VideoReference Store(string filename, MemoryStream video)
        {
            return new VideoReference {
                Duration = 10,
                ExternalId = "12345678",
                Filename = filename,
                MimeType = "video/mp4",
                Thumbnail = "https://i.ytimg.com/vi/qJg3ci4Eluw/hq720.jpg?sqp=-oaymwEcCOgCEMoBSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLChvcjw8GFe_OG_Z8pzYH1abpv7wQ",
                SizeInBytes = 1024,
                Uri = "test mpd",
            };
        }
    }
}