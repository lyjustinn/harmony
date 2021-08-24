using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Harmony.Models.Song;
using Harmony.Models.Video;

namespace Harmony.Services.Youtube
{
    public class YoutubeService : IYoutubeService
    {
        private readonly HttpClient _client;

        public YoutubeService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> CreateYoutubePlaylist (Harmony.Models.Playlist.PlaylistItem spotifyPlaylist, string apiKey, GoogleCredential credential)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
            });

            var playlist = await CreatePlaylist(youtubeService, spotifyPlaylist.Name, spotifyPlaylist.Description);

            foreach (var song in spotifyPlaylist.Tracks.Items)
            {
                await AddSongToPlaylist(playlist, youtubeService, song, apiKey);
            }

            return playlist.Id;
        }

        private async Task<Playlist> CreatePlaylist (Google.Apis.YouTube.v3.YouTubeService youtubeService, string name, string description)
        {
            var playlist = new Playlist();
            playlist.Snippet = new PlaylistSnippet();
            playlist.Snippet.Title = name;
            playlist.Snippet.Description = description;
            playlist.Status = new PlaylistStatus();
            playlist.Status.PrivacyStatus = "private";

            playlist = await youtubeService.Playlists.Insert(playlist, "snippet,status").ExecuteAsync();

            return playlist;
        }

        private async Task AddSongToPlaylist (Playlist playlist, Google.Apis.YouTube.v3.YouTubeService youTubeService, SongItem song, string apiKey)
        {
            var uri = new UriBuilder("https://youtube.googleapis.com/youtube/v3/search?");
            uri.Port = -1;

            var query = HttpUtility.ParseQueryString(uri.Query);
            query["part"] = "snippet";
            query["maxResults"] = "5";
            
            var searchString = $"{song.Track.Name}";
            foreach(var artist in song.Track.Artists)
            {
                searchString += $" {artist.Name}";
            }

            query["q"] = searchString;
            query["type"] = "video";
            query["key"] = apiKey;

            uri.Query = query.ToString();
            string endpoint = uri.ToString();

            var response = await _client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode) return;
            var body = await response.Content.ReadFromJsonAsync<VideoSearch>();

            if (body.Items.Count == 0) return;
            var videoId = body.Items[0].Id.VideoId;

            var playlistItem = new Google.Apis.YouTube.v3.Data.PlaylistItem();
            playlistItem.Snippet = new PlaylistItemSnippet();
            playlistItem.Snippet.PlaylistId = playlist.Id;
            playlistItem.Snippet.ResourceId = new ResourceId();
            playlistItem.Snippet.ResourceId.Kind = "youtube#video";
            playlistItem.Snippet.ResourceId.VideoId = videoId;
            playlistItem = await youTubeService.PlaylistItems.Insert(playlistItem, "snippet").ExecuteAsync();

            return;
        }
    }
}