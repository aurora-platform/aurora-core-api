using AuroraCore.Application.Dependencies;
using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using AuroraCore.Application.Providers;
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
        private readonly IObjectMapper _mapper;
        private readonly PasswordProvider _passwordProvider;

        public UserService(IUserRepository userRepository, IObjectMapper mapper, IHashProvider hashProvider)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordProvider = new PasswordProvider(hashProvider);
        }

        private static void ValidateUser(User user)
        {
            if (user == null)
            {
                throw new ValidationException("User not exists");
            }

            if (!user.IsValid())
            {
                throw new ValidationException("Invalid user");
            }
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

        public UserProfile GetProfile(Guid id)
        {
            User user = _userRepository.FindByID(id);
            ValidateUser(user);

            return new UserProfile(user);
        }

        public void EditProfile(UserProfile userProfile)
        {
            User user = _userRepository.FindByID(userProfile.Id);
            ValidateUser(user);

            userProfile.Email = user.Email;

            User mappedUser = _mapper.Map(userProfile, user);

            _userRepository.Update(mappedUser);
        }

        public void EditLikedTopics(Guid userId, IEnumerable<Topic> likedTopics)
        {
            User user = _userRepository.FindByID(userId);
            ValidateUser(user);

            if (!user.HasLikedTopics())
            {
                throw new ValidationException("Must be selected at least 1 topic");
            }

            _userRepository.UpdateLikedTopics(user.Id, likedTopics);
        }

        public void ChangePassword(Guid userId, string currentPassword, string newPassword, string confirmNewPassword)
        {
            User user = _userRepository.FindByID(userId);
            ValidateUser(user);

            _passwordProvider.Verify(currentPassword, user.Password);

            if (currentPassword == newPassword)
            {
                throw new ValidationException("The new password and current are the same");
            }

            if (newPassword != confirmNewPassword)
            {
                throw new ValidationException("The new password and confirmation are not the same");
            }

            string protectedNewPassword = _passwordProvider.Protect(newPassword);

            user.SetPassword(protectedNewPassword);

            _userRepository.Update(user);
        }
    }
}
