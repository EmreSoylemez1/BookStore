using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var existingUser = _userService.Authenticate(loginRequest.Username, loginRequest.Password);
            if (existingUser == null)
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");
            }

            // Email verification is optional
            if (!string.IsNullOrEmpty(loginRequest.Email) && existingUser.Email != loginRequest.Email)
            {
                return Unauthorized("Email hatalı.");
            }

            return Ok("Giriş başarılı.");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
