using FastEndpoints;
using FastEndpoints.Swagger;

class Startup
{
    private static void Main(string[] args)
    {
        var bld = WebApplication.CreateBuilder();
        bld.Services.AddFastEndpoints()
           .AddFastEndpoints()
           .SwaggerDocument();

        var app = bld.Build();
        app.UseFastEndpoints()
           .UseSwaggerGen();

        app.Run();
    }
}