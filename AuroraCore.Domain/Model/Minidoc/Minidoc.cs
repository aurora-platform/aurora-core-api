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
        public IEnumerable<Topic> Topics { get; private set; }
        public IEnumerable<MinidocCategory> Categories { get; private set; }

        public Minidoc(string title, string description, Channel channel, IEnumerable<Topic> topics, IEnumerable<MinidocCategory> categories)
        {
            Id = new Guid();
            Title = title;
            Description = description;
            Topics = topics;
            Categories = categories;
            Channel = channel;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetTopics(IEnumerable<Topic> topics)
        {
            Topics = topics;
        }

        public void SetCategories(IEnumerable<MinidocCategory> categories)
        {
            Categories = categories;
        }
    }
}
