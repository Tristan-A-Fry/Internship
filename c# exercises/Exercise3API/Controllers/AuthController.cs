using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Exercise3api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var authResult = _userRepository.Authenticate(userLogin.Username, userLogin.Password);

            if (!authResult.IsSuccess)
            {
                return Unauthorized();
            }

            var token = JwtTokenGenerator.GenerateToken(userLogin.Username, authResult.Roles, _configuration);
            return Ok(new { Token = token });
        }
    }

    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

