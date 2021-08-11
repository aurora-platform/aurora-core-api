using AuroraCore.Application.DTOs;
using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;

namespace AuroraCore.Application.Interfaces
{
    public interface IUserService
    {
        UserResource Create(string username, string email, string password);

        void Edit(Guid userId, UserEditionParams editionParams);

        void SetupInitialSettings(Guid userId, string name, IEnumerable<Topic> likedTopics);

        void EditLikedTopics(Guid userId, IEnumerable<Topic> likedTopics);

        UserResource Get(Guid userId);

        void ChangePassword(Guid userId, string currentPassword, string newPassword, string confirmNewPassword);
    }
}