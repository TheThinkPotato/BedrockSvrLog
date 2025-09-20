using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BedrockSvrLog.Models.Base;

namespace BedrockSvrLog.Models;

public class PlayerDeaths : PlayerDayTimeLocationSpawnEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Player))]
    public string Xuid { get; set; } = string.Empty;

    public virtual User? Player { get; set; }

    [ForeignKey(nameof(Killer))]
    public string? KillerXuid { get; set; }

    public virtual User? Killer { get; set; }

    public DateTime DeathTime { get; set; }
    public string Cause { get; set; } = string.Empty;

    public PlayerDeaths()
    {
    }
}
