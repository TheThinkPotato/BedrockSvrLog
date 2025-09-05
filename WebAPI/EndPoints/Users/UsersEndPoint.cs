using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class UsersEndpoint : EndpointWithoutRequest<UsersResponse[]>
{
    private readonly AppDbContext _db;

    public UsersEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/Users/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var users = await _db.User
            .Select(u => new UsersResponse
            {
                Name = u.Name,
                Xuid = u.Xuid,
                Pfid = u.Pfid,
                DiceBearAvatarUrl = u.AvatarLink ?? AvatarHelper.GetDiceBearAvatarUrl(u.Name)
            }).ToListAsync(ct);

        await Send.OkAsync(users.ToArray(), ct);
    }
}