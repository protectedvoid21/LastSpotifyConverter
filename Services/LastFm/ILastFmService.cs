namespace Services.LastFm;

public interface ILastFmService {
    Task<ApiResponse> GetTrackNames(string username, TrackPeriod trackPeriod);

    Task<IEnumerable<string>> JsonToTrackList(string jsonResponse);
}
