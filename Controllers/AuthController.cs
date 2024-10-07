using Microsoft.AspNetCore.Mvc;
using UserAuthAPI.Data;
using UserAuthAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace UserAuthAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            // Check if the user already exists
            if (_context.Users.Any(u => u.Username == userDto.Username))
            {
                return BadRequest("Username already exists");
            }

            // Create a password hash
            using var hmac = new HMACSHA512();
            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password))),
                Email = userDto.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == userDto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            // Validate password
            using var hmac = new HMACSHA512();
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
            if (user.PasswordHash != Convert.ToBase64String(computedHash))
            {
                return Unauthorized("Invalid password");
            }

            // Return a JWT token (you'll need to implement JWT creation here)
            return Ok("Login successful");
        }
    }

}