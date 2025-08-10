namespace WebAPI;
public class UsersResponse
{
    public List<UserDTO> Users { get; set; } = [];
}

public record UserDTO
{
    public string Name { get; init; } = string.Empty;
    public string Xuid { get; init; } = string.Empty;
    public string? Pfid { get; init; } = null;
    public string DiceBearAvatarUrl { get; init; } = string.Empty;
}
