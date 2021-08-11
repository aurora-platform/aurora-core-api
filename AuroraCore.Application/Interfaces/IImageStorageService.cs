using AuroraCore.Domain.Model;

namespace AuroraCore.Application.Interfaces
{
    public interface IImageStorageService
    {
        ImageReference Store(string filename, string base64);

        void Delete(ImageReference image);
    }
}