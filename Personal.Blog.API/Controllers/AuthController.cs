using Microsoft.AspNetCore.Mvc;
using Personal.Blog.Application.Services;
using Personal.Blog.Domain.Dtos;

namespace Personal.Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("token")]
        public async Task<IActionResult> GenerateToken([FromQuery] UserDto user)
        {
            var token = _authService.GenerateKey(user.UserSecret);
            return Ok(token);
        }
    }
}
