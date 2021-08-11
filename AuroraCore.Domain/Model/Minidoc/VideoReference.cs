using System;
using System.ComponentModel.DataAnnotations;

namespace AuroraCore.Domain.Model
{
    public class VideoReference
    {
        public Guid Id { get; private set; }
        public int Duration { get; set; }
        public string Uri { get; private set; }
        public VideoProcessingStatus ProcessingStatus { get; set; }

        public VideoReference(int duration)
        {
            Id = Guid.NewGuid();

            if (duration <= 0) throw new ValidationException("A video must have a duration");

            Duration = duration;
            ProcessingStatus = VideoProcessingStatus.Processing;
        }
    }
}