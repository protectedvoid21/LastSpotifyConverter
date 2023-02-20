using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Spotify;

public interface ISpotifyService {
    Uri GetAuthorizeLink();

    Task InitializeByCallback(string code);

    Task<string> GetCurrentUserId();

    Task<bool> CreatePlaylist(string userId, IEnumerable<string> trackNames);
}
