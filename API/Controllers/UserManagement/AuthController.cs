using System.Security.Claims;
using Application.Contracts.UserManagement;
using Application.Dtos.UserManagement;
using Domain.Models.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
        }
        public static User user = new();
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await _authRepository.RegisterAsync(request);
            if (user is null)
            {
                return BadRequest("Username already exsits.");
            }
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login(UserDto request)
        {
            var token = await _authRepository.LoginAsync(request);
            if (token is null)
            {
                return BadRequest("Invalid email or password.");
            }
            return Ok(token);

        }
        [HttpPost("verify_token")]
        [Authorize]
        public async Task<ActionResult<User>> VerifyToken()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
                return Unauthorized("Username not found in token.");

            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }
    }
}