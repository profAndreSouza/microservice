using Microsoft.AspNetCore.Mvc;
using UserAuth.API.DTOs;
using UserAuth.Application.Helpers;
using UserAuth.Application.Interfaces;

namespace UserAuth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(UserLoginDTO userLoginDTO)
        {
            var result = await _authService.LoginUser(userLoginDTO);
            if (result == null)
                return Unauthorized("Invalid Credentials");

            var token = TokenHelper.tokenize(userLoginDTO.Username);
            return Ok(token);
        }
    }
}
