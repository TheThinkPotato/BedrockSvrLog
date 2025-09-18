using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BedrockSvrLog.Models.Base;

namespace BedrockSvrLog.Models;

public class PlayerDeaths : PlayerDayTimeLocationSpawnEntity
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Xuid")]
    public string Xuid { get; set; } = string.Empty; // Foreign key to User table
    public virtual User? Player { get; set; }
    [ForeignKey("Xuid")]
    public string? KillerXuid { get; set; } = string.Empty;
    public virtual User? Killer { get; set; }

    public DateTime DeathTime { get; set; }
    public string Cause { get; set; } = string.Empty;

    public PlayerDeaths()
    {
    }
}