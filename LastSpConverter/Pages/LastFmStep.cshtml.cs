﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.LastFm;
using Services.Spotify;

namespace LastSpConverter.Pages; 

public class LastFmDataModel : PageModel {
    private readonly ILastFmService lastFmService;
    private readonly ISpotifyService spotifyService;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel {
        [Required]
        public string UserName { get; set; }
        [Required]
        public TrackPeriod TrackPeriod { get; set; } = TrackPeriod.Overall;
    }

    public LastFmDataModel(ILastFmService lastFmService, ISpotifyService spotifyService) {
        this.lastFmService = lastFmService;
        this.spotifyService = spotifyService;
    }

    public async Task OnGet(string code) {
        await spotifyService.InitializeByCallback(code);
    }

    public async Task<IActionResult> OnPost() {
        if(!ModelState.IsValid) {
            return Page();
        }

        var apiResponse = await lastFmService.GetTrackNames(Input.UserName, Input.TrackPeriod);

        if(apiResponse.IsSuccess == false) {
            ModelState.AddModelError("Input.UserName", "User with this name does not exist");
            return Page();
        }

        IEnumerable<string> trackList = await lastFmService.JsonToTrackList(apiResponse.Content);

        bool playlistSucceeded = await spotifyService.CreatePlaylist(await spotifyService.GetCurrentUserId(), trackList);
        if(playlistSucceeded == false) {
            ModelState.AddModelError("Input.UserName", "Something went wrong with creating playlist. " +
                "User with this nickname may not have scrobbled anything in selected timespan");
        }
        return Page();
    }
}