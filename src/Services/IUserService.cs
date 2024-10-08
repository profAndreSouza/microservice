using UserAuthAPI.DTOs;

namespace UserAuthAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(int id);
        Task AddUser(UserDTO userDTO);
        Task UpdateUser(int id, UserDTO userDTO);
        Task DeleteUser(int id);
    }
}
