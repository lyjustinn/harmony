using System.Threading.Tasks;
using Harmony.Models;
using Harmony.Models.Playlist;
using Harmony.Services.Spotify;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.Controllers 
{
    [Route("api/playlists")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;

        public PlaylistsController(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        [HttpGet]
        public async Task<ActionResult<Image>> GetPlaylists()
        {
            var res = await _spotifyService.GetUserPlaylists(Request.Cookies["harmony_authToken"]);
        
            if (res == null) return StatusCode(404);

            return Ok(res);
        }

        [HttpGet]
        [Route("{playlistId}")]
        public async Task<ActionResult<PlaylistItem>> GetPlaylistById([FromRoute] string playlistId)
        {
            var res = await _spotifyService.GetPlaylist(Request.Cookies["harmony_authToken"], playlistId);
        
            if (res == null) return StatusCode(404);

            return Ok(res);
        }
    }
}