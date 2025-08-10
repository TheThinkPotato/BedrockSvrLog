using System.Runtime.Versioning;
using System.Text.Json;

namespace WebAPI;

public static class AvatarHelper
{
    private static string gamerTagAvatarsJsonFile = "gamerTagAvatars.json";
    private static string backgroundColor = "b6e3f4";
    private static string size = "64";

    public static string GetDiceBearAvatarUrl(string gamerTag)
    {
        gamerTag = getNameTagReplacementFromNameTagJsonFile(gamerTag);
        return $"https://api.dicebear.com/9.x/pixel-art/svg?seed={gamerTag}&size={size}&backgroundColor={backgroundColor}";
    }

    private static string getNameTagReplacementFromNameTagJsonFile(string gamerTag)
    {
        // Get the full path to the JSON file
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), gamerTagAvatarsJsonFile);

        if (!File.Exists(jsonFilePath))
        {
            createGameTagAvatarsJsonFile();
            return gamerTag;
        }

        var jsonString = File.ReadAllText(jsonFilePath);

        if (string.IsNullOrEmpty(jsonString))
        {
            return gamerTag;
        }

        try
        {
            GameTagAvatars data = JsonSerializer.Deserialize<GameTagAvatars>(jsonString)!;

            var matchingTag = data.GameTags.FirstOrDefault(tag =>
                string.Equals(tag.GameTag, gamerTag, StringComparison.OrdinalIgnoreCase));

            if (matchingTag != null)
            {
                return matchingTag.DiceBearSeed;
            }
            else
            {
                return gamerTag;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            return gamerTag;
        }
    }

    private static void createGameTagAvatarsJsonFile()
    {
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), gamerTagAvatarsJsonFile);
        var data = new GameTagAvatars
        {
            GameTags = [new GameTagAvatar { GameTag = "gamerTag", DiceBearSeed = "diceBearSeed" }]
        };

        var jsonString = JsonSerializer.Serialize(data);
        File.WriteAllText(jsonFilePath, jsonString);
    }
}

public class GameTagAvatar
{
    public string GameTag { get; set; } = string.Empty;
    public string DiceBearSeed { get; set; } = string.Empty;
}

public class GameTagAvatars
{
    public List<GameTagAvatar> GameTags { get; set; } = [];
}