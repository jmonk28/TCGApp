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

            CollectionCard newCollectionCardBuilder = new CollectionCard();
            newCollectionCardBuilder.CollectionCardID = _cardService.GenerateCollectionCardID();
            newCollectionCardBuilder.CollectionID = newCollectionCard.CollectionID;
            newCollectionCardBuilder.CardID = newCollectionCard.CardID;

            try
            {
                _cardService.AddCollectionCard(newCollectionCardBuilder);
            }
            catch (Exception ex) {
                _logger.LogError("New Collection Card could not be added");
            }


            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("getcollectioncards")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> GetCollectionCards([FromBody] int collectionID)
        {
            //Read refresh token
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) _logger.LogInformation("Refresh cookie not working");
            if (refreshToken == null) return Unauthorized("Could not validate user");

            var collectionCards = await _cardService.GetCardsInCollection(collectionID);

            return Ok(collectionCards);
        }


    }
}
