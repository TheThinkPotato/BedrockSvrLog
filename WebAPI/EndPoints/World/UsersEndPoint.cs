using FastEndpoints;
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class WorldEndpoint : EndpointWithoutRequest<WorldResponse>
{
    private readonly AppDbContext _db;

    public WorldEndpoint(AppDbContext db)
    {
        _db = db;
    }

    public override void Configure()
    {
        Get("/api/World/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var world = await _db.World
            .Select( w => new WorldResponse
            {
                Name = w.Name ?? "Bedrock Level",
                Seed = w.Seed
            })
            .FirstOrDefaultAsync(ct);

        if (world == null)
        {
            world = new WorldResponse();
        }

        await Send.OkAsync( world , ct);
    }
}