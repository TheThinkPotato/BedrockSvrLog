using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BedrockSvrLog;

class Program
{
    public const string Version = "0.2e";
    public const string Title = "Bedrock Server Log Tool Wrapper";

    public static string bedrockServerFolderLocation = @"..\";

    public const string LogFolder = "logs";
    public const string ServerLogFile = "server.log";
    public const string DebugFile = "debug.txt";

    public const string RealmLogString = "Realms Story ::";
    public const string PlayerSpawnString = "Player Spawned:";
    public const string PlayerConnectedString = "Player connected:";
    public const string PlayerDisconnectedString = "Player disconnected:";
    public const string ApiBridgeScriptString = "[Scripting]";

    protected static AppDbContext MyAppDbContext;
    public static DbHelpers dbHelpers;

    private const string SplashText = $"\t{Title} Version {Version}\n\tBy Daniel Lopez.";
    static async Task Main(string[] args)
    {

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=app.db");
        MyAppDbContext = new AppDbContext(optionsBuilder.Options);

        MyAppDbContext.Database.EnsureCreated();
        dbHelpers = new DbHelpers(MyAppDbContext);

        // Calcualte the acutual folder on the drive
        bedrockServerFolderLocation = Path.GetFullPath(bedrockServerFolderLocation);


        //Update missing avatars
        await dbHelpers.UpdateMissingUserAvatarLinks(new CancellationToken());

        underlinedText(SplashText, '=', 1);

        ServerApiBridgeScript.checkSetupServerApiBridgeScript();

        underline(SplashText.Length, '=', 1, 1);

        try
        {

            var worldName = FileHelpers.GetWorldNameFromConfig(bedrockServerFolderLocation);
            var worldSeed = FileHelpers.GetWorldSeedFromConfig(bedrockServerFolderLocation);

            FileHelpers.CheckCreateLogFolder();

            if (!string.IsNullOrEmpty(worldName))
            {
                Console.WriteLine($"World Name: {worldName}");
                dbHelpers.UpdateWorldName(worldName);
            }

            if (!string.IsNullOrEmpty(worldSeed))
            {
                Console.WriteLine($"World Seed: {worldSeed}\n");
                dbHelpers.UpdateWorldSeed(worldSeed);
            }

            underline(SplashText.Length, '=', 0, 2);

            LogHelpers.InitiliazeAndLoadPlayerIgnoreList();

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

                    if (line != null && LogHelpers.ContainsPlayerIgnored(line)) ;
                    if (line != null && line.Contains(ApiBridgeScriptString))
                    {
                        var timeAndDay = LogHelpers.GetTimeAndDayFromString(line);

                        if (timeAndDay != null && timeAndDay.Day != -1)
                        {
                            await dbHelpers.UpdateWorldTimeAndDay(timeAndDay, new CancellationToken());
                        }

                        var locationDetails = LogHelpers.GetLocationDataFromString(line);
                        if (locationDetails != null)
                        {
                            await dbHelpers.UpdateUserLocationAsync(locationDetails, new CancellationToken());
                        }
                    }
                    else if (line != null && line.Contains(RealmLogString))
                    {
                        dbHelpers.addRealmEventToDb(LogHelpers.getDateTimeFromLogLine(line), LogHelpers.GetRealmStoryDataFromLogLine(line));
                    }
                    else if (line != null && line.Contains(PlayerConnectedString))
                    {
                        dbHelpers.addUserToDb(LogHelpers.GetPlayerNameFromLogLine(line), LogHelpers.GetXuidFromLogLine(line), null);
                        dbHelpers.addUserLoginToDb(LogHelpers.GetXuidFromLogLine(line), LogHelpers.getDateTimeFromLogLine(line));
                    }

                    else if (line != null && line.Contains(PlayerSpawnString))
                    {
                        dbHelpers.updateuserPfid(LogHelpers.GetXuidFromLogLine(line), LogHelpers.getPfidFromLogLine(line));
                        dbHelpers.updateUserLoginSpawnTime(LogHelpers.GetXuidFromLogLine(line), LogHelpers.getDateTimeFromLogLine(line));
                    }

                    else if (line != null && line.Contains(PlayerDisconnectedString))
                    {
                        dbHelpers.updateUserLoginLogoutTime(LogHelpers.GetXuidFromLogLine(line), LogHelpers.getDateTimeFromLogLine(line));
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
            FileHelpers.writeToDebugFile($"{currentTimeDate} Exception: " + ex.ToString());
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
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

    public static void underline(int numberCharWidth, char character, int previousLines = 0, int postLines = 0)
    {
        for (int i = 0; i < previousLines; i++)
        {
            Console.WriteLine();
        }
        Console.WriteLine(new string(character, numberCharWidth));
        for (int i = 0; i < postLines; i++)
        {
            Console.WriteLine();
        }
    }
}

public record RealmStoryData
{
    public string Xuid { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
}
