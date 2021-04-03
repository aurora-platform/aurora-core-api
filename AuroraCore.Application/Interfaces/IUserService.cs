using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.Interfaces
{
    public interface IUserService
    {
        void SetupInitialSettings(Guid id, string name, IEnumerable<Topic> likedTopics);

        User FindUser(Guid id);
    }
}
