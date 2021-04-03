using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByUsername(string username);

        User FindByUsernameOrEmail(string username, string email);

        void UpdateLikedTopics(Guid userId, IEnumerable<Topic> likedTopics);
    }
}
