using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class DurationsEndpoint : EndpointWithoutRequest<DurationsResponse>
{
    private readonly AppDbContext _db;

    public DurationsEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/Durations/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var durations = _db.User
            .Join(_db.Login, u => u.Xuid, l => l.Xuid, (u, l) => new { u, l })
            .AsEnumerable() // Move to client-side evaluation
            .GroupBy(x => new { x.u.Name, x.u.Xuid, x.u.Pfid })
            .Select(g => new DurationDTO
            {
                Name = g.Key.Name,
                Xuid = g.Key.Xuid,
                Pfid = g.Key.Pfid,
                Duration = TimeSpan.FromSeconds(g.Sum(x => x.l.Duration?.TotalSeconds ?? 0)),
                GameplayDuration = TimeSpan.FromSeconds(g.Sum(x => x.l.GameplayeDuration?.TotalSeconds ?? 0)),
            })
            .ToList();

        await Send.OkAsync(new DurationsResponse { Durations = durations }, ct);
    }
}

