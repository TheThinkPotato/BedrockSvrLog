using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class UserEndpoint : EndpointWithoutRequest<UserResponse>
{
    private readonly AppDbContext _db;

    public UserEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/User/{xuid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var xuid = Route<string>("xuid");

        var user = await _db.User
            .Where(u => u.Xuid == xuid)
            .Select(u => new UserResponse
            {
                Name = u.Name,
                Xuid = u.Xuid,
                Pfid = u.Pfid,
                DiceBearAvatarUrl = AvatarHelper.GetDiceBearAvatarUrl(u.Name)
            }).FirstOrDefaultAsync(ct);

        if (user == null)
        {
            await Send.NotFoundAsync(ct);
        }
        else
        {
            await Send.OkAsync(user, ct);
        }
    }
}