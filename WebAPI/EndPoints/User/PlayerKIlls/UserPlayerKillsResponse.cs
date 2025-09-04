namespace WebAPI.EndPoints.User.PlayerKIlls;

public class UserPlayerKillsResponse
{
    public string Xuid { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public int TotalKills { get; init; } = 0;
    public string FavouriteKillEntity => PlayerKillsList.OrderByDescending(pk => pk.KillCount).FirstOrDefault()?.EntityType ?? "None";
    public int FavouriteKillCount => PlayerKillsList.OrderByDescending(pk => pk.KillCount).FirstOrDefault()?.KillCount ?? 0;

    public List<PlayerKillListUser> PlayerKillsList { get; init; } = [];

}

public record PlayerKillListUser
{
    public string EntityType { get; init; } = string.Empty;
    public int KillCount { get; init; } = 0;
}
