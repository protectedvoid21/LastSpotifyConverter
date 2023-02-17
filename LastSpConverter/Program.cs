using Services.LastFm;
using Services.Spotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddTransient<ILastFmService, LastFmService>();
builder.Services.AddSingleton<ISpotifyService, SpotifyService>();

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
