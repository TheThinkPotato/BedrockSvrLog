using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class UsersOnlineEndpoint : EndpointWithoutRequest<UsersOnlineResponse>
{
    private readonly AppDbContext _db;

    public UsersOnlineEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/Users/Online");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await _db.User
            .Join(_db.Login, u => u.Xuid, l => l.Xuid, (u, l) => new { u, l })
            .Where( u => u.l.LogoutTime == null)
            .Select(u => new UserOnlineDTO
            {
                Name = u.u.Name,
                Xuid = u.u.Xuid,
                Pfid = u.u.Pfid,
                LastLogin = u.l.LoginTime,
                SpawnTime = u.l.SpawnTime ?? DateTime.MinValue,
                DiceBearAvatarUrl = AvatarHelper.GetDiceBearAvatarUrl(u.u.Name)
            }).ToListAsync(ct);

        await Send.OkAsync( new UsersOnlineResponse {Users = users }, ct);
    }
}
