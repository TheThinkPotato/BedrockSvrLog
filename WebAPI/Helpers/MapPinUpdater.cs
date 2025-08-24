
using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class MapPinUpdater
{
    private readonly AppDbContext _db;
    public const string CustomMarkerFileName = @"wwwroot\map\custom.markers.js";

    public MapPinUpdater(AppDbContext db)
    {
        _db = db;
    }

    public async Task createUnMinedCustomMarkerJsAsync()
    {

        var db1 = await _db.User.ToListAsync();

        var customMarkers = await _db.User
            .Where(u => (u.LocationDimension != null && u.LocationDimension == "overworld"))
            .Select(u => new UnMinedCustomMarker
            {
                x = u.LocationX.Value,
                z = u.LocationZ.Value,
                image = "playerpin.png",
                imageAnchor = new double[] { 0.5, 0.5 },
                imageScale = 0.08,
                offsetX = 0,
                offsetY = 200
            }).ToArrayAsync();

        if (customMarkers == null || !customMarkers.Any())
        {
            //Console.WriteLine("No custom markers found to update.");
            return;
        }

        var unminedCustomMarkerObject = new UnminedCustomMarkerObject
        {
            isEnabled = true,
            markers = customMarkers.ToList()
        };

        var json = $"UnminedCustomMarkers = {System.Text.Json.JsonSerializer
                            .Serialize(unminedCustomMarkerObject, new System.Text.Json.JsonSerializerOptions { WriteIndented = true })}";

        var filePath = Path.Combine(GetWebRootPath(), CustomMarkerFileName);
        try
        {
            await File.WriteAllTextAsync(filePath, json);
            //Console.WriteLine($"Custom markers updated successfully at {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing custom markers to file: {ex}");
        }
    }

    public static string GetWebRootPath()
    {
        // Assuming the web root path is the current directory, adjust as necessary
        return Directory.GetCurrentDirectory();
    }

}



public record UnminedCustomMarkerObject
{
    public bool isEnabled { get; set; } = true;
    public List<UnMinedCustomMarker> markers { get; set; } = new List<UnMinedCustomMarker>();
}

public record UnMinedCustomMarker
{
    public int x { get; set; }
    public int z { get; set; }
    public string image { get; set; }
    public double[] imageAnchor { get; set; } = [];
    public double imageScale { get; set; }
    public int offsetX { get; set; }
    public int offsetY { get; set; }
}
