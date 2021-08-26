using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Harmony.Models.Playlist;
using Harmony.Models.User;
using System.Linq;

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
        public async Task<UserPlaylists> GetUserPlaylists (string token, int pageNum)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _client.GetAsync($"/v1/me/playlists?limit=10&offset={pageNum}");

            if (!response.IsSuccessStatusCode) return null;

            var user = await GetUser(token);

            var body = await response.Content.ReadFromJsonAsync<UserPlaylists>();
            var playlists = body.Items.Where(x => x.Owner.Id == user.Id).ToList();
            body.Items = playlists;

            

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

        public async Task<PlaylistItem> GetPlaylist (string token, string playlistId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _client.GetAsync($"/v1/playlists/{playlistId}");

            if (!response.IsSuccessStatusCode) return null;

            var body = await response.Content.ReadFromJsonAsync<PlaylistItem>();
            return body;
        }
    }
}