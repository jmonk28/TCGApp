using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCGApp.Server.Service;
using TCGApp.Server.Models;
using TCGApp.Server.Utilities;
using TCGApp.Server.DTO;

namespace TCGApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private UserService _userService;

        public RegisterController(ILogger<RegisterController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

  
        [HttpPost("newuser")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterRequest newUser)
        {
            _logger.LogInformation("Attempting to register new user...");
            Console.WriteLine(newUser.Username);
            Console.WriteLine(newUser.Email);
            Console.WriteLine(newUser.PasswordHash);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Hash Password
            SHA256Hash hasher = new SHA256Hash();
            string pass_hash = hasher.ComputeSHA256Hash(newUser.PasswordHash);

            var savedUser = new TCGUser();
            savedUser.UserID = _userService.GenerateUserID();
            savedUser.Username = newUser.Username;
            savedUser.Email = newUser.Email;
            savedUser.PasswordHash = pass_hash;
            savedUser.LastLogin = null;


            try
            {
                _userService.CreateUser(savedUser);
            }
            catch (Exception ex) {
                _logger.LogError("New user could not be registered");
                throw ex;
            }

            return Ok(savedUser);

        }
       
    }
}
