using AuroraCore.Application.DTOs;
using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.Interfaces
{
    public interface IUserService
    {
        void SetupInitialSettings(Guid id, string name, IEnumerable<Topic> likedTopics);

        void EditLikedTopics(Guid id, IEnumerable<Topic> likedTopics);

        UserProfile GetProfile(Guid id);

        void EditProfile(UserProfile userProfile);
    }
}
