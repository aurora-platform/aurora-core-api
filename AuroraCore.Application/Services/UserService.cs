using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuroraCore.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void SetupInitialSettings(Guid id, string name, IEnumerable<Topic> likedTopics)
        {
            User user = _userRepository.FindByID(id);

            bool hasLikedTopics = likedTopics != null && likedTopics.Any();

            if (user == null)
            {
                throw new ValidationException("The user not exists");
            }

            if (!hasLikedTopics)
            {
                throw new ValidationException("Must be selected at least 1 topic");
            }

            if (!user.IsActivated)
            {
                throw new ValidationException("User is not activated");
            }

            if (user.IsConfigured)
            {
                throw new ValidationException("User is already configured");
            }

            user.SetName(name);
            user.SetLikedTopics(likedTopics);
            user.SetAsConfigured(); 

            _userRepository.Update(user);
        }

        public User FindUser(Guid id)
        {
            User user = _userRepository.FindByID(id);

            if (user == null)
            {
                throw new ValidationException("User not exists");
            }

            return user;
        }

        public void EditLikedTopics(Guid userId, IEnumerable<Topic> likedTopics)
        {
            User user = _userRepository.FindByID(userId);

            bool hasLikedTopics = likedTopics != null && likedTopics.Any();

            if (user == null)
            {
                throw new ValidationException("The user not exists");
            }

            if (!user.IsValid())
            {
                throw new ValidationException("Invalid user");
            }

            if (!hasLikedTopics)
            {
                throw new ValidationException("Must be selected at least 1 topic");
            }

            user.SetLikedTopics(likedTopics);

            _userRepository.Update(user);
        }
    }
}
