using Amazon;
using Amazon.ElasticTranscoder;
using Amazon.ElasticTranscoder.Model;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AuroraCore.Infrastructure.Services
{
    internal class Presets
    {
        public static readonly string HLSVideo400k = "1351620000001-200055";
        public static readonly string HLSVideo600k = "1351620000001-200045";
        public static readonly string HLSVideo1M = "1351620000001-200035";
    }

    public class AwsElasticTranscoder : ITranscodingService
    {
        private readonly AmazonElasticTranscoderClient _client;
        private readonly AwsS3 _s3Client;

        public AwsElasticTranscoder()
        {
            _client = new AmazonElasticTranscoderClient(RegionEndpoint.USWest2);
            _s3Client = new AwsS3();
        }

        public async Task CreateJob(string id, string format, Stream data)
        {
            var inputKey = $"{id}.{format}";

            await _s3Client.Store(inputKey, data);

            var outputKeys = new List<string> { $"{id}400k", $"{id}600k", $"{id}1M" };

            var command = new CreateJobRequest
            {
                PipelineId = "1627523670929-0ul0iz",
                Input = new JobInput
                {
                    Key = inputKey
                },
                Outputs = new List<CreateJobOutput> {
                    new CreateJobOutput {
                        PresetId = Presets.HLSVideo400k,
                        Key = outputKeys[0],
                        SegmentDuration = "10",
                    },
                    new CreateJobOutput {
                        PresetId = Presets.HLSVideo600k,
                        Key = outputKeys[1],
                        SegmentDuration = "10",
                    },
                    new CreateJobOutput {
                        PresetId = Presets.HLSVideo1M,
                        Key = outputKeys[2],
                        SegmentDuration = "10",
                    }
                },
                Playlists = new List<CreateJobPlaylist> {
                    new CreateJobPlaylist {
                        Name = id,
                        Format = "HLSv4",
                        OutputKeys = outputKeys
                    }
                }
            };

            await _client.CreateJobAsync(command);
        }
    }
}