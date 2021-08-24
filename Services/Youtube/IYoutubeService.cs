using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Harmony.Models.Playlist;

namespace Harmony.Services.Youtube 
{
    public interface IYoutubeService
    {
        Task<string> CreateYoutubePlaylist (PlaylistItem spotifyPlaylist, string apiKey, GoogleCredential credential);
    }
}