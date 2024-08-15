using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ecommapp.Models;
using ecommapp.Services;

namespace ecommapp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdUser = await _userRepository.CreateUserAsync(user);
                return Ok(createdUser);
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(new {message = ex.Message});
            }

            // var createdUser = await _userRepository.CreateUserAsync(user);
            // return Ok(createdUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null || user.Id != id)
            {
                return BadRequest("User cannot be null and must have a matching ID.");
            }

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResult>> Authenticate([FromBody] LoginRequest loginRequest)
        {
            var result = await _userRepository.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            if (!result.IsSuccess)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
