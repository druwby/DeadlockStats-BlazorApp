using System.Text.Json.Serialization;

namespace BlazorApp1.Models;

// This is no longer the root object, but a wrapper we'll use in the UI
public class PlayerHeroSummary
{
    public string PlayerName { get; set; } = "Unknown Player";
    public List<HeroStat> HeroStats { get; set; } = new();
}

public class HeroStat
{
    [JsonPropertyName("hero_id")]
    public int HeroId { get; set; }

    // Note: The API usually returns the internal hero name (e.g. "hero_abrams")
    [JsonPropertyName("hero_name")]
    public string HeroName { get; set; } = "";

    [JsonPropertyName("matches_played")]
    public int MatchesPlayed { get; set; } = 0;

    [JsonPropertyName("wins")]
    public int Wins { get; set; } =  0;

    [JsonPropertyName("last_played")]
    public long LastPlayed { get; set; } = 0;
    public DateTime LastPlayedDate => DateTimeOffset.FromUnixTimeSeconds(LastPlayed).DateTime;

    [JsonPropertyName("time_played")]
    public double TimePlayed { get; set; } = 0;
    public TimeSpan TimePlayedSpan => TimeSpan.FromSeconds(TimePlayed);

    [JsonPropertyName("win_rate")]
    public double WinRate => MatchesPlayed > 0 ? (double)Wins / MatchesPlayed * 100 : 0;
}

public class HeroAsset
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
}