using UserAuth.API.DTOs;
using UserAuth.Application.Helpers;
using UserAuth.Application.Interfaces;
using UserAuth.Domain.Entities;
using UserAuth.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuth.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users.Select(user => new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                Roles = user.Roles.Select(role => new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToList()
            });
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                Roles = user.Roles.Select(role => new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToList()
            };
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
                Roles = user.Roles.Select(role => new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToList()
            };
        }

        public async Task AddUser(UserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Username = userDTO.Username,
                Password = PasswordHelper.HashPassword(userDTO.Password),
                Roles = userDTO.Roles.Select(roleDTO => new Role
                {
                    Id = roleDTO.Id,
                    Name = roleDTO.Name
                }).ToList()
            };

            await _userRepository.AddUser(user);
        }

        public async Task UpdateUser(int id, UserDTO userDTO)
        {
            var user = await _userRepository.GetUserById(id);
            if (user != null)
            {
                user.Name = userDTO.Name;
                user.Email = userDTO.Email;
                user.Username = userDTO.Username;
                
                user.Roles = userDTO.Roles.Select(roleDTO => new Role
                {
                    Id = roleDTO.Id,
                    Name = roleDTO.Name
                }).ToList();

                await _userRepository.UpdateUser(user);
            }
        }

        public async Task DeleteUser(int id)
        {
            await _userRepository.DeleteUser(id);
        }
    }
}
