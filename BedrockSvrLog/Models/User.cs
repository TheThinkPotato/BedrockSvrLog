using System.ComponentModel.DataAnnotations;

namespace BedrockSvrLog.Data;
public class User
{
    public string Name { get; set; } = string.Empty;
    [Key]
    public string Xuid { get; set; }
    public string? Pfid { get; set; }
    public ICollection<Login> Logins { get; set; }
    public ICollection<RealmEvent> RealmEvents { get; set; }

    public User()
	{
	}
}
