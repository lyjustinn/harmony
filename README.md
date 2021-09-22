# harmony
A Web API designed to interface with Spotify's Developer API and Google's Youtube API. Handles authentication using OAuth2.0 and HTTP cookies

### Dependencies
* Google.Apis
* Google.Apis.Auth.AspNetCore3
* Google.Apis.YouTube.v3
* Microsoft.AspNetCore.Authentication.JwtBearer
* System.IdentityModel.Tokens.Jwt
* Microsoft.AspNetCore.Cors
* Swashbuckle.AspNetCore

### Installation

Running the server locally, will require to create and register your OWN api keys.

#### Local Installation
1. Download the repositry
2. Install the dependencies above with [Nuget](https://www.nuget.org/ "nuget")
3. Create an app with Spotify, set your callback uri to the following: http://localhost:5000/api/auth/callback
4. Create an app with Youtube, follow [this](https://developers.google.com/youtube/v3/getting-started "Get your Youtube API credentials") guide to get your keys and credentials
5. In your terminal, navigate to the top level directory of the repository
6. Run the following commands in your terminal:
   * `dotnet user-secrets init`
   * `dotnet user-secrets set "GoogleClientId" "YOUR GOOGLE CLIENT ID"`
   * `dotnet user-secrets set "GoogleClientSecret" "YOUR GOOGLE CLIENT SECRET"`
   * `dotnet user-secrets set "GoogleApiKey" "YOUR GOOGLE API KEY"`
   * `dotnet user-secrets set "SpotifyClientId" "YOUR SPOTIFY CLIENT ID"`
   * `dotnet user-secrets set "SpotifyClientSecret" "YOUR SPOTIFY CLIENT SECRET"`
7. Once everything is setup you can run `dotnet run` in your terminal
   
