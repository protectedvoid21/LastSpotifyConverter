using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Services.LastFm;

public class LastFmService : ILastFmService {
    private readonly string apiKey;
    private readonly HttpClient httpClient;

    public LastFmService(IConfiguration configuration, HttpClient httpClient) {
        apiKey = configuration["lastfm-key"];
        this.httpClient = httpClient;
    }

    public async Task<ApiResponse> GetTrackNames(string userName, TrackPeriod trackPeriod) {
        string uri = $"?method=user.gettoptracks &period={PeriodMap.GetName(trackPeriod)}&api_key={apiKey}&user={userName}&format=json";
        HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));

        if(response.IsSuccessStatusCode) {
            return new ApiResponse(true, await response.Content.ReadAsStringAsync());
        }

        return new ApiResponse(false, "Error, user was not found");
    }

    public async Task<IEnumerable<string>> JsonToTrackList(string jsonResponse) {
        RootObject root = JsonSerializer.Deserialize<RootObject>(jsonResponse);

        return root.toptracks.track.Select(t => t.name);
    }
}