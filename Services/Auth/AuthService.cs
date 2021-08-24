using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Harmony.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;

namespace Harmony.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;

        public AuthService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://accounts.spotify.com");
            _client = client;
        }
        public async Task<string> GetToken (string code, string callback, string clientId, string clientSecret, string signingKey)
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
                var response = await _client.PostAsync("/api/token", content);

                if (!response.IsSuccessStatusCode) return "";

                var body = await response.Content.ReadFromJsonAsync<SpotifyAuthResponse>();
                return GenerateJwt(body.Access_token, signingKey);
            } catch (Exception e) 
            {
                Console.WriteLine(e);
                return "";
            }
        }

        private string GenerateJwt (string rsa, string signingKey) 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Rsa, rsa),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            };

            var jwt = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials( new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)), SecurityAlgorithms.HmacSha256)),
                    new JwtPayload(claims)
            ); 
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private bool ValidateJwt (string jwt,string signingKey)
        {
            var handler = new JwtSecurityTokenHandler();

            var param = new TokenValidationParameters{
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            SecurityToken res;

            var token = handler.ValidateToken(jwt, param, out res);

            return true;
        }

        public string GetAccessToken (string jwt, string signingKey)
        {
            try 
            {
                ValidateJwt(jwt, signingKey);

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(jwt);

                foreach (var claim in jwtToken.Claims)
                {
                    Console.WriteLine(claim.Type);
                }

                var rsaClaim = jwtToken.Claims.First(x => x.Type ==  ClaimTypes.Rsa);

                return rsaClaim.Value;
            } catch (Exception e)
            {
                Console.WriteLine(e);
                return "noo";
            }
        }
    }
}