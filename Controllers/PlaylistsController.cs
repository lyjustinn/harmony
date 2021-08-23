using Harmony.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.Controllers 
{
    [Route("api/playlists")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Playlist> GetPlaylists()
        {
            var p = new Playlist();
            p.Id = 1;
            return Ok(p);
        }
    }
}