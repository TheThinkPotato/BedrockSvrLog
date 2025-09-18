using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;
using BedrockSvrLog.Models;
using BedrockSvrLog.Helpers;

namespace BedrockSvrLog.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public void addUserToDb(string name, string xuid, string? pfid)
    {
        FileHelpers.writeToDebugFile($"Debug: Adding new user to database: Name: {name}, XUID: {xuid}, PFID: {pfid}");
        try
        {

            // Check if user already exists (by Xuid, which is [Key])
            var existingUser = _context.User.Find(xuid);
            if (existingUser == null)
            {
                var user = new User
                {
                    Name = name,
                    Xuid = xuid,
                    Pfid = pfid ?? string.Empty // Use empty string if pfid is null
                };
                _context.User.Add(user);
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error adding user to DB: {ex}");
        }
    }

    public void updateuserPfid(string xuid, string pfid)
    {
        try
        {
            var user = _context.User.Find(xuid);
            if (user != null)
            {
                user.Pfid = pfid;
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating user PFID in DB: {ex}");
        }
    }

    public async Task UpdateUserLocationAsync(EntityLocation location, CancellationToken ct)
    {
        FileHelpers.writeToDebugFile($"Debug: Updating user location in database: XUID: {location.EntityName}, Location: {location}");

        var user = await _context.User.Where(u => u.Name == location.EntityName).SingleOrDefaultAsync(ct);
        if (user != null)
        {
            user.LocationX = location.x;
            user.LocationY = location.y;
            user.LocationZ = location.z;
            user.LocationDimension = location.dimension;
            user.SpawnX = location.SpawnX;
            user.SpawnY = location.SpawnY;
            user.SpawnZ = location.SpawnZ;
            _context.SaveChanges();
        }
    }

    public async Task<User?> getCurrentUserDetails(string gameTagName, CancellationToken ct)
    {
        return await _context.User.Where(u => u.Name == gameTagName).SingleOrDefaultAsync(ct);
    }

    public async Task UpdateMissingUserAvatarLinks(CancellationToken ct)
    {
        const string BackgroundColor = "b6e3f4";
        const string Size = "64";

        var baseUrl = $"https://api.dicebear.com/9.x/pixel-art/svg?seed=gamer&size={Size}&backgroundColor={BackgroundColor}";

        try
        {
            var usersWithoutAvatar = await _context.User
                .Where(u => string.IsNullOrEmpty(u.AvatarLink) && !string.IsNullOrEmpty(u.Pfid))
                .ToListAsync(ct);

            foreach (var user in usersWithoutAvatar)
            {
                user.AvatarLink = baseUrl.Replace("gamer", user.Name);
            }
            await _context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating user avatar links in DB: {ex}");
        }
    }
}

public record EntityLocation
{
    public string? EntityName { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int z { get; set; }
    public int? SpawnX { get; set; }
    public int? SpawnY { get; set; }
    public int? SpawnZ { get; set; }
    public string dimension { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"X: {x}, Y: {y}, Z: {z}, Dimension: {dimension}";
    }
}