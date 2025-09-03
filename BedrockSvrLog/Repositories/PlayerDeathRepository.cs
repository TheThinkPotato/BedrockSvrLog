using BedrockSvrLog.Data;
using BedrockSvrLog.Model;


namespace BedrockSvrLog.Repositories;

public class PlayerDeathRepository
{
    private readonly AppDbContext _context;

    public PlayerDeathRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddPlayerDeathEventToDbAsync(EntityDeath entityDeath, User userDetails, World currentWorldDetails, CancellationToken ct)
    {

        // Break out if no user or world details found
        if (userDetails == null || currentWorldDetails == null)
        {
            return;
        }

        var playerDeathDetails = new PlayerDeaths
        {
            Xuid = userDetails.Xuid ?? "Unknown",
            DeathTime = DateTime.Now,
            GameDay = currentWorldDetails.CurrentDay,
            GameTime = currentWorldDetails.CurrentTime,
            SpawnPositionX = userDetails.SpawnX ?? currentWorldDetails.SpawnX,
            SpawnPositionY = userDetails.SpawnY ?? currentWorldDetails.SpawnY,
            SpawnPositionZ = userDetails.SpawnZ ?? currentWorldDetails.SpawnZ,
            PositionX = entityDeath.PositionX,
            PositionY = entityDeath.PositionY,
            PositionZ = entityDeath.PositionZ,
            Dimension = entityDeath.Dimension,
            Cause = entityDeath.Cause ?? "Unknown",
            KillerXuid = entityDeath.KillerXuid ?? null,
        };

        _context.PlayerDeaths.Add(playerDeathDetails);
        await _context.SaveChangesAsync(ct);
    }

}
