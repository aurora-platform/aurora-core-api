using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.IO;

namespace AuroraCore.Infrastructure.Services
{
    public class CloudinaryImageStorageService : IImageStorageService
    {
        private const string CloudName = "aurora-platform";
        private const string ApiKey = "757229786243174";
        private const string ApiSecret = "uPMg8AHKOkJ_6nANxreYYCKydkY";

        private readonly Cloudinary _cloudinary;

        public CloudinaryImageStorageService()
        {
            _cloudinary = new Cloudinary(new Account(CloudName, ApiKey, ApiSecret));
        }

        public void Delete(ImageReference image)
        {
            string[] publicIds = { image.ExternalId };
             _cloudinary.DeleteResources(ResourceType.Image, publicIds);
        }

        public ImageReference Store(string filename, string base64)
        {
            var bytes = Convert.FromBase64String(base64);
            var contents = new MemoryStream(bytes);

            bool isJPG = base64.Substring(0, 1) == "/";
            bool isPNG = base64.Substring(0, 1) == "i";

            if (!isJPG && !isPNG)
                throw new ValidationException("The file extension provided is not allowed. Only jpg and png are supported.");

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filename, contents),
            };

            ImageUploadResult result = _cloudinary.Upload(uploadParams);

            return new ImageReference
            {
                ExternalId = result.PublicId,
                Filename = result.OriginalFilename,
                Format = result.Format,
                Height = result.Height,
                Width = result.Width,
                Bytes = result.Bytes,
                Uri = result.Url.AbsoluteUri,
            }; ;
        }
    }
}
