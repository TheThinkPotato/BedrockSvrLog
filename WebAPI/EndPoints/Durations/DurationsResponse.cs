namespace WebAPI;
public class DurationsResponse
{
    public List<DurationDTO> Durations { get; set; } = [];
}

public record DurationDTO
{
    public string Name { get; init; } = string.Empty;
    public string Xuid { get; init; } = string.Empty;
    public string? Pfid { get; init; } = null;
    public string DiceBearAvatarUrl { get; init; } = string.Empty;
    public TimeSpan TotalDuration { get; init; } = TimeSpan.Zero;
    public TimeSpan TotalGameplayDuration { get; init; } = TimeSpan.Zero;
    public TimeSpan TotalLiveDuration { get; init; } = TimeSpan.Zero;
    public TimeSpan TotalLiveGameplayDuration { get; init; } = TimeSpan.Zero;
    public DateTime LastLogin { get; init; } = DateTime.MinValue;
    public DateTime SpawnTime { get; init; } = DateTime.MinValue;
    public TimeSpan TimeSinceLastLogin => DateTime.Now - LastLogin;
    public TimeSpan TimeSinceLastSpawn =>  SpawnTime == DateTime.MinValue ? TimeSpan.Zero : DateTime.Now - SpawnTime;
    public bool IsOnline { get; init; }
}
