using System.Diagnostics.Eventing.Reader;

namespace WebAPI;
public class UsersOnlineResponse
{
    public List<UserOnlineDTO> Users { get; set; } = [];
}

public record UserOnlineDTO
{
    public string Name { get; init; } = string.Empty;
    public string Xuid { get; init; } = string.Empty;
    public string? Pfid { get; init; } = null;
    public DateTime LastLogin { get; init; }
    public DateTime SpawnTime { get; init; }
    
    public TimeSpan TimeSinceLastLogin => DateTime.Now - LastLogin;
    public TimeSpan TimeSinceSpawn =>  SpawnTime == DateTime.MinValue ? TimeSpan.Zero : DateTime.Now - SpawnTime;   

    public string DiceBearAvatarUrl { get; init; } = string.Empty;
}
