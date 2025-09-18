using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BedrockSvrLog.Models.Base;

namespace BedrockSvrLog.Models;

public class PlayerKills : PlayerDayTimeLocationSpawnEntity
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Xuid")]
    public string Xuid { get; set; } = string.Empty; // Foreign key to User table
    public virtual User? Player { get; set; }
    
    public DateTime KillTime { get; set; }
    public string EntityType { get; set; } = string.Empty;

    public PlayerKills()
    {
    }
}