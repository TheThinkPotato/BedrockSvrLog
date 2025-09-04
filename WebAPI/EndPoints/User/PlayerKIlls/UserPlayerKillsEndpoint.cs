using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.EndPoints.User.PlayerKIlls;

public class UserPlayerKillsEndpoint : EndpointWithoutRequest<PlayerKillsResponse>
{
    private readonly AppDbContext _db;

    public UserPlayerKillsEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/User/{xuid}/PlayerKills");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var xuid = Route<string>("xuid");

        var userPlayerKills = await _db.User
                                .Where(u => u.Xuid == xuid)
                                .Join(_db.PlayerKills, u => u.Xuid, pk => pk.Xuid, (u, pk) => new { u, pk })
                                .GroupBy(upk => new { upk.u.Xuid, upk.u.Name })
                                .ToListAsync(ct);

        var playerKills = userPlayerKills.Select(g => new PlayerKills
        {
            Xuid = g.Key.Xuid,
            Name = g.Key.Name,
            TotalKills = g.Count(),
            PlayerKillsList = g.GroupBy(x => x.pk.EntityType)
                                        .Select(eg => new PlayerKillList
                                        {
                                            EntityType = eg.Key,
                                            KillCount = eg.Count()
                                        }).ToList()
        }).ToList();

        var response = new PlayerKillsResponse
        {
            PlayerKills = playerKills
        };

        await Send.OkAsync(response, ct);
    }
}
