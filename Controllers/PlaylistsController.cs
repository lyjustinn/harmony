using System.Threading.Tasks;
using Harmony.Models;
using Harmony.Models.Playlist;
using Harmony.Services.Spotify;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.YouTube.v3;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Harmony.Services.Auth;
using Harmony.Services.Youtube;

namespace Harmony.Controllers 
{
    [Route("api/playlists")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;

        private readonly IAuthService _authService;

        private readonly IConfiguration _cfig;

        private readonly IYoutubeService _youtubeService;

        public PlaylistsController(ISpotifyService spotifyService, IConfiguration cfig, IAuthService authService, IYoutubeService youtubeService)
        {
            _spotifyService = spotifyService;
            _cfig = cfig;
            _authService = authService;
            _youtubeService = youtubeService;
        }

        [HttpGet]
        public async Task<ActionResult<Image>> GetPlaylists()
        {
            var signingKey = _cfig["SigningKey"];
            var accessToken = _authService.GetAccessToken(Request.Cookies["harmony_authToken"], signingKey);
            var res = await _spotifyService.GetUserPlaylists(accessToken);
        
            if (res == null) return StatusCode(404);

            return Ok(res);
        }

        [HttpGet]
        [Route("{playlistId}")]
        public async Task<ActionResult<PlaylistItem>> GetPlaylistById([FromRoute] string playlistId)
        {
            var signingKey = _cfig["SigningKey"];
            var accessToken = _authService.GetAccessToken(Request.Cookies["harmony_authToken"], signingKey);
            var res = await _spotifyService.GetPlaylist(accessToken, playlistId);
        
            if (res == null) return StatusCode(404);

            return Ok(res);
        }
        [HttpGet]
        [GoogleScopedAuthorize(YouTubeService.ScopeConstants.Youtube)]
        [Route("convert/youtube/{playlistId}")]
        public async Task<ActionResult> ConvetSpotifyToYoutube([FromRoute] string playlistId, [FromServices] IGoogleAuthProvider authProvider)
        {
            var signingKey = _cfig["SigningKey"];
            var accessToken = _authService.GetAccessToken(Request.Cookies["harmony_authToken"], signingKey);
            var userPlaylists = await _spotifyService.GetUserPlaylists(accessToken);
            
            if (userPlaylists == null) return StatusCode(404);
        
            var isUserOwner = userPlaylists.Items.Where(x => x.Id == playlistId).Count() > 0;

            if (!isUserOwner) return StatusCode(404);

            var specifiedPlaylist = await _spotifyService.GetPlaylist(accessToken, playlistId);
        
            if (specifiedPlaylist == null) return StatusCode(404);

            var apiKey = _cfig["GoogleApiKey"];
            var cred = await authProvider.GetCredentialAsync();
            var youtubePlaylistId = await _youtubeService.CreateYoutubePlaylist(specifiedPlaylist, apiKey, cred);
            
            return Redirect($"https://www.youtube.com/playlist?list={youtubePlaylistId}");
        }
    }
}