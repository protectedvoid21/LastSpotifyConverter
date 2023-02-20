using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Services.LastFm;
using Services.Spotify;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddTransient<ILastFmService, LastFmService>();
builder.Services.AddSingleton<ISpotifyService, SpotifyService>();

string keyVaultUrl = builder.Configuration["KeyVault:Url"];

var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

if(builder.Environment.IsProduction()) {
    builder.Configuration.AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions());
}

var spotifySecret = await secretClient.GetSecretAsync("SpotifySecret");
var spotifyClient = await secretClient.GetSecretAsync("SpotifyClient");
var lastFmKey = await secretClient.GetSecretAsync("LastFmKey");

builder.Configuration["spotify-secret"] = spotifySecret.Value.Value;
builder.Configuration["spotify-client"] = spotifyClient.Value.Value;
builder.Configuration["lastfm-key"] = lastFmKey.Value.Value;

builder.Services.AddHttpClient<ILastFmService, LastFmService>("LastFm", client => {
    client.BaseAddress = new Uri("http://ws.audioscrobbler.com/2.0/");
});

var app = builder.Build();

if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
