using System.Net.Http;
using System.Threading.Tasks;

namespace Harmony.Services.Youtube
{
    public class YoutubeService : IYoutubeService
    {
        private readonly HttpClient _client;

        public YoutubeService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> CreateYoutubePlaylist (string spotifyPlaylistId)
        {
            return "";
        }

        // private
    }
}