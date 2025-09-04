using BedrockSvrLog.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class UserDurationEndPoint: EndpointWithoutRequest<UserDurationResponse>
{

    private readonly AppDbContext _db;

    public UserDurationEndPoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/User/{xuid}/userDurations");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var xuid = Route<string>("xuid");

        var durations = await _db.User.Where(u => u.Xuid == xuid)
            .Join(_db.Login, u => u.Xuid, l => l.Xuid, (u, l) => new { u, l })
            .ToListAsync(ct);

        if(durations == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        if (!durations.Any())
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        await Send.OkAsync( new UserDurationResponse
        {
            Name = durations.First().u.Name,
            Xuid = durations.First().u.Xuid,
            Pfid = durations.First().u.Pfid,
            DiceBearAvatarUrl = durations.First().u.AvatarLink ?? AvatarHelper.GetDiceBearAvatarUrl(durations.First().u.Name),
            TotalDuration = TimeSpan.FromSeconds(durations.Sum(x => x.l.Duration?.TotalSeconds ?? 0)),
            TotalGameplayDuration = TimeSpan.FromSeconds(durations.Sum(x => x.l.GameplayeDuration?.TotalSeconds ?? 0)),
            TotalLiveDuration = durations.Any(x => x.l.LogoutTime == null) ? DateTime.Now.Subtract(durations.Max(x => x.l.SpawnTime ?? DateTime.Now)) +
                                            TimeSpan.FromSeconds(durations.Sum(x => x.l.Duration?.TotalSeconds ?? 0)) :
                                            TimeSpan.FromSeconds(durations.Sum(x => x.l.Duration?.TotalSeconds ?? 0)),
            TotalLiveGameplayDuration = durations.Any(x => x.l.LogoutTime == null) ? DateTime.Now.Subtract(durations.Max(x => x.l.SpawnTime ?? DateTime.Now)) +
                                            TimeSpan.FromSeconds(durations.Sum(x => x.l.GameplayeDuration?.TotalSeconds ?? 0)) :
                                            TimeSpan.FromSeconds(durations.Sum(x => x.l.GameplayeDuration?.TotalSeconds ?? 0)),
            LastLogin = durations.Max(x => x.l.LoginTime),
            LastLogOut = durations.Max(x => x.l.LogoutTime),
            SpawnTime = durations.Max(x => x.l.SpawnTime ?? DateTime.Now),
            IsOnline = durations.Any(x => x.l.LogoutTime == null && x.l.SpawnTime != null)
        });
    }
}
