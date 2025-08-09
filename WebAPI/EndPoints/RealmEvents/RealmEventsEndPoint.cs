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
        var users = await _db.User
        .Join(_db.RealmEvent, u => u.Xuid, r => r.Xuid, (u, r) => new { u, r })
        .Select(realEvent => new RealmsEventDTO
        {
            Name = realEvent.u.Name,
            Xuid = realEvent.u.Xuid,
            Pfid = realEvent.u.Pfid,
            RealmEvent = realEvent.r.EventType,
            EventTime = realEvent.r.EventTime
        }).ToListAsync(ct);


        await Send.OkAsync(new RealmsEventResponse { Users = users }, ct);
    }
}