using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class RealmEventsEndpoint : EndpointWithoutRequest<RealmsEventResponse>
{
    private readonly AppDbContext _db;

    public RealmEventsEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/RealmEvents/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var realmEvetns = await _db.User
        .Join(_db.RealmEvent, u => u.Xuid, r => r.Xuid, (u, r) => new { u, r })
        .Select(realmEvent => new RealmEventDetails
        {
            Name = realmEvent.u.Name,
            Xuid = realmEvent.u.Xuid,
            Pfid = realmEvent.u.Pfid,
            RealmEvent = realmEvent.r.EventType,
            EventTime = realmEvent.r.EventTime,
            DiceBearAvatarUrl = realmEvent.u.AvatarLink ?? AvatarHelper.GetDiceBearAvatarUrl(realmEvent.u.Name)
        })
        .OrderByDescending(r => r.EventTime) 
        .ToListAsync(ct);


        await Send.OkAsync(new RealmsEventResponse { RealmEvents = realmEvetns }, ct);
    }
}