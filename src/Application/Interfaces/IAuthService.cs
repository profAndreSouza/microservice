using Microsoft.AspNetCore.Mvc;
using UserAuth.API.DTOs;

namespace UserAuth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<int?> LoginUser(UserLoginDTO userLoginDTO);
    }
}
