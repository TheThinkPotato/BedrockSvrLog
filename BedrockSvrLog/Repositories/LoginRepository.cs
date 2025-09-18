using BedrockSvrLog.Data;
using BedrockSvrLog.Helpers;
using BedrockSvrLog.Models;

namespace BedrockSvrLog.Repositories;

public class LoginRepository
{
    private readonly AppDbContext _context;

    public LoginRepository(AppDbContext context)
    {
        _context = context;
    }

    public void addUserLoginToDb(string xuid, DateTime loginTime)
    {
        FileHelpers.writeToDebugFile($"Debug: Adding user login to database: XUID: {xuid}, Login Time: {loginTime}");
        try
        {
            var user = _context.User.Find(xuid);
            if (user != null)
            {
                var login = new Login
                {
                    Xuid = xuid,
                    LoginTime = loginTime
                };
                _context.Login.Add(login);
                _context.SaveChanges();
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
            var login = _context.Login.OrderByDescending(l => l.LoginTime)
                .FirstOrDefault(l => l.Xuid == xuid && l.SpawnTime == null);
            if (login != null)
            {
                login.SpawnTime = spawnTime;
                _context.SaveChanges();
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
            var login = _context.Login.OrderByDescending(l => l.LoginTime)
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
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.writeToDebugFile($"Error updating user login logout time in DB: {ex}");
        }
    }
}
