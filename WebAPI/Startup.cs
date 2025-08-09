using BedrockSvrLog.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using IniParser.Model;

using IniParser;

namespace WebAPI;

class Startup
{
    private static void Main(string[] args)
    {
        var bld = WebApplication.CreateBuilder();

        // Read DB path from config.ini
        var parser = new FileIniDataParser();
        IniData ini = parser.ReadFile("config.ini");

        string dbPath = ini["Database"]["Path"];
        string fullPath = Path.GetFullPath(dbPath);

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