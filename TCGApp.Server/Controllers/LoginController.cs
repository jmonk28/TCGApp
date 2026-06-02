using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCGApp.Server.Database;

namespace TCGApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private DAO db;

        public LoginController(ILogger<LoginController> logger, DAO dao)
        {
            _logger = logger;
            db = dao;
        }

        [HttpGet("username")]
        public IActionResult GetUserByUsername([FromQuery(Name = "username")]string username)
        {
            var user = db.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }
    }
}
