using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using System.IO;
using System.Threading.Tasks;

namespace AuroraCore.Infrastructure.Services
{
    public class AwsS3
    {
        private readonly AmazonS3Client _client;
        private const string InputBucketName = "aurora-input-videos";
        private const string OutputBucketName = "aurora-output-videos";

        public AwsS3()
        {
            _client = new AmazonS3Client(RegionEndpoint.USWest2);
        }

        public async Task Store(string filename, Stream file)
        {
            await _client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = InputBucketName,
                Key = filename,
                InputStream = file
            });
        }

        private string GetURL(string masterPlaylistName)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = OutputBucketName,
                Key = masterPlaylistName,
            };

            return _client.GetPreSignedURL(request);
        }

        public async Task Delete(VideoReference reference)
        {
            throw new System.NotImplementedException();
        }
    }
}