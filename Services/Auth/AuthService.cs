using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Harmony.Models;

namespace Harmony.Services.Auth
{
    public class AuthService : IAuthService
    {
        public HttpClient Client { get; }

        public AuthService(HttpClient client)
        {
            Client = client;
        }
        public async Task<string> GetToken (string code, string callback, string clientId, string clientSecret)
        {
            Dictionary<string, string> postReq = new Dictionary<string, string>();

            postReq.Add("grant_type", "authorization_code");
            postReq.Add("redirect_uri", callback);
            postReq.Add("code", code);
            postReq.Add("client_id", clientId);
            postReq.Add("client_secret", clientSecret);

            var content = new FormUrlEncodedContent(postReq);

            try
            {
                var response = await Client.PostAsync("https://accounts.spotify.com/api/token", content);

                if (!response.IsSuccessStatusCode) return "";

                var body = await response.Content.ReadFromJsonAsync<SpotifyAuthResponse>();
                return body.access_token;
            } catch (Exception e) 
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}