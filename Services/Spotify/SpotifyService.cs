using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web;

namespace Services.Spotify;

public class SpotifyService : ISpotifyService {
    private SpotifyClient spotify;
    private readonly string clientKey;
    private readonly string clientSecret;

    public SpotifyService(IConfiguration configuration) {
        clientKey = configuration["spotify-client"];
        clientSecret = configuration["spotify-secret"];
    }

    public Uri GetAuthorizeLink() {
        var loginRequest = new LoginRequest(new Uri("https://localhost:7162/LastFmStep"), clientKey, LoginRequest.ResponseType.Code) {
            Scope = new[] { Scopes.PlaylistModifyPublic, Scopes.PlaylistModifyPrivate, Scopes.UserReadEmail, Scopes.UserReadPrivate }
        };

        return loginRequest.ToUri();
    }

    public async Task InitializeByCallback(string code) {
        var response = await new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(clientKey, clientSecret, code, new Uri("https://localhost:7162/LastFmStep"))
        );

        Console.WriteLine($"Response expiration : {response.IsExpired}");

        var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new AuthorizationCodeAuthenticator(clientKey, clientSecret, response));

        spotify = new SpotifyClient(config);
    }

    public async Task<string> GetCurrentUserId() {
        PrivateUser user = await spotify.UserProfile.Current();
        return user.Id;
    }

    public async Task CreatePlaylist(string userId, IEnumerable<string> trackNames) {
        List<string> trackUriList = new();

        foreach(var track in trackNames) {
            SearchResponse response = await spotify.Search.Item(new SearchRequest(SearchRequest.Types.Track, track));
            if(response.Tracks.Items == null) {
                continue;
            }
            if(response.Tracks.Items.Count != 0) {
                trackUriList.Add(response.Tracks.Items[0].Uri);
            }
        } 

        PlaylistCreateRequest playlistRequest = new("My lastfm") {
            Public = true,
            Description = "The most popular tracks fetched from Last.Fm",
        };
        var playList = await spotify.Playlists.Create(userId, playlistRequest);

        PlaylistAddItemsRequest playlistAddRequest = new(trackUriList);
        await spotify.Playlists.AddItems(playList.Id, playlistAddRequest);
    }
}
