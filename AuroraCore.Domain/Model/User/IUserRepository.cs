using System;
using System.Collections.Generic;

namespace AuroraCore.Domain.Model
{
    public interface IUserRepository
    {
        void Store(User user);
        void Update(User user);
        User FindById(Guid id);
        User FindByUsername(string username);
        User FindByUsernameOrEmail(string username, string email);
        void UpdateLikedTopics(Guid userId, IEnumerable<Topic> likedTopics);
    }
}
