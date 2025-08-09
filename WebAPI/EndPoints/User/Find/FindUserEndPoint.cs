using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class FindUserEndpoint : EndpointWithoutRequest<FindUserResponse>
{
    private readonly AppDbContext _db;

    public FindUserEndpoint(AppDbContext db)
    {
        _db = db;        
    }

    public override void Configure()
    {        
        Get("/api/User/Find");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {        
        var name = Query<string>("name");

        var user = await _db.User
            .Where(u => u.Name.Trim().ToLower() == name.Trim().ToLower())
            .Select(u => new FindUserResponse
            {
                Name = u.Name.Trim() ?? "",
                Xuid = u.Xuid ?? "",
                Pfid = u.Pfid ?? ""
            }).FirstOrDefaultAsync(ct);

        if (user == null)
        {
            await Send.OkAsync(new FindUserResponse { Name = "", Xuid = "", Pfid = "" }, ct);
            return;
        }

        await Send.OkAsync(user, ct);
    }
}