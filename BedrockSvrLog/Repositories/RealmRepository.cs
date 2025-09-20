using BedrockSvrLog.Data;
using BedrockSvrLog.Helpers;
using BedrockSvrLog.Models;

namespace BedrockSvrLog.Repositories;

public class RealmRepository
{
    private readonly AppDbContext _context;
    public RealmRepository(AppDbContext context)
    {
        _context = context;
    }

    public void addRealmEventToDb(DateTime eventTime, RealmStoryData realmStoryData)
    {
        try
        {
            var user = _context.User.Find(realmStoryData.Xuid);
            if (user != null)
            {
                var realmEvent = new RealmEvent
                {
                    Xuid = realmStoryData.Xuid,
                    EventTime = eventTime,
                    EventType = realmStoryData.EventType,
                };
                _context.RealmEvent.Add(realmEvent);
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            FileHelpers.WriteToDebugFile($"Error adding realm event to DB: {ex}");
        }
    }
}
