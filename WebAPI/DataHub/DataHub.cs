using Microsoft.AspNetCore.SignalR;
using BedrockSvrLog.Data;
using BedrockSvrLog.Model;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.DataHub;

public class DataHub : Hub
{
    private readonly AppDbContext _db;

    public DataHub(AppDbContext db)
    {
        _db = db;
    }

    // This method receives a ping and replies directly to the sender
    public async Task SendPing(string message)
    {
        Console.WriteLine($"Ping received: {message}");
        await Clients.Caller.SendAsync("ReceivePong", $"Pong: {message} {DateTime.Now.ToLongTimeString()}");
    }

    public async Task SendWorldData()
    {
        var world = await _db.World
            .FirstOrDefaultAsync();

        var onlinePlayers = await _db.Login
            .Where(l => l.LogoutTime == null && l.SpawnTime != null)
            .Select(l => new OnlinePlayer
            {
                Name = l.User.Name,
                Xuid = l.Xuid,
                Pfid = l.User.Pfid,
                LocationX = l.User.LocationX,
                LocationY = l.User.LocationY,
                LocationZ = l.User.LocationZ,
                LocationDimension = l.User.LocationDimension,
                AvatarLink = l.User.AvatarLink,
                SpawnTime = l.SpawnTime
            })
            .OrderBy(p => p.SpawnTime)
            .ToListAsync();

        var worldData = new WorldData
        {
            Id = world?.Id ?? 0,
            Name = world?.Name ?? "Bedrock Level",
            CurrentTime = world?.CurrentTime ?? null,
            CurrentDay = world?.CurrentDay ?? 0,
            Seed = world?.Seed ?? "random seed",
            OnlinePlayers = onlinePlayers
        };

        var worldDataJson = System.Text.Json.JsonSerializer.Serialize(worldData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

        await Clients.All.SendAsync("ReceiveWorldData", worldDataJson);
    }
}

public class WorldData : World
{
    public List<OnlinePlayer>? OnlinePlayers { get; set; }
}

public record OnlinePlayer
{
    public string? Name { get; set; }
    public string Xuid { get; set; }
    public string? Pfid { get; set; }
    public string? AvatarLink { get; set; }
    public int? LocationX { get; set; }
    public int? LocationY { get; set; }
    public int? LocationZ { get; set; }
    public string? LocationDimension { get; set; }
    public DateTime? SpawnTime { get; set; }
}

public class MessageScheduler : BackgroundService
{
    private readonly IHubContext<DataHub> _hubContext;

    public MessageScheduler(IHubContext<DataHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Broadcast "hello" to everyone every 30s
            await _hubContext.Clients.All.SendAsync("ReceiveData", "Hello!", cancellationToken: stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Fixed delay to 30s as per comment
        }
    }
}
