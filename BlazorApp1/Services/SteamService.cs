using System.Text.Json.Serialization;
using BlazorApp1.Models;
public class SteamService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    public SteamService(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _apiKey = configuration["SteamApi:ApiKey"] ?? "";
    }
    public async Task<SteamPlayer?> GetPlayerSummary(string steamId)
{
    // The endpoint for player details
    var url = $"ISteamUser/GetPlayerSummaries/v0002/?key={_apiKey}&steamids={steamId}";
    
    var result = await _http.GetFromJsonAsync<SteamSummaryResponse>(url);

    // Get the first player in the list (since we only requested one ID)
    var apiData = result?.Response.Players.FirstOrDefault();

    if (apiData == null) return null;

    // Map the "API Data" to your clean "SteamPlayer" model
    return new SteamPlayer
    {
        PersonaName = apiData.PersonaName,
        AvatarUrl = apiData.AvatarUrl,
        vanityUrl = apiData.ProfileUrl
    };
}
    public async Task<SteamPlayer?> GetPlayerByVanityUrl(string vanityName)
    {
    // Step 1: Convert the Name to an ID
    var vanityUrl = $"ISteamUser/ResolveVanityURL/v0001/?key={_apiKey}&vanityurl={vanityName}";
    var resolve = await _http.GetFromJsonAsync<SteamVanityResponse>(vanityUrl);

    if (resolve?.Response.Success != 1 || string.IsNullOrEmpty(resolve.Response.SteamId))
    {
        return null; // Handle "User Not Found"
    }

    // Step 2: Use the ID to get the profile (reuse your existing logic)
    return await GetPlayerSummary(resolve.Response.SteamId);
    }
}


public class SteamVanityResponse
    {
        [JsonPropertyName("response")]
        public VanityResult Response { get; set; } = new();
    }

public class VanityResult
    {
        [JsonPropertyName("success")]
        public int Success { get; set; }

        [JsonPropertyName("steamid")]
        public string SteamId { get; set; } = "";
    }
// The top-level wrapper
public class SteamSummaryResponse
{
    [JsonPropertyName("response")]
    public PlayerListWrapper Response { get; set; } = new();
}

// The "players" list wrapper
public class PlayerListWrapper
{
    [JsonPropertyName("players")]
    public List<SteamPlayerApiData> Players { get; set; } = new();
}

// Exactly what the Steam API sends back
public class SteamPlayerApiData
{
    [JsonPropertyName("personaname")]
    public string PersonaName { get; set; } = "";

    [JsonPropertyName("avatarfull")]
    public string AvatarUrl { get; set; } = "";

    [JsonPropertyName("profileurl")]
    public string ProfileUrl { get; set; } = "";
}