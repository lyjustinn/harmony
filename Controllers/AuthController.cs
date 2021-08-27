using System.Threading.Tasks;
using Harmony.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Harmony.Controllers 
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _cfig;
        private readonly IAuthService _authService;
        private readonly string _scope = "user-read-private playlist-modify-private playlist-read-private";
        private readonly string _redirectUrl = "https://localhost:5001/api/auth/callback";

        public AuthController(IConfiguration cfig, IAuthService authService)
        {
            _cfig = cfig;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            string clientId = _cfig["SpotifyClientId"];

            return Redirect($"https://accounts.spotify.com/authorize?client_id={clientId}&response_type=code&redirect_uri={_redirectUrl}&scope={_scope}");
        }

        [HttpGet]
        [Route("signout")]
        public ActionResult Signout() {
            Response.Cookies.Delete("harmony_authToken", new CookieOptions { 
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
    
            return Ok();
        }

        [HttpGet("callback")]
        public async Task<ActionResult<string>> Callback (string code, string state)
        {
            string clientId = _cfig["SpotifyClientId"];
            string clientSecret = _cfig["SpotifyClientSecret"];
            var signingKey = _cfig["SigningKey"];
            string token = await _authService.GetToken(code, _redirectUrl, clientId, clientSecret, signingKey);

            Response.Cookies.Append(
                "harmony_authToken",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                }
            );

            return Redirect("http://localhost:4200");
        }
    }
}