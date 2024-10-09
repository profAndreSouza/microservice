using Microsoft.AspNetCore.Mvc;
using UserAuth.API.DTOs;
using UserAuth.Application.Helpers;
using UserAuth.Application.Interfaces;
using UserAuth.Domain.Interfaces;

namespace UserAuth.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int?> LoginUser(UserLoginDTO userLoginDTO)
        {
            var user = await _userRepository.FindUserByUsername(userLoginDTO.Username);
            if (user != null)
                if (PasswordHelper.VerifyPassword(userLoginDTO.Password, user.Password))
                    return user.Id;
                    
            return null;
        }
    }
}
