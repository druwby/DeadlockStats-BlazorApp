using BlazorApp1.Models;
using System.Net.Http.Json;

namespace BlazorApp1.Services;

public class DeadlockService
{
    private readonly HttpClient _http;
    private Dictionary<int, string>? _heroNameMap;

    public DeadlockService(HttpClient http) { _http = http; }

    private async Task LoadHeroNames()
    {
        if (_heroNameMap != null) return;

        // Call the Asset API from your uploaded JSON file
        var heroes = await _http.GetFromJsonAsync<List<HeroAsset>>("https://assets.deadlock-api.com/v2/heroes");
        _heroNameMap = heroes?.ToDictionary(h => h.Id, h => h.Name) ?? new();
    }

    public async Task<PlayerHeroSummary?> GetPlayerWinRates(string steamId)
    {
        await LoadHeroNames(); // Ensure we have the names first
        
        var stats = await _http.GetFromJsonAsync<List<HeroStat>>($"https://api.deadlock-api.com/v1/players/{steamId}/hero-stats");
        
        if (stats != null && _heroNameMap != null)
        {
            foreach (var s in stats)
            {
                // Bridge the gap: If the ID exists in our map, use that name
                if (_heroNameMap.TryGetValue(s.HeroId, out var name))
                {
                    s.HeroName = name;
                }
            }
        }
        
        return new PlayerHeroSummary { HeroStats = stats ?? new() };
    }
}