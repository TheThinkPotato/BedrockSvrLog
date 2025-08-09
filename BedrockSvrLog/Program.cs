using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BedrockSvrLog;

class Program
{
    public const string Version = "0.2a";
    public const string Title = "Bedrock Server Log Tool Wrapper";

    public static string bedrockServerFolderLocation = @"..\";

    public const string LogFolder = "logs";
    public const string ServerLogFile = "server.log";
    public const string DebugFile = "debug.txt";

    public const string RealmLogString = "Realms Story ::";
    public const string PlayerSpawnString = "Player Spawned:";
    public const string PlayerConnectedString = "Player connected:";
    public const string PlayerDisconnectedString = "Player disconnected:";

    protected static AppDbContext MyAppDbContext;
    public static DbHelpers dbHelpers;

    static async Task Main(string[] args)
    {
        
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=app.db");
        MyAppDbContext = new AppDbContext(optionsBuilder.Options);

        MyAppDbContext.Database.EnsureCreated();
        dbHelpers = new DbHelpers(MyAppDbContext);

        // Calcualte the acutual folder on the drive
        bedrockServerFolderLocation = Path.GetFullPath(bedrockServerFolderLocation);
        
        underlinedText($"\t{Title} Version {Version}\n\tBy Daniel Lopez.", '=', 2);

        try
        {
            var psi = new ProcessStartInfo
            {
                WorkingDirectory = bedrockServerFolderLocation,
                FileName = "bedrock_server.exe",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };


            var exeFullPath = Path.Combine(bedrockServerFolderLocation, psi.FileName);

                if (!File.Exists(exeFullPath))
                {
                    Console.WriteLine($"Error:\tThe file '{exeFullPath}' does not exist.\n\tPlease ensure that the folder is in bedrock server folder.\n Current folder: {bedrockServerFolderLocation} ");
                    return;
                }
                psi.FileName = exeFullPath;


            CheckCreateLogFolder();

            using var process = new Process { StartInfo = psi };
            process.Start();

            // Task to read and log stdout
            var outputTask = Task.Run(async () =>
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = await process.StandardOutput.ReadLineAsync();
                    Console.WriteLine(line);
                    await File.AppendAllTextAsync($"{LogFolder}\\{ServerLogFile}", line + Environment.NewLine);
                    
                    if (line != null && line.Contains(RealmLogString))
                    {
                        dbHelpers.addRealmEventToDb(getDateTimeFromLogLine(line), GetRealmStoryDataFromLogLine(line));
                    }
                    else if (line != null && line.Contains(PlayerConnectedString))
                    {
                        dbHelpers.addUserToDb(getPlayerNameFromLogLine(line), getXuidFromLogLine(line), null);
                        dbHelpers.addUserLoginToDb(getXuidFromLogLine(line), getDateTimeFromLogLine(line));
                    }

                    else if (line != null && line.Contains(PlayerSpawnString))
                    {
                        dbHelpers.updateuserPfid(getXuidFromLogLine(line), getPfidFromLogLine(line));
                        dbHelpers.updateUserLoginSpawnTime(getXuidFromLogLine(line), getDateTimeFromLogLine(line));
                    }

                    else if (line != null && line.Contains(PlayerDisconnectedString))
                    {
                        dbHelpers.updateUserLoginLogoutTime(getXuidFromLogLine(line), getDateTimeFromLogLine(line));
                    }
                }
            });

            // Task to read and log stderr (optional)
            var errorTask = Task.Run(async () =>
            {
                while (!process.StandardError.EndOfStream)
                {
                    var line = await process.StandardError.ReadLineAsync();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(line);
                    Console.ResetColor();
                    await File.AppendAllTextAsync($"{LogFolder}\\server.log", "[ERROR] " + line + Environment.NewLine);
                }
            });

            // Task to forward user input to server
            var inputTask = Task.Run(async () =>
            {
                while (!process.HasExited)
                {
                    var input = Console.ReadLine();
                    await process.StandardInput.WriteLineAsync(input);
                }
            });

            await Task.WhenAny(outputTask, errorTask, inputTask);

            process.WaitForExit();
        }
        catch (Exception ex)
        {
            var currentTimeDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            writeToDebugFile($"{currentTimeDate} Exception: " + ex.ToString());
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }

    public static void writeToDebugFile(string message)
    {
        var debugFilePath = $"{LogFolder}\\{DebugFile}";
        if (!File.Exists(debugFilePath))
        {
            File.WriteAllText(debugFilePath, "Debug Log Created: " + DateTime.Now + Environment.NewLine);
        }
        File.AppendAllText(debugFilePath, message + Environment.NewLine);
    }

    public static void CheckCreateLogFolder()
    {
        if (!Directory.Exists(LogFolder))
        {
            Directory.CreateDirectory(LogFolder);
        }
    }

    public static DateTime getDateTimeFromLogLine(string logLine)
    {
        // Example: NO LOG FILE! - [2025-08-06 16:27:06:947 INFO] Player connected:

        int startIndex = logLine.IndexOf('[') + 1;
        int endIndex = logLine.IndexOf("INFO]");

        if (startIndex <= 0 || endIndex <= startIndex)
        {
            writeToDebugFile($"Error: Unable to extract timestamp from log line: {logLine}");
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
            writeToDebugFile($"Error: Unable to parse date time from log line: {logLine}");
            return DateTime.Now;
        }
    }

    public static string getPlayerNameFromLogLine(string logLine)
    {
        // Data looks like this. NO LOG FILE! - [2025-08-06 16:27:06:947 INFO] Player connected: RandomPlayer, xuid: 1234567890123456
        // Also look like this. NO LOG FILE! - [2025-08-06 16:27:10:488 INFO] Player disconnected: RandomPlayer, xuid: 1234567890123456, pfid: a12ab345678c9d01
        int startIndex = logLine.IndexOf("Player ") + 7; // 7 is the length of "Player "
        int endIndex = logLine.IndexOf(',', startIndex);
        if (endIndex == -1) // If no comma found, take the rest of the string
        {
            endIndex = logLine.Length;
        }
        return logLine.Substring(startIndex, endIndex - startIndex).Trim().Replace("disconnected:", "").Replace("Spawned:", "").Replace("connected:", "");
    }

    public static string getXuidFromLogLine(string logLine)
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

    public static void underlinedText(string text, char character, int newLines = 0)
    {
        Console.WriteLine(new string(character, text.Length));
        Console.WriteLine(text);
        Console.WriteLine(new string(character, text.Length));
        if (newLines > 0)
        {
            Console.WriteLine(new string('\n', newLines - 1));
        }
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
            writeToDebugFile($"Error: Unable to extract XUID from log line: {logLine}");
            return null;
        }
        if (string.IsNullOrEmpty(eventType))
        {
            writeToDebugFile($"Error: Unable to extract event type from log line: {logLine}");
            return null;
        }

        return new RealmStoryData
        {
            Xuid = xuid,
            EventType = eventType
        };
    }

}

public record RealmStoryData
{
    public string Xuid { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
}
