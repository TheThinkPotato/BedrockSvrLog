using BedrockSvrLog.Data;
using WebAPI.DataHub;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using IniParser.Model;

using IniParser;
using System.Net;

namespace WebAPI;
class Startup
{
    private static void Main(string[] args)
    {
        // Read DB path from config.ini
        var parser = new FileIniDataParser();
        IniData ini = parser.ReadFile("config.ini");

        string dbPath = ini["Database"]["Path"];
        string fullPath = Path.GetFullPath(dbPath);

        //// start new task that runs MapPinUpdater every 5 minutes
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite($"Data Source={fullPath}");
        var mapPinUpdater = new MapPinUpdater(new AppDbContext(optionsBuilder.Options));

        Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    await mapPinUpdater.createUnMinedCustomMarkerJsAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating custom markers: {ex.Message}");
                }
                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        });

        IPAddress hostIp = IPAddress.Parse(ini["WebAPI"]["HostIP"]);
        int hostPort = int.Parse(ini["WebAPI"]["Port"]);

        string appIp = (ini["WebApp"]["AppIP"]);
        int appPort = int.Parse(ini["WebAPI"]["Port"]);
        var webAppAddress = $"http://{appIp}:{appPort}";

        Console.WriteLine($"Host IP: {hostIp}");
        Console.WriteLine($"Port: {hostPort}");

        var bld = WebApplication.CreateBuilder();

        bld.Services.AddCors(options =>
        {
            //options.AddPolicy("AllowAll", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //           .AllowAnyMethod()
            //           .AllowAnyHeader();
            //});
            options.AddPolicy("AllowReactApp", builder =>
            {
                builder
                    .WithOrigins(webAppAddress, "http://localhost:5173", "http://localhost:5000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(_ => true); // Required for SignalR with WebSockets
            });
        });

        Console.WriteLine($"CORS policy configured to allow React app at: {webAppAddress}");

        

        // Bind to all network interfaces on port 5000
        bld.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(hostIp, hostPort);
            options.ListenAnyIP(5000);
        });

        bld.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={fullPath}")
        );

        bld.Services.AddFastEndpoints()
           .AddFastEndpoints()
           .SwaggerDocument();

        bld.Services.AddSignalR();
        bld.Services.AddHostedService<WebAPI.DataHub.MessageScheduler>();

        var app = bld.Build();

        //app.UseCors("AllowReactApp")
        //    .UseCors("AllowLocalHostDev");

        app.UseDefaultFiles()
           .UseStaticFiles();

        app.UseFastEndpoints()
           .UseSwaggerGen();

        app.UseCors("AllowReactApp");

        app.MapHub<DataHub.DataHub>("/dataHub");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}