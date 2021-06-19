using System.IO;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using Google.Cloud.Storage.V1;

namespace AuroraCore.Infrastructure.Services
{
    public class GoogleCloudStorage : IVideoStorageService
    {
        private readonly string _bucketName = "your-unique-bucket-name";

        public void Delete(VideoReference reference)
        {
            throw new System.NotImplementedException();
        }

        public VideoReference Override(string externalId, Stream video)
        {
            throw new System.NotImplementedException();
        }

        public VideoReference Store(string filename, Stream video)
        {
            StorageClient storage = StorageClient.Create();
            Google.Apis.Storage.v1.Data.Object gcsObject = storage.UploadObject(_bucketName, filename, null, video);

            return new VideoReference {
                Duration = gcsObject.,
                ExternalId = gcsObject.Id,
                Filename = filename,
                Format = "",
                Thumbnail = "",
                SizeInBytes = ,
                Uri = gcsObject.
            }
        }
    }
}