using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BedrockSvrLog.Model;

namespace BedrockSvrLog;

public class DbHelpers
{
    private readonly AppDbContext MyAppDbContext;

    public DbHelpers(AppDbContext myAppDbContext)
    {
        MyAppDbContext = myAppDbContext;
    }

    public void UpdateWorldName(string worldName)
    {
        try
        {
            var world = MyAppDbContext.World.FirstOrDefault();
            if (world == null)
            {
                world = new World
                {
                    Name = worldName
                };
                MyAppDbContext.World.Add(world);
            }
            else
            {
                world.Name = worldName;
            }
            MyAppDbContext.SaveChanges();
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
            var world = MyAppDbContext.World.FirstOrDefault();
            if (world == null)
            {
                world = new World
                {
                    Seed = worldSeed
                };
                MyAppDbContext.World.Add(world);
            }
            else
            {
                world.Seed = worldSeed;
            }
            MyAppDbContext.SaveChanges();
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
            var world = await MyAppDbContext.World.FirstOrDefaultAsync(ct);
            
            if (world != null)
            {
                world.CurrentDay = worldTimeDaySpawnPoint.Day;
                world.CurrentTime = worldTimeDaySpawnPoint.Time;
                world.SpawnX = worldTimeDaySpawnPoint.SpawnX;
                world.SpawnY = worldTimeDaySpawnPoint.SpawnY;
                world.SpawnZ = worldTimeDaySpawnPoint.SpawnZ;
                MyAppDbContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating world day and time in DB: {ex}");
        }
    }
    public void addUserToDb(string name, string xuid, string? pfid)
    {
        FileHelpers.writeToDebugFile($"Debug: Adding new user to database: Name: {name}, XUID: {xuid}, PFID: {pfid}");
        try
        {

            // Check if user already exists (by Xuid, which is [Key])
            var existingUser = MyAppDbContext.User.Find(xuid);
            if (existingUser == null)
            {
                var user = new User
                {
                    Name = name,
                    Xuid = xuid,
                    Pfid = pfid ?? string.Empty // Use empty string if pfid is null
                };
                MyAppDbContext.User.Add(user);
                MyAppDbContext.SaveChanges();
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
            var user = MyAppDbContext.User.Find(xuid);
            if (user != null)
            {
                user.Pfid = pfid;
                MyAppDbContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating user PFID in DB: {ex}");
        }
    }

    public void addUserLoginToDb(string xuid, DateTime loginTime)
    {
        FileHelpers.writeToDebugFile($"Debug: Adding user login to database: XUID: {xuid}, Login Time: {loginTime}");
        try
        {
            var user = MyAppDbContext.User.Find(xuid);
            if (user != null)
            {
                var login = new Login
                {
                    Xuid = xuid,
                    LoginTime = loginTime
                };
                MyAppDbContext.Login.Add(login);
                MyAppDbContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error adding user login to DB: {ex}");
        }
    }

    public void updateUserLoginSpawnTime(string xuid, DateTime spawnTime)
    {
        FileHelpers.writeToDebugFile($"Debug: Updating user login spawn time in database: XUID: {xuid}, Spawn Time: {spawnTime}");
        try
        {
            var login = MyAppDbContext.Login.OrderByDescending(l => l.LoginTime)
                .FirstOrDefault(l => l.Xuid == xuid && l.SpawnTime == null);
            if (login != null)
            {
                login.SpawnTime = spawnTime;
                MyAppDbContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating user login spawn time in DB: {ex}");
        }
    }

    public void updateUserLoginLogoutTime(string xuid, DateTime logoutTime)
    {
        FileHelpers.writeToDebugFile($"Debug: Updating user login logout time in database: XUID: {xuid}, Logout Time: {logoutTime}");
        try
        {
            var login = MyAppDbContext.Login.OrderByDescending(l => l.LoginTime)
                .FirstOrDefault(l => l.Xuid == xuid && l.LogoutTime == null);
            if (login != null)
            {
                login.LogoutTime = logoutTime;
                login.Duration = logoutTime - login.LoginTime;
                if (login.SpawnTime.HasValue)
                {
                    login.GameplayeDuration = logoutTime - login.SpawnTime.Value;
                }
                else
                {
                    login.GameplayeDuration = null; // No spawn time, so no gameplay duration
                }
                MyAppDbContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating user login logout time in DB: {ex}");
        }
    }

    public void addRealmEventToDb(DateTime eventTime, RealmStoryData realmStoryData)
    {
        try
        {
            var user = MyAppDbContext.User.Find(realmStoryData.Xuid);
            if (user != null)
            {
                var realmEvent = new RealmEvent
                {
                    Xuid = realmStoryData.Xuid,
                    EventTime = eventTime,
                    EventType = realmStoryData.EventType,
                };
                MyAppDbContext.RealmEvent.Add(realmEvent);
                MyAppDbContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error adding realm event to DB: {ex}");
        }
    }

    public async Task UpdateUserLocationAsync(EntityLocation location, CancellationToken ct)
    {
        FileHelpers.writeToDebugFile($"Debug: Updating user location in database: XUID: {location.EntityName}, Location: {location}");

        var user = await MyAppDbContext.User.Where(u => u.Name == location.EntityName).SingleOrDefaultAsync(ct);
        if (user != null)
        {
            user.LocationX = location.x;
            user.LocationY = location.y;
            user.LocationZ = location.z;
            user.LocationDimension = location.dimension;
            user.SpawnX = location.SpawnX;
            user.SpawnY = location.SpawnY;
            user.SpawnZ = location.SpawnZ;
            MyAppDbContext.SaveChanges();
        }
    }

    public async Task UpdateMissingUserAvatarLinks(CancellationToken ct)
    {
        const string BackgroundColor = "b6e3f4";
        const string Size = "64";

        var baseUrl = $"https://api.dicebear.com/9.x/pixel-art/svg?seed=gamer&size={Size}&backgroundColor={BackgroundColor}";

        try
        {
            var usersWithoutAvatar = await MyAppDbContext.User
                .Where(u => string.IsNullOrEmpty(u.AvatarLink) && !string.IsNullOrEmpty(u.Pfid))
                .ToListAsync(ct);

            foreach (var user in usersWithoutAvatar)
            {
                user.AvatarLink = baseUrl.Replace("gamer",user.Name);
            }
            await MyAppDbContext.SaveChangesAsync(ct);
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

public record WorldTimeDaySpawnPoint
{
    public required string Time { get; set; }
    public int Day { get; set; }
    public int? SpawnX { get; set; }
    public int? SpawnY { get; set; }
    public int? SpawnZ { get; set; }
}
