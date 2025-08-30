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
            .GroupBy(x => new { x.u.Name, x.u.Xuid, x.u.Pfid, x.u.AvatarLink })
            .Select(g => new DurationDTO
            {
                Name = g.Key.Name,
                Xuid = g.Key.Xuid,
                Pfid = g.Key.Pfid,
                DiceBearAvatarUrl = g.Key.AvatarLink ?? AvatarHelper.GetDiceBearAvatarUrl(g.Key.Name),
                TotalDuration = TimeSpan.FromSeconds(g.Sum(x => x.l.Duration?.TotalSeconds ?? 0)),
                TotalGameplayDuration = TimeSpan.FromSeconds(g.Sum(x => x.l.GameplayeDuration?.TotalSeconds ?? 0)),
                TotalLiveDuration = g.Any(x => x.l.LogoutTime == null) ? DateTime.Now.Subtract(g.Max(x => x.l.SpawnTime ?? DateTime.Now)) +
                                            TimeSpan.FromSeconds(g.Sum(x => x.l.Duration?.TotalSeconds ?? 0)) :
                                            TimeSpan.FromSeconds(g.Sum(x => x.l.Duration?.TotalSeconds ?? 0)),
                TotalLiveGameplayDuration = g.Any(x => x.l.LogoutTime == null) ? DateTime.Now.Subtract(g.Max(x => x.l.SpawnTime ?? DateTime.Now)) +
                                            TimeSpan.FromSeconds(g.Sum(x => x.l.GameplayeDuration?.TotalSeconds ?? 0)) :
                                            TimeSpan.FromSeconds(g.Sum(x => x.l.GameplayeDuration?.TotalSeconds ?? 0)),
                LastLogin = g.Max(x => x.l.LoginTime),
                LastLogOut = g.Max(x => x.l.LogoutTime),
                SpawnTime = g.Max(x => x.l.SpawnTime ?? DateTime.Now),
                IsOnline = g.Any(x => x.l.LogoutTime == null && x.l.SpawnTime != null)
            })
            .OrderByDescending(x => x.TotalLiveDuration)
            .ToList();

        await Send.OkAsync(new DurationsResponse { Durations = durations }, ct);
    }
}

