using System;

namespace AuroraCore.Domain.Model
{
    public interface IVideoReferenceRepository
    {
        void Store(VideoReference reference);
        void Update(VideoReference reference);
        void Delete(Guid id);
        Minidoc FindById(Guid id);
    }
}
