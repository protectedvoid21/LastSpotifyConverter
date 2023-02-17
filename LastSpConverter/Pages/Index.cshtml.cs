using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Spotify;

namespace LastSpConverter.Pages; 

public class IndexModel : PageModel {
    private readonly ISpotifyService spotifyService;

    public string AuthLink { get; set; }

    public IndexModel(ISpotifyService spotifyService) {
        this.spotifyService = spotifyService;
    }

    public void OnGet() {
        AuthLink = spotifyService.GetAuthorizeLink().AbsoluteUri;
    }
}
