using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCGApp.Server.Service;
using TCGApp.Server.Models;
using TCGApp.Server.Utilities;
using TCGApp.Server.DTO;
using Microsoft.AspNetCore.Authorization;

namespace TCGApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private UserService _userService;
        private CollectionService _collectionService;

        public RegisterController(ILogger<RegisterController> logger, UserService userService, CollectionService collectionService)
        {
            _logger = logger;
            _userService = userService;
            _collectionService = collectionService;
        }

        [AllowAnonymous]
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

            //Add base collection for user
            var baseCollection = new Collection();
            baseCollection.CollectionID = _collectionService.GenerateCollectionID();
            baseCollection.CollectionName = "Base Collection";
            baseCollection.CollectionType = "All";
            baseCollection.CardCount = 0;
            baseCollection.TCGUserID = savedUser.UserID;
            //Here we are creating the user and their base collection, so we will set IsBase to 1
            baseCollection.IsBase = 1;


            try
            {
                _userService.CreateUser(savedUser);
                _collectionService.AddCollection(baseCollection);
            }
            catch (Exception ex) {
                _logger.LogError("New user could not be registered");
                throw ex;
            }

            return Ok(savedUser);

        }
       
    }
}
