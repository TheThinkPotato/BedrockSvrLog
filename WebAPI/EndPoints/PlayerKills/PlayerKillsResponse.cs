namespace WebAPI;

public class PlayerKillsResponse
{
    public List<PlayerKills> PlayerKills { get; init; } = [];
}

public class PlayerKills
{
    public string Xuid { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int TotalKills { get; init; } = 0;
    public string FavouriteKillEntity => PlayerKillsList.OrderByDescending(pk => pk.KillCount).FirstOrDefault()?.EntityType ?? "None";

    public List<PlayerKillList> PlayerKillsList { get; init; } = [];

}

public record PlayerKillList
{
    public string EntityType { get; init; } = string.Empty;
    public int KillCount { get; init; } = 0;
}
