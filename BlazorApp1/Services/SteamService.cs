public class SteamService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    public SteamService(HttpClient http, IConfiguration configuration)
    {
        _http = http;
        _apiKey = configuration["SteamApi:ApiKey"] ?? "";
    }
}