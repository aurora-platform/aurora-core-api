using System.IO;
using System.Threading.Tasks;

namespace AuroraCore.Application.Interfaces
{
    public interface ITranscodingService
    {
        Task CreateJob(string id, string format, Stream Data);
    }
}