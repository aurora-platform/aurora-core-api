using System;

namespace AuroraCore.Domain.Model
{
    public class ImageReference
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; }
        public long Bytes { get; set; }
        public string Uri { get; set; }
        public string ExternalId { get; set; }

        public ImageReference()
        {
            Id = Guid.NewGuid();
        }

        public void ChangeReference(ImageReference newReference)
        {
            Filename = newReference.Filename;
            Width = newReference.Width;
            Height = newReference.Height;
            Format = newReference.Format;
            Bytes = newReference.Bytes;
            Uri = newReference.Uri;
            ExternalId = newReference.ExternalId;
        }

        public Uri GetUri()
        {
            return new Uri(Uri);
        }
    }
}
