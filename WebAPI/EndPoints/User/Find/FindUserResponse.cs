namespace WebAPI;
public class FindUserResponse
{
    public string Name { get; init; } = string.Empty;
    public string Xuid { get; init; } = string.Empty;
    public string? Pfid { get; init; } = null;
}