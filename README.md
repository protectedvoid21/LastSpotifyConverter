# LastSpotifyConverter

Link to website : https://lastspotifyconverter.azurewebsites.net

# Important

As Spotify api is shared by default in Development Mode the website won't get your request and will throw an error. As long as application won't get Quota Extension from
Spotify, the server will return an error because of missing authorization. To proceed a user, I must add his name and email to whitelist so he will be able to access Spotify api through my app. To find if the website is working, the best solution is to download the code, create your own Spotify application, get the client_id and secret_id, pass them to application and then the lastFm tracks will appear as a playlist on Spotify account.

# What is this website doing?

LastSpotifyConverter will get your spotify authorization to read data (to get your userId) and add new playlist to your account. After that you will be redirected to
pass LastFm username (this does not has to be yours :3) and select timespan of track listening. After 20-30 sec a newly created playlist will appear on your spotify.

# Used technologies and libaries
- <a href="https://dotnet.microsoft.com/en-us/apps/aspnet">ASP.NET with Razor Pages</a>
- <a href="https://johnnycrazy.github.io/SpotifyAPI-NET/">SpotifyAPI-Net</a>
- <a href="https://azure.microsoft.com/en-us/">Microsoft Azure</a> (hosting)
- <a href="https://azure.microsoft.com/en-us/products/key-vault/">Azure Key Vault</a>

# Incoming
- Inform user about playlist creation success
- Improve frontend
