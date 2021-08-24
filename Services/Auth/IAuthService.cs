using System.Threading.Tasks;
using Harmony.Models;

namespace Harmony.Services.Auth {
    public interface IAuthService 
    {
        Task<string> GetToken (string code, string callback, string secret, string clientSecret, string signingKey);

        string GetAccessToken (string refreshToken, string signingKey);
    }
}