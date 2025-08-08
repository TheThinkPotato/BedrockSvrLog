using BedrockSvrLog.Data;


namespace BedrockSvrLog;

public class DbHelpers
{
    private readonly AppDbContext MyAppDbContext;

    public DbHelpers(AppDbContext myAppDbContext)
    {
        MyAppDbContext = myAppDbContext;
    }
    public void addUserToDb(string name, string xuid, string? pfid)
    {
        Program.writeToDebugFile($"Debug: Adding new user to database: Name: {name}, XUID: {xuid}, PFID: {pfid}");
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
            Program.writeToDebugFile($"Error adding user to DB: {ex}");
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
            Program.writeToDebugFile($"Error updating user PFID in DB: {ex}");
        }
    }

    public void addUserLoginToDb(string xuid, DateTime loginTime)
    {
        Program.writeToDebugFile($"Debug: Adding user login to database: XUID: {xuid}, Login Time: {loginTime}");
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
            Program.writeToDebugFile($"Error adding user login to DB: {ex}");
        }
    }

    public void updateUserLoginSpawnTime(string xuid, DateTime spawnTime)
    {
        Program.writeToDebugFile($"Debug: Updating user login spawn time in database: XUID: {xuid}, Spawn Time: {spawnTime}");
        try
        {
            var login = MyAppDbContext.Login.FirstOrDefault(l => l.Xuid == xuid && l.SpawnTime == null);
            if (login != null)
            {
                login.SpawnTime = spawnTime;
                MyAppDbContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Program.writeToDebugFile($"Error updating user login spawn time in DB: {ex}");
        }
    }

    public void updateUserLoginLogoutTime(string xuid, DateTime logoutTime)
    {
        Program.writeToDebugFile($"Debug: Updating user login logout time in database: XUID: {xuid}, Logout Time: {logoutTime}");
        try
        {
            var login = MyAppDbContext.Login.FirstOrDefault(l => l.Xuid == xuid && l.LogoutTime == null);
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
            Program.writeToDebugFile($"Error updating user login logout time in DB: {ex}");
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
            Program.writeToDebugFile($"Error adding realm event to DB: {ex}");
        }
    }
}
