using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAPI.DataHub;

public class DataHub : Hub
{
    public async Task SendMessage(string data) // Fixed typo: SendMEssage -> SendMessage
    {
        await Clients.All.SendAsync("ReceiveData", data);
    }

    // This method receives a ping and replies directly to the sender
    public async Task SendPing(string message)
    {
        Console.WriteLine($"Ping received: {message}");
        await Clients.Caller.SendAsync("ReceivePong", $"Pong: {message}");
    }

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
