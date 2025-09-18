using BedrockSvrLog.Data;
using BedrockSvrLog.Models;

namespace BedrockSvrLog.Repositories;

public class PlayerKillRepository
{
    private readonly AppDbContext _context;

    public PlayerKillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddKillEventToDbAsync(EntityDeath entityDeath, User userDetails, World currentWorldDetails,  CancellationToken ct)
    {

        // Break out if no user or world details found
        if (userDetails == null || currentWorldDetails == null)
        {
            return;
        }

        var playerKillDetails = new PlayerKills
        {
            Xuid = userDetails.Xuid ?? "Unknown",
            KillTime = DateTime.Now,
            EntityType = entityDeath.EntityType,
            GameDay = currentWorldDetails.CurrentDay,
            GameTime = currentWorldDetails.CurrentTime,
            SpawnPositionX = userDetails.SpawnX ?? currentWorldDetails.SpawnX,
            SpawnPositionY = userDetails.SpawnY ?? currentWorldDetails.SpawnY,
            SpawnPositionZ = userDetails.SpawnZ ?? currentWorldDetails.SpawnZ,
            PositionX = entityDeath.PositionX,
            PositionY = entityDeath.PositionY,
            PositionZ = entityDeath.PositionZ,
            Dimension = entityDeath.Dimension,
        };

        _context.PlayerKills.Add(playerKillDetails);
        await _context.SaveChangesAsync(ct);
    }
}

public record EntityDeath
{
    public string EntityType { get; set; } = string.Empty;
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public int PositionZ { get; set; }
    public string Dimension { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public string? Cause { get; set; }
    public string? KillerXuid { get; set; }
}
