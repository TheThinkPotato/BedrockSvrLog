using BedrockSvrLog.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BedrockSvrLog.Models;

public class Article
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Xuid")]
    public string? PlayerXuid { get; set; } 
    public virtual User? User { get; set; }
    [ForeignKey("Id")]
    public int? PlayerDeathId { get; set; } 
    public virtual PlayerDeaths? PlayerDeath { get; set; }
    [ForeignKey("Id")]
    public int? PlayerKillsId { get; set; } 
    public virtual PlayerKills? PlayerKills { get; set; }
    [ForeignKey("Id")]
    public int? RealmEventId { get; set; } 
    public virtual RealmEvent? RealmEvent { get; set; }
    [ForeignKey("Id")]
    public int PaperId { get; set; }
    public required virtual Paper Paper { get; set; }

    public required string Title { get; set; }
    public string? Subtitle { get; set; }
    public required string Content { get; set; }
    public DateTime PublishedDate { get; set; }
    public required List<string> Tags { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int GameDay { get; set; }
    public int GameTime { get; set; }
    public int PlayerSpawnX { get; set; }
    public int PlayerSpawnY { get; set; }
    public int PlayerSpawnZ { get; set; }

}
