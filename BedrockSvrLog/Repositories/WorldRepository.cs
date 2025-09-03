using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;
using BedrockSvrLog.Model;
using BedrockSvrLog.Helpers;

namespace BedrockSvrLog.Repositories;

public class WorldRepository
{
    private readonly AppDbContext _context;

    public WorldRepository(AppDbContext context)
    {
        _context = context;
    }

    public void UpdateWorldName(string worldName)
    {
        try
        {
            var world = _context.World.FirstOrDefault();
            if (world == null)
            {
                world = new World
                {
                    Name = worldName
                };
                _context.World.Add(world);
            }
            else
            {
                world.Name = worldName;
            }
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating world name in DB: {ex}");
        }
    }


    public void UpdateWorldSeed(string worldSeed)
    {
        try
        {
            var world = _context.World.FirstOrDefault();
            if (world == null)
            {
                world = new World
                {
                    Seed = worldSeed
                };
                _context.World.Add(world);
            }
            else
            {
                world.Seed = worldSeed;
            }
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating world seed in DB: {ex}");
        }
    }

    public async Task UpdateWorldTableData(WorldTimeDaySpawnPoint worldTimeDaySpawnPoint, CancellationToken ct)
    {
        try
        {
            var world = await _context.World.FirstOrDefaultAsync(ct);

            if (world != null)
            {
                world.CurrentDay = worldTimeDaySpawnPoint.Day;
                world.CurrentTime = worldTimeDaySpawnPoint.Time;
                world.SpawnX = worldTimeDaySpawnPoint.SpawnX;
                world.SpawnY = worldTimeDaySpawnPoint.SpawnY;
                world.SpawnZ = worldTimeDaySpawnPoint.SpawnZ;
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating world day and time in DB: {ex}");
        }
    }

    public async Task<World?> getCurrentWorldDetails(CancellationToken ct)
    {
        return await _context.World.FirstOrDefaultAsync(ct);
    }

}
public record WorldTimeDaySpawnPoint
{
    public int? Time { get; set; }
    public int Day { get; set; }
    public int? SpawnX { get; set; }
    public int? SpawnY { get; set; }
    public int? SpawnZ { get; set; }
}
