using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class PlayerKillsEndPoint : EndpointWithoutRequest<PlayerKillsResponse>
{
    private readonly AppDbContext _db;

    public PlayerKillsEndPoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/PlayerKills/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var killList = _db.PlayerKills.GroupBy(pk => new { pk.Xuid, pk.EntityType })
            .Select(g => new
            {
                g.Key.Xuid,
                g.Key.EntityType,
                KillCount = g.Count()
            }).OrderByDescending(pk => pk.KillCount);

        var users = _db.User.Select(u => new { u.Xuid, u.Name });

        // Move to client side
        var userPlayerKills = await _db.User
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

