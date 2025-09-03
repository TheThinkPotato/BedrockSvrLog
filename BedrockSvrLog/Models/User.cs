using System.ComponentModel.DataAnnotations;

namespace BedrockSvrLog.Model;
public class User
{
    public string Name { get; set; } = string.Empty;
    [Key]
    public string Xuid { get; set; }
    public string? Pfid { get; set; }
    public string? AvatarLink { get; set; }
    public ICollection<Login> Logins { get; set; }
    public ICollection<RealmEvent> RealmEvents { get; set; }

    public int? LocationX { get; set; }
    public int? LocationY { get; set; }
    public int? LocationZ { get; set; }

    public string? LocationDimension { get; set; }

    public int? SpawnX { get; set; }
    public int? SpawnY { get; set; }
    public int? SpawnZ { get; set; }
    public User()
	{
	}
}
