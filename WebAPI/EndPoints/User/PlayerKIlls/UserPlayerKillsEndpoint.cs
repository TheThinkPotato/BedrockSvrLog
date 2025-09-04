using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.EndPoints.User.PlayerKIlls;

public class UserPlayerKillsEndpoint : EndpointWithoutRequest<UserPlayerKillsResponse>
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

        UserPlayerKillsResponse response;

        if (userPlayerKills == null)
        {
            response = response = new UserPlayerKillsResponse();
        }
        else
        {
            var group = userPlayerKills.First();
            var playerKillsList = group
                    .GroupBy(x => x.pk.EntityType)
                    .Select(eg => new PlayerKillListUser
                    {
                        EntityType = eg.Key,
                        KillCount = eg.Count()
                    }).ToList();

            response = response = new UserPlayerKillsResponse
                                            {
                                                Xuid = group.Key.Xuid,
                                                Name = group.Key.Name,
                                                TotalKills = group.Count(),
                                                PlayerKillsList = playerKillsList
                                            };
        }

        await Send.OkAsync(response, ct);
    }
}
