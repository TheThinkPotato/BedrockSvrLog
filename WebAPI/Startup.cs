using BedrockSvrLog.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using IniParser.Model;

using IniParser;
using System.Net;
using NJsonSchema.Validation.FormatValidators;

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
            options.AddPolicy("AllowReactApp", builder =>
            {
                builder
                    .WithOrigins(webAppAddress)
                    .WithOrigins("http://localhost:5000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
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

        var app = bld.Build();

        app.UseCors("AllowReactApp")
            .UseCors("AllowLocalHostDev");

        app.UseDefaultFiles()
           .UseStaticFiles();

        app.UseFastEndpoints()
           .UseSwaggerGen();

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}