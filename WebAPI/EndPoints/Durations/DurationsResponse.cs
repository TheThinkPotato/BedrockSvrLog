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
    public TimeSpan Duration { get; init; } = TimeSpan.Zero;
    public TimeSpan GameplayDuration { get; init; } = TimeSpan.Zero;
}
