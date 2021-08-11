using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public class Minidoc
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Channel Channel { get; private set; }
        public VideoReference Video { get; private set; }
        public IList<Topic> Topics { get; private set; }
        public IList<MinidocCategory> Categories { get; private set; }

        public Minidoc()
        {
        }

        public Minidoc(string title, string description, int duration, Channel channel, IList<Topic> topics, IList<MinidocCategory> categories)
        {
            Validate.NotNullOrWhiteSpace(title, "The title is required");
            Validate.NotNull(channel, "The channel is required");
            
            if (topics.Count > 3)
                throw new ValidationException("The number of topics must be less than or equal to 3");

            if (categories.Count > 3)
                throw new ValidationException("The number of categories must be less than or equal to 3");

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Topics = topics;
            Categories = categories;
            Channel = channel;
            Video = new VideoReference(duration);
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetTopics(IList<Topic> topics)
        {
            Topics = topics;
        }

        public void SetCategories(IList<MinidocCategory> categories)
        {
            Categories = categories;
        }

        public void SetChannel(Channel channel)
        {
            Channel = channel;
        }

        public void SetVideo(VideoReference reference)
        {
            Video = reference;
        }

        public void PrepareToStream()
        {
            Video.ProcessingStatus = VideoProcessingStatus.Ready;
        }

        public void MarkAsFailedToStream()
        {
            Video.ProcessingStatus = VideoProcessingStatus.Failed;
        }
    }
}