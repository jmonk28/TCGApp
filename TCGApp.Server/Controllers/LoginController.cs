using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCGApp.Server.Data;
using TCGApp.Server.Service;
using TCGApp.Server.Models;
using TCGApp.Server.DTO;
using TCGApp.Server.Utilities;

namespace TCGApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private UserService _userService;

        public LoginController(ILogger<LoginController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("username/{username}")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }

        [HttpPost("login")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> Login([FromBody] LoginRequest credentials)
        {
            SHA256Hash hasher = new SHA256Hash();
            string pass_hash = hasher.ComputeSHA256Hash(credentials.Password);

            var loginResult = await _userService.PerformLogin(credentials.Email, pass_hash);
            if (!loginResult) return Unauthorized();

            //Issue cookie to frontend
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            //Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            //return Ok(new { message = "Logged in" });
        }
    }
}
