using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using System;

namespace AuroraCore.UnitTests.Infrastructure.Services
{
    public class FakeImageStorageService : IImageStorageService
    {
        public void Delete(ImageReference image)
        {
        }

        public ImageReference Store(string filename, string base64)
        {
            return new ImageReference
            {
                Bytes = 1024,
                ExternalId = "aaaaaaa",
                Filename = filename,
                Format = "png",
                Height = 100,
                Width = 100,
                Id = Guid.NewGuid(),
                Uri = $"www.google.com.br/{Guid.NewGuid()}"
            };
        }
    }
}