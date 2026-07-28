using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    public class UserInformationController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private UserService _userService;

        public UserInformationController(ILogger<LoginController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("userinfo")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            //Read refresh token
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) _logger.LogInformation("Refresh cookie not working in refresh");
            if (refreshToken == null) return Unauthorized("No active refresh token");

            //Get user
            var user = await _userService.GetUserByRefreshToken(refreshToken);
            if (user == null) return Unauthorized("Invalid refresh token");

            return Ok(
                new { 
                    username = user.Username,
                    email = user.Email
                }
            );

        }
    }
}

