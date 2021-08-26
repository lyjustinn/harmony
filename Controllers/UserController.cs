using System;
using System.Threading.Tasks;
using Harmony.Models.User;
using Harmony.Services.Auth;
using Harmony.Services.Spotify;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Harmony.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;

        private readonly IAuthService _authService;

        private readonly IConfiguration _cfig;

        public UserController(ISpotifyService spotifyService, IConfiguration cfig, IAuthService authService)
        {
            _spotifyService = spotifyService;
            _authService = authService;
            _cfig = cfig;
        }

        [HttpGet]
        public async Task<ActionResult<CurrentUser>> GetCurrentUser() 
        {
            var signingKey = _cfig["SigningKey"];
            var accessToken = _authService.GetAccessToken(Request.Cookies["harmony_authToken"], signingKey);
            var res = await _spotifyService.GetUser(accessToken);
        
            if (res == null) return StatusCode(404);

            return Ok(res);
        }

        [HttpGet]
        [Route("ping")]
        public async Task<ActionResult<bool>> Ping() 
        {
            var signingKey = _cfig["SigningKey"];
            Console.WriteLine(Request.Cookies["harmony_authToken"]);
            var accessToken = _authService.GetAccessToken(Request.Cookies["harmony_authToken"], signingKey);
            var res = await _spotifyService.GetUser(accessToken);
        
            if (res == null) return NotFound(false);

            return Ok(true);
        }
    }
}