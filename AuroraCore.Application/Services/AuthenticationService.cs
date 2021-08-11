using AuroraCore.Application.DTOs;
using AuroraCore.Application.Interfaces;
using AuroraCore.Domain.Model;
using AuroraCore.Domain.Shared;

namespace AuroraCore.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordService _passwordService;
        private readonly IObjectMapper _mapper;

        public AuthenticationService(IUserRepository userRepository, IHashProvider hashProvider, IObjectMapper mapper)
        {
            _userRepository = userRepository;
            _passwordService = new PasswordService(hashProvider);
            _mapper = mapper;
        }

        public UserResource AuthenticateWithPassword(string username, string password)
        {
            User user = _userRepository.FindByUsername(username);

            if (user == null) throw new ValidationException("User not exists");

            _passwordService.Verify(password, user.Password);

            return _mapper.Map<UserResource>(user);
        }
    }
}