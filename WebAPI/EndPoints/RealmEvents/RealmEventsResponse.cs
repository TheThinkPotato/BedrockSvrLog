namespace WebAPI;
public class RealmsEventResponse
{
    public List<RealmEventDetails> RealmEvents { get; set; } = [];
}

public record RealmEventDetails
{
    public string Name { get; init; } = string.Empty;
    public string Xuid { get; init; } = string.Empty;
    public string? Pfid { get; init; } = null;
    public string RealmEvent { get; init; } = string.Empty;
    public DateTime? EventTime { get; init; } = null;
    public string DiceBearAvatarUrl { get; init; } = string.Empty;
}
