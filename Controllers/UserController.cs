using System.Threading.Tasks;
using Harmony.Models.User;
using Harmony.Services.Spotify;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;

        public UserController(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        [HttpGet]
        public async Task<ActionResult<CurrentUser>> GetCurrentUser() 
        {
            var res = await _spotifyService.GetUser(Request.Cookies["harmony_authToken"]);
        
            if (res == null) return StatusCode(404);

            return Ok(res);}
    }
}