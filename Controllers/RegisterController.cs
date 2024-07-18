using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly UserService _userService;

        public RegisterController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            if (_userService.UsernameExists(user.Username))
            {
                return BadRequest("Bu kullanıcı adı zaten alınmış.");
            }

            if (_userService.EmailExists(user.Email))
            {
                return BadRequest("Bu email zaten kayıtlı.");
            }

            if (!_userService.IsRoleValid(user.Role))
            {
                return BadRequest("Geçersiz rol.");
            }

            _userService.Add(user);
            return Ok("Kayıt başarılı.");
        }
    }
}
