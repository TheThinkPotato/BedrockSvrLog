namespace WebAPI;
public class UserRealmResponse
{
    public List<RealmEventDetails> RealmEvents { get; init; } = [];
    public bool hasRealmEvents => RealmEvents.Count > 0;
}

//public class RealmEventDetails
//{
//    public string Name { get; init; } = string.Empty;
//    public string Xuid { get; init; } = string.Empty;
//    public string? Pfid { get; init; } = null;
//    public string DiceBearAvatarUrl { get; init; } = string.Empty;
//    public string RealmEvent { get; init; } = string.Empty;
//    public DateTime EventTime { get; init; } = DateTime.MinValue;
//}