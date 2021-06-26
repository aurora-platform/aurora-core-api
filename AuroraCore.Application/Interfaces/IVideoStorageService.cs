using System.IO;
using AuroraCore.Domain.Model;

namespace AuroraCore.Application.Interfaces
{
    public interface IVideoStorageService
    {
        VideoReference Store(string filename, MemoryStream video);
        VideoReference Override(string externalId, MemoryStream video);
        void Delete(VideoReference reference);
    }
}
