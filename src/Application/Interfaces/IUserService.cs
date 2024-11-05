using UserAuth.API.DTOs;

namespace UserAuth.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(int id);
        Task<UserDTO> GetUserByEmail(string email);
        Task AddUser(UserDTO userDTO);
        Task UpdateUser(int id, UserDTO userDTO);
        Task DeleteUser(int id);
    }
}
