using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class UserRealmEndpoint : EndpointWithoutRequest<UserRealmResponse>
{
    private readonly AppDbContext _db;

    public UserRealmEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {        
        Get("/api/User/{xuid}/RealmEvents");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userXuid = Route<string>("xuid");
        Console.WriteLine($"UserRealmEndpoint: User XUID: {userXuid}");

        var events = await _db.User
            .Where(u => u.Xuid == userXuid)
            .Join(_db.RealmEvent, u => u.Xuid, r => r.Xuid, (u, r) => new { u, r })
            .Select(ur => new RealmEventDetails
            {
                Name = ur.u.Name,
                Xuid = ur.u.Xuid,
                Pfid = ur.u.Pfid,
                DiceBearAvatarUrl = AvatarHelper.GetDiceBearAvatarUrl(ur.u.Name),
                RealmEvent = ur.r.EventType,
                EventTime = ur.r.EventTime
            })
            .OrderByDescending(re => re.EventTime)
            .ToListAsync(ct);



        if (events == null)
        {
            await Send.OkAsync(new UserRealmResponse (), ct);
            return;
        }

        await Send.OkAsync(new UserRealmResponse { RealmEvents = events } , ct);
    }
}