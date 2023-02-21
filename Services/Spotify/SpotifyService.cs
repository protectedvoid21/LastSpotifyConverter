using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpotifyAPI.Web;

namespace Services.Spotify;

public class SpotifyService : ISpotifyService {
    private ILogger<SpotifyService> logger;
    private SpotifyClient spotify;
    private readonly string redirectLink;
    private readonly string clientKey;
    private readonly string clientSecret;

    public SpotifyService(ILogger<SpotifyService> logger, IConfiguration configuration) {
        this.logger = logger;
        redirectLink = configuration["SiteUrl"];
        clientKey = configuration["spotify-client"];
        clientSecret = configuration["spotify-secret"];
    }

    public Uri GetAuthorizeLink() {
        var loginRequest = new LoginRequest(new Uri(redirectLink + "/LastFmStep"), clientKey, LoginRequest.ResponseType.Code) {
            Scope = new[] { Scopes.PlaylistModifyPublic, Scopes.PlaylistModifyPrivate, Scopes.UserReadEmail, Scopes.UserReadPrivate }
        };

        return loginRequest.ToUri();
    }

    public async Task InitializeByCallback(string code) {
        var response = await new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(clientKey, clientSecret, code, new Uri(redirectLink + "/LastFmStep"))
        );

        var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new AuthorizationCodeAuthenticator(clientKey, clientSecret, response));

        spotify = new SpotifyClient(config);
    }

    public async Task<string> GetCurrentUserId() {
        try {
            var user = await spotify.UserProfile.Current();
            return user.Id;
        }
        catch(APIException ex) {
            logger.LogError(ex.Message);
        }
        return "Error";
    }

    public async Task<bool> CreatePlaylist(string userId, IEnumerable<string> trackNames) {
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

        if(trackUriList.Count == 0) {
            return false;
        }

        PlaylistCreateRequest playlistRequest = new("My lastfm") {
            Public = true,
            Description = "The most popular tracks fetched from Last.Fm",
        };
        var playList = await spotify.Playlists.Create(userId, playlistRequest);

        PlaylistAddItemsRequest playlistAddRequest = new(trackUriList);
        await spotify.Playlists.AddItems(playList.Id, playlistAddRequest);
        return true;
    }
}
