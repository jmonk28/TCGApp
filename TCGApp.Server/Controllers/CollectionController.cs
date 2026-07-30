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
    public class CollectionController : ControllerBase
    {
        private readonly ILogger<CollectionController> _logger;
        private UserService _userService;
        private CollectionService _collectionService;

        public CollectionController(ILogger<CollectionController> logger, UserService userService, CollectionService collectionService)
        {
            _logger = logger;
            _userService = userService;
            _collectionService = collectionService;
        }

        [AllowAnonymous]
        [HttpPost("addcollection")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> CreateNewCollection([FromBody] CollectionRequest newCollection)
        {
            //Read refresh token
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) _logger.LogInformation("Refresh cookie not working in refresh");
            if (refreshToken == null) return Unauthorized("No active refresh token");

            //Get user
            var user = await _userService.GetUserByRefreshToken(refreshToken);
            if (user == null) return Unauthorized("Invalid refresh token");

            //Build new collection
            Collection collectionBuilder = new Collection();
            collectionBuilder.CollectionName = newCollection.CollectionName;
            collectionBuilder.CollectionType = newCollection.CollectionType;
            //New collection starts with no cards
            collectionBuilder.CardCount = 0;
            //MOST IMPORTANT THING!!! Collection MUST be foreign keyed to its owning user
            collectionBuilder.TCGUserId = user.UserID;

            try
            {
                _collectionService.AddCollection(collectionBuilder);
            } catch (Exception ex) {
                _logger.LogError("Collection could not be submitted");
                throw ex;
              }

            return Ok();
        }

    }
}
