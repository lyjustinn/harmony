using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Harmony.Models.Playlist;
using Harmony.Models.User;

namespace Harmony.Services.Spotify
{
    public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient _client;
        public SpotifyService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.spotify.com");
            _client = client;
        }
        public async Task<UserPlaylists> GetUserPlaylists (string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _client.GetAsync("/v1/me/playlists");

            if (!response.IsSuccessStatusCode) return null;

            var body = await response.Content.ReadFromJsonAsync<UserPlaylists>();
            return body;
        }
        
        public async Task<CurrentUser> GetUser (string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _client.GetAsync("/v1/me");

            if (!response.IsSuccessStatusCode) return null;

            var body = await response.Content.ReadFromJsonAsync<CurrentUser>();
            return body;
        }
    }
}