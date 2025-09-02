using System.Text.RegularExpressions;

namespace BedrockSvrLog.Helpers;

public static class LogHelpers
{
    public static List<string> playerIgnoreList = new List<string>();

    public static string getPfidFromLogLine(string logLine)
    {
        // Data looks like this. NO LOG FILE! - [2025-08-06 16:27:10:488 INFO] Player disconnected: RandomPlayer, xuid: 1234567890123456, pfid: a12ab345678c9d01
        int startIndex = logLine.IndexOf("pfid:") + 5; // 5 is the length of "pfid:"
        int endIndex = logLine.IndexOf(',', startIndex);
        if (endIndex == -1) // If no comma found, take the rest of the string
        {
            endIndex = logLine.Length;
        }
        return logLine.Substring(startIndex, endIndex - startIndex).Trim();
    }

    public static RealmStoryData? GetRealmStoryDataFromLogLine(string logLine)
    {
        // example data. NO LOG FILE! - [2025-08-05 17:42:42:156 INFO] Realms Story :: event: FirstNetherPortalLit, xuids: [ 1234567890123456 ]

        int startIndex = logLine.IndexOf("event:") + 6; // 6 is the length of "event:"
        int endIndex = logLine.IndexOf(',', startIndex);
        if (endIndex == -1) // If no comma found, take the rest of the string
        {
            endIndex = logLine.Length;
        }
        string eventType = logLine.Substring(startIndex, endIndex - startIndex).Trim();

        startIndex = logLine.IndexOf("xuids:") + 6; // 6 is the length of "xuids:"
        endIndex = logLine.IndexOf(']', startIndex);
        if (endIndex == -1) // If no closing bracket found, take the rest of the string
        {
            endIndex = logLine.Length;
        }
        string xuid = logLine.Substring(startIndex, endIndex - startIndex).Trim().Replace("[", "").Replace("]", "").Trim();
        if (string.IsNullOrEmpty(xuid))
        {
            FileHelpers.writeToDebugFile($"Error: Unable to extract XUID from log line: {logLine}");
            return null;
        }
        if (string.IsNullOrEmpty(eventType))
        {
            FileHelpers.writeToDebugFile($"Error: Unable to extract event type from log line: {logLine}");
            return null;
        }

        return new RealmStoryData
        {
            Xuid = xuid,
            EventType = eventType
        };
    }

    public static DateTime getDateTimeFromLogLine(string logLine)
    {
        // Example: NO LOG FILE! - [2025-08-06 16:27:06:947 INFO] Player connected:

        int startIndex = logLine.IndexOf('[') + 1;
        int endIndex = logLine.IndexOf("INFO]");

        if (startIndex <= 0 || endIndex <= startIndex)
        {
            FileHelpers.writeToDebugFile($"Error: Unable to extract timestamp from log line: {logLine}");
            return DateTime.Now;
        }

        string timeStampWithColonMs = logLine.Substring(startIndex, endIndex - startIndex).Trim();

        // Replace last colon with a dot to match valid datetime format
        int lastColon = timeStampWithColonMs.LastIndexOf(':');
        if (lastColon != -1)
        {
            timeStampWithColonMs = timeStampWithColonMs.Substring(0, lastColon) + "." + timeStampWithColonMs.Substring(lastColon + 1);
        }

        // Now the format should be: 2025-08-06 16:27:06.947
        if (DateTime.TryParseExact(timeStampWithColonMs, "yyyy-MM-dd HH:mm:ss.fff", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDateTime))
        {
            return parsedDateTime;
        }
        else
        {
            FileHelpers.writeToDebugFile($"Error: Unable to parse date time from log line: {logLine}");
            return DateTime.Now;
        }
    }

    public static void InitiliazeAndLoadPlayerIgnoreList()
    {
        string ignoreFilePath = "ignorePlayers.txt";
        if (!File.Exists(ignoreFilePath))
        {
            // Create the file if it does not exist
            File.WriteAllText(ignoreFilePath, "");
        }
        playerIgnoreList = File.ReadAllLines(ignoreFilePath).Select(p => p.Trim()).ToList();
        FileHelpers.writeToDebugFile($"Debug: Loaded {playerIgnoreList.Count} players from ignore list.");
    }


    public static bool ContainsPlayerIgnored(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            return false; // Cannot ignore if player name is not found
        }


        if (playerIgnoreList.Contains(playerName))
        {
            FileHelpers.writeToDebugFile($"Debug: Ignoring player {playerName} as they are in the ignore list.");
            return true; // Player is in the ignore list
        }
        return false; // Player is not in the ignore list
    }

    public static string GetPlayerNameFromLogLine(string logLine)
    {
        // Data looks like this. NO LOG FILE! - [2025-08-06 16:27:06:947 INFO] Player connected: RandomPlayer, xuid: 1234567890123456
        // Also look like this. NO LOG FILE! - [2025-08-06 16:27:10:488 INFO] Player disconnected: RandomPlayer, xuid: 1234567890123456, pfid: a12ab345678c9d01
        int startIndex = logLine.IndexOf("Player ") + 7; // 7 is the length of "Player "
        int endIndex = logLine.IndexOf(',', startIndex);
        if (endIndex == -1) // If no comma found, take the rest of the string
        {
            endIndex = logLine.Length;
        }
        return logLine.Substring(startIndex, endIndex - startIndex).Trim().Replace("disconnected:", "").Replace("Spawned: ", "").Replace("connected: ", "");
    }

    public static string GetXuidFromLogLine(string logLine)
    {
        // Data looks like this. NO LOG FILE! - [2025-08-06 16:27:06:947 INFO] Player connected: RandomPlayer, xuid: 1234567890123456
        // Also look like this. NO LOG FILE! - [2025-08-06 16:27:10:488 INFO] Player disconnected: RandomPlayer, xuid: 1234567890123456, pfid: a12ab345678c9d01
        int startIndex = logLine.IndexOf("xuid:") + 5; // 5 is the length of "xuid:"
        int endIndex = logLine.IndexOf(',', startIndex);

        if (endIndex == -1) // If no comma found, take the rest of the string
        {
            endIndex = logLine.Length;
        }

        return logLine.Substring(startIndex, endIndex - startIndex).Trim();
    }

    public static WorldTimeDaySpawnPoint GetTimeAndDayFromString(string logLine)
    {
        //Example input: NO LOG FILE! - [2025-08-30 20:18:25:400 INFO] [Scripting] [TIME] Day: 20 Time of Day: 8300 World SpawnX: 0 SpawnY: 32767 SpawnZ: 0
        var regex = new Regex(
        @"Day:\s*(\d+)\s*Time\s+of\s+Day:\s*(\d+).*?SpawnX:\s*(-?\d+)\s*SpawnY:\s*(-?\d+)\s*SpawnZ:\s*(-?\d+)", RegexOptions.IgnoreCase);

        var match = regex.Match(logLine);
        if (match.Success)
        {
            return new WorldTimeDaySpawnPoint
            {
                Day = int.Parse(match.Groups[1].Value),
                Time = int.Parse(match.Groups[2].Value),
                SpawnX = int.Parse(match.Groups[3].Value),
                SpawnY = int.Parse(match.Groups[4].Value),
                SpawnZ = int.Parse(match.Groups[5].Value)
            };
        }

        return new WorldTimeDaySpawnPoint
        {
            Day = -1,
            Time = null,
            SpawnX = null,
            SpawnY = null,
            SpawnZ = null
        };
    }

    public static EntityDeath? GetKilledEntity(string logLine)
    {
        // Exmaple [2025-09-02 18:15:26:611 INFO] [Scripting] Script Entity Death: minecraft:wandering_trader at: 266, 75, 399 in minecraft:overworld by: player - ThePlayerMan
        string pattern = @"Script Entity Death:\s+(\S+)\s+at:\s*(-?\d+),\s*(-?\d+),\s*(-?\d+)\s+in\s+(\S+)\s+by:\s+player\s*-\s*(\S+)";

        const string FilterString = "minecraft:";

        Match match = Regex.Match(logLine, pattern);
        if (!match.Success)
        {
            return null;
        }
        return new EntityDeath
        {
            EntityType = match.Groups[1].Value.Replace(FilterString,"").Trim(),
            PositionX = int.Parse(match.Groups[2].Value),
            PositionY = int.Parse(match.Groups[3].Value),
            PositionZ = int.Parse(match.Groups[4].Value),
            Dimension = match.Groups[5].Value.Replace(FilterString,"").Trim(),
            PlayerName = match.Groups[6].Value.Trim()
        };
    }

    public static EntityLocation? GetLocationDataFromString(string logLine)
    {
        //NO LOG FILE! - [2025 - 08 - 24 15:25:34:604 INFO][Scripting][TRACKING] MrPlayerName is at X:0, Y: 77, Z: -23 in minecraft: overworld
        // Example input: NO LOG FILE! - [2025-08-30 20:50:48:557 INFO] [Scripting] [TRACKING] TheStinkPotatoe is at X:241, Y:83, Z:396 in minecraft:overworld. SpawnPoint SpawnX: 236, SpawnY: 86, SpawnZ: 398
        string pattern = @"\[TRACKING\]\s+(.+?)\s+is at X:\s*(-?\d+),\s*Y:\s*(-?\d+),\s*Z:\s*(-?\d+)\s+in\s+minecraft:([^\.\s]+)\.\s*SpawnPoint\s*SpawnX:\s*(-?\d+),\s*SpawnY:\s*(-?\d+),\s*SpawnZ:\s*(-?\d+)";
        Match match = Regex.Match(logLine, pattern);
        if (!match.Success)
        {
            FileHelpers.writeToDebugFile($"Error: Unable to extract location data from log line: {logLine}");
            return null;
        }
        return new EntityLocation
        {
            EntityName = match.Groups[1].Value.Trim(),
            x = int.Parse(match.Groups[2].Value),
            y = int.Parse(match.Groups[3].Value),
            z = int.Parse(match.Groups[4].Value),
            dimension = match.Groups[5].Value.Trim(),
            SpawnX = int.TryParse(match.Groups[6].Value, out var sx) ? sx : null,
            SpawnY = int.TryParse(match.Groups[7].Value, out var sy) ? sy : null,
            SpawnZ = int.TryParse(match.Groups[8].Value, out var sz) ? sz : null
        };
    }
}
