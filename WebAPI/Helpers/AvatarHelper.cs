using System.Runtime.Versioning;
using System.Text.Json;

namespace WebAPI;

public static class AvatarHelper
{
    private static string backgroundColor = "b6e3f4";
    private static string size = "64";

    public static string GetDiceBearAvatarUrl(string gamerTag)
    {
        return $"https://api.dicebear.com/9.x/pixel-art/svg?seed={gamerTag}&size={size}&backgroundColor={backgroundColor}";
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