using System.Threading.Tasks;
using Harmony.Models.Playlist;
using Harmony.Models.User;

namespace Harmony.Services.Youtube 
{
    public interface IYoutubeService
    {
        Task<string> CreateYoutubePlaylist (string spotifyPlaylistId);
    }
}