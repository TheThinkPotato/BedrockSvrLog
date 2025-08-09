namespace WebAPI;
public class RealmsEventResponse
{
    public List<RealmsEventDTO> Users { get; set; } = [];
}

public record RealmsEventDTO
{
    public string Name { get; init; } = string.Empty;
    public string Xuid { get; init; } = string.Empty;
    public string? Pfid { get; init; } = null;
    public string RealmEvent { get; init; } = string.Empty;
    public DateTime? EventTime { get; init; } = null;
}
