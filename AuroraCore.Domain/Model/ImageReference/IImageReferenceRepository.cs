using AuroraCore.Domain.Shared;

namespace AuroraCore.Domain.Model
{
    public interface IImageReferenceRepository
    {
        void Store(ImageReference reference);
        void Update(ImageReference reference);
    }
}
