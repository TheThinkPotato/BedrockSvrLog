using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BedrockSvrLog.Models;

public class Login
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Xuid")]
    public required string Xuid { get; set; } // Foreign key to User table
    public virtual User User { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime? SpawnTime { get; set; }
    public DateTime? LogoutTime { get; set; }
    public TimeSpan? Duration { get; set; }
    public TimeSpan? GameplayeDuration { get; set; }

    public Login()
    {
    }
}
