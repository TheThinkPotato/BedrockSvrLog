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
        int port = int.Parse(ini["WebAPI"]["Port"]);


        Console.WriteLine($"Host IP: {hostIp}");
        Console.WriteLine($"Port: {port}");

        var bld = WebApplication.CreateBuilder();


        // Bind to all network interfaces on port 5000
        bld.WebHost.ConfigureKestrel(options =>
        {

            options.Listen(hostIp, port);
            options.ListenAnyIP(5000);
        });

        bld.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={fullPath}")
        );

        bld.Services.AddFastEndpoints()
           .AddFastEndpoints()
           .SwaggerDocument();

        var app = bld.Build();
        app.UseFastEndpoints()
           .UseSwaggerGen();

        app.Run();
    }
}