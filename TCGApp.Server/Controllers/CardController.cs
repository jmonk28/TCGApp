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
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private UserService _userService;
        private CardService _cardService;

        public CardController(ILogger<CardController> logger, UserService userService, CardService cardService)
        {
            _logger = logger;
            _userService = userService;
            _cardService = cardService;
        }

        [AllowAnonymous]
        [HttpPost("addtobasecollection")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> AddNewCardToBaseCollection([FromBody] CollectionCardRequest newCollectionCard)
        {
            //Read refresh token
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) _logger.LogInformation("Refresh cookie not working");
            if (refreshToken == null) return Unauthorized("Could not validate user");

            //Get user
            var user = await _userService.GetUserByRefreshToken(refreshToken);
            if (user == null) return Unauthorized("Invalid refresh token");

            CollectionCard newCollectionCardBuilder = new CollectionCard();
            newCollectionCardBuilder.CollectionCardID = _cardService.GenerateCollectionCardID();
            newCollectionCardBuilder.CollectionID = newCollectionCard.CollectionID;
            newCollectionCardBuilder.CardID = newCollectionCard.CardID;
            newCollectionCardBuilder.TCGUserID = user.UserID;

            try
            {
                _cardService.AddCollectionCard(newCollectionCardBuilder);
            }
            catch (Exception ex) {
                _logger.LogError("New Collection Card could not be added");
            }


            return Ok();
        }

        //[AllowAnonymous]
        //[HttpPost("getcollectioncards")]
        //[EnableCors("AllowSpecificOrigins")]
        //public async Task<IActionResult> GetCollectionCards([FromBody] int collectionID)
        //{
        //    //Read refresh token
        //    var refreshToken = Request.Cookies["refreshToken"];
        //    if (string.IsNullOrEmpty(refreshToken)) _logger.LogInformation("Refresh cookie not working");
        //    if (refreshToken == null) return Unauthorized("Could not validate user");

        //    var collectionCards = await _cardService.GetCardsInCollection(collectionID);
        //    if (collectionCards == null) _logger.LogInformation("NO COLLECTION CARDS WERE RETURNED FROM THE CONTROLLER");

        //    return Ok(collectionCards);
        //}

        [AllowAnonymous]
        [HttpPost("getdatabasecards")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> GetDatabaseCards([FromBody] int collectionID)
        {
            //Read refresh token
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) _logger.LogInformation("Refresh cookie not working");
            if (refreshToken == null) return Unauthorized("Could not validate user");

            var databaseCards = await _cardService.GetFirstTenDatabaseCards();

            return Ok(databaseCards);
        }

        [AllowAnonymous]
        [HttpPost("getdatabasecardsfromcollectioncards")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> GetDatabaseCardsFromCollectionCards([FromBody] int collectionID)
        {
            //Read refresh token
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) _logger.LogInformation("Refresh cookie not working");
            if (refreshToken == null) return Unauthorized("Could not validate user");

            //Get collection cards
            var collectionCards = await _cardService.GetCardsInCollection(collectionID);
            if (collectionCards == null) _logger.LogInformation("NO COLLECTION CARDS WERE RETURNED FROM THE CONTROLLER");

            //Get corresponding database cards
            var databaseCards = await _cardService.GetDatabaseCardsFromCollectionCards(collectionCards);
            if (databaseCards == null) _logger.LogInformation("NO DATABASE CARDS WERE RETURNED FROM THE CONTROLLER");

            return Ok(databaseCards);
        }

    }
}
