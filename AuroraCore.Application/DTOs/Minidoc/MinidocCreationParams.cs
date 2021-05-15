using System;
using System.Collections.Generic;

namespace AuroraCore.Application.DTOs
{
    public class MinidocCreationParams {
      public Guid ChannelId { get; }
      public string Title { get; }
      public string Description { get; }
      public IEnumerable<Guid> Topics { get; }
      public IEnumerable<Guid> Categories { get; }
    }
}