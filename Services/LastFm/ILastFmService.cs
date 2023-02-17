namespace Services.LastFm;

public interface ILastFmService {
    Task<ApiResponse> GetTrackNames(string username);

    Task<IEnumerable<string>> JsonToTrackList(string jsonResponse);
}
