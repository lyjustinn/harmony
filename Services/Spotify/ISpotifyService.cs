using System.Collections.Generic;
using System.Threading.Tasks;
using Harmony.Models.Playlist;
using Harmony.Models.User;

namespace Harmony.Services.Spotify 
{
    public interface ISpotifyService
    {
        Task<UserPlaylists> GetUserPlaylists (string token);
        
        Task<CurrentUser> GetUser (string token);

        Task<PlaylistItem> GetPlaylist (string token, string playlistId);
    }
}