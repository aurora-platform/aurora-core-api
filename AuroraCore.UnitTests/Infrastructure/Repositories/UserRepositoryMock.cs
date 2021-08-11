using AuroraCore.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuroraCore.UnitTests.Infrastructure.Repositories
{
    public class UserRepositoryMock : IUserRepository
    {
        public readonly List<User> users;

        public UserRepositoryMock()
        {
            users = new List<User>();
        }

        public int Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public User FindById(Guid id)
        {
            return users.FirstOrDefault(user => user.Id == id);
        }

        public User FindByUsername(string username)
        {
            return users.FirstOrDefault(user => user.Username == username);
        }

        public User FindByUsernameOrEmail(string username, string email)
        {
            return users.FirstOrDefault(user => (user.Username == username) || (user.Email == email));
        }

        public IEnumerable<User> GetAll()
        {
            return users;
        }

        public void Store(User entity)
        {
            users.Add(entity);
        }

        public void Update(User entity)
        {
            User foundUser = users.FirstOrDefault(user => user.Id == entity.Id);
            if (foundUser != null) foundUser = entity;
        }

        public void UpdateLikedTopics(Guid userId, IEnumerable<Topic> likedTopics)
        {
            User foundedUser = users.FirstOrDefault(user => user.Id == userId);
            if (foundedUser != null)
            {
                foundedUser.SetLikedTopics(likedTopics);
            }
        }
    }
}