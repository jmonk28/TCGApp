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
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private UserService _userService;
        private TokenService _tokenService;

        public LoginController(ILogger<LoginController> logger, UserService userService, TokenService tokenService)
        {
            _logger = logger;
            _userService = userService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("login")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> Login([FromBody] LoginRequest credentials)
        {
            SHA256Hash hasher = new SHA256Hash();
            string pass_hash = hasher.ComputeSHA256Hash(credentials.Password);

            _logger.LogInformation("Grabbing login user...");
            var loginUser = await _userService.PerformLogin(credentials.Email, pass_hash);
            if (loginUser == null) return Unauthorized();

            //Configure jwt and refresh tokens from token service
            _logger.LogInformation("Generating access and refresh tokens...");
            string jwtToken = _tokenService.GenerateJwtToken(loginUser);
            string refreshToken = _tokenService.GenerateRefreshToken();
            _logger.LogInformation("Tokens:\n" + jwtToken + "\n" +  refreshToken);

            try
            {
                _logger.LogInformation("Saving refresh token to user in database...");
                await _userService.SaveRefreshTokenAsync(loginUser.UserID, refreshToken);
            } catch (Exception ex)
            {
                _logger.LogError("Error while saving user refresh token");
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Domain = "localhost",
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _logger.LogInformation("Appending refresh token as browser cookie...");
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

            //Return access token to frontend
            return Ok(new
            {
                accessToken = jwtToken,
                username = loginUser.Username
            });

        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> TokenRefresh()
        {
            //Read refresh token
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) _logger.LogInformation("Refresh cookie not working in refresh");
            if (refreshToken == null) return Unauthorized("No active refresh token");

            //Make sure user with refresh token actually exists
            var user = await _userService.GetUserByRefreshToken(refreshToken);
            if (user == null) return Unauthorized("Invalid refresh token");

            //Validate refresh token expiry
            if (user.RefreshTokenExpiry < DateTime.UtcNow) return Unauthorized("Refresh token expired");

            //Build new refresh token
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            await _userService.SaveRefreshTokenAsync(user.UserID, newRefreshToken);

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly= true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Domain = "localhost",
                Expires= DateTime.UtcNow.AddDays(7)
            });

            //Generate the new access token
            var newToken = _tokenService.GenerateJwtToken(user);

            return Ok(new
            {
                accessToken = newToken,
                username = user.Username
            });

        }

        [AllowAnonymous]
        [HttpPost("logout")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> Logout()
        {
            //Clear the user's refresh token in the database
            var refreshToken = Request.Cookies["refreshToken"];
            _logger.LogInformation("Grabbed refresh token cookie");
            var user = await _userService.GetUserByRefreshToken(refreshToken);
            _logger.LogInformation("Grabbed user");
            var success = await _userService.UserLogout(user);
            if (!success) return BadRequest("User logout failed");
            _logger.LogInformation("Logged out user");

            //Clear the refresh token in the browser
            Response.Cookies.Delete("refreshToken");
            return Ok();
        }
    }
}
