using AuroraCore.Application.DTOs;
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
        private readonly IObjectMapper _mapper;
        private readonly PasswordService _passwordService;

        public UserService(IUserRepository userRepository, IObjectMapper mapper, IHashProvider hashProvider)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordService = new PasswordService(hashProvider);
        }

        private void CheckIfUserExists(string username, string email)
        {
            User existingUser = _userRepository.FindByUsernameOrEmail(username, email);

            if (existingUser != null)
            {
                if (existingUser.Email == email)
                    throw new ValidationException("A user already exists with this email");

                if (existingUser.Username == username)
                    throw new ValidationException("This username already taken");
            }
        }

        public UserResource Create(string username, string email, string password)
        {
            CheckIfUserExists(username, email);

            var user = new User(username, email, "User");

            if (string.IsNullOrWhiteSpace(password))
                throw new ValidationException("Password is required");

            var protectedPassword = _passwordService.Protect(password);

            user.SetPassword(protectedPassword);
            user.SetAsActive();

            _userRepository.Store(user);

            return _mapper.Map<UserResource>(user);
        }

        public void SetupInitialSettings(Guid userId, string name, IEnumerable<Topic> likedTopics)
        {
            User user = _userRepository.FindByID(userId);

            bool hasLikedTopics = likedTopics != null && likedTopics.Any();

            if (user == null) throw new ValidationException("The user not exists");

            if (!hasLikedTopics) throw new ValidationException("Must be selected at least 1 topic");

            if (!user.IsActivated) throw new ValidationException("User is not activated");

            if (user.IsConfigured) throw new ValidationException("User is already configured");

            user.SetName(name);
            user.SetLikedTopics(likedTopics);
            user.SetAsConfigured(); 

            _userRepository.Update(user);
        }

        public UserResource Get(Guid userId)
        {
            User user = _userRepository.FindByID(userId);

            if (user == null) throw new ValidationException("The user not exists");

            return _mapper.Map<UserResource>(user);
        }

        public void Edit(Guid userId, UserEditionParams editionParams)
        {
            User findedUser = _userRepository.FindByID(userId);

            if (findedUser == null) throw new ValidationException("The user not exists");

            findedUser.SetName(editionParams.Name);
            findedUser.SetUsername(editionParams.Username);
            findedUser.SetPhone(editionParams.Phone);
            findedUser.SetAboutMe(editionParams.AboutMe);
            findedUser.Validate();

            _userRepository.Update(findedUser);
        }

        public void EditLikedTopics(Guid userId, IEnumerable<Topic> likedTopics)
        {
            User user = _userRepository.FindByID(userId);

            if (user == null) throw new ValidationException("The user not exists");

            user.Validate();

            if (likedTopics == null || !likedTopics.Any())
                throw new ValidationException("Must be selected at least 1 topic");

            _userRepository.UpdateLikedTopics(user.Id, likedTopics);
        }

        public void ChangePassword(Guid userId, string currentPassword, string newPassword, string confirmNewPassword)
        {
            User user = _userRepository.FindByID(userId);

            if (user == null) throw new ValidationException("The user not exists");

            user.Validate();

            _passwordService.Verify(currentPassword, user.Password);

            if (currentPassword == newPassword)
                throw new ValidationException("The new password and current are the same");

            if (newPassword != confirmNewPassword)
                throw new ValidationException("The new password and confirmation are not the same");

            string protectedNewPassword = _passwordService.Protect(newPassword);

            user.SetPassword(protectedNewPassword);

            _userRepository.Update(user);
        }
    }
}
