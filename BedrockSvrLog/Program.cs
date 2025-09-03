using BedrockSvrLog.Data;
using BedrockSvrLog.Helpers;
using BedrockSvrLog.Models;
using BedrockSvrLog.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BedrockSvrLog;

class Program
{
    public const string Version = "0.2i";
    public const string Title = "Bedrock Server Log Tool Wrapper";

    public static string bedrockServerFolderLocation = @"..\";
    public static string dbSource = "Data Source=app.db";

    public const string LogFolder = "logs";
    public const string ServerLogFile = "server.log";
    public const string DebugFile = "debug.txt";

    public const string RealmLogString = "Realms Story ::";
    public const string PlayerSpawnString = "Player Spawned:";
    public const string PlayerConnectedString = "Player connected:";
    public const string PlayerDisconnectedString = "Player disconnected:";
    public const string ApiBridgeScriptString = "[Scripting]";

    protected static AppDbContext MyAppDbContext;

    public static WorldRepository _worldRepo;
    public static UserRepository _userRepo;
    public static LoginRepository _loginRepo;
    public static RealmRepository _realmRepo;
    public static PlayerKillRepository _playerKillRepo;
    public static PlayerDeathRepository _playerDeathRepo;

    private const string SplashText = $"\t{Title} Version {Version}\n\tBy Daniel Lopez.";
    static async Task Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite(dbSource);
        MyAppDbContext = new AppDbContext(optionsBuilder.Options);

        MyAppDbContext.Database.EnsureCreated();

        // Calcualte the acutual folder on the drive
        bedrockServerFolderLocation = Path.GetFullPath(bedrockServerFolderLocation);

        var _worldRepo = new WorldRepository(MyAppDbContext);
        var _userRepo = new UserRepository(MyAppDbContext);
        var _loginRepo = new LoginRepository(MyAppDbContext);
        var _realmRepo = new RealmRepository(MyAppDbContext);
        var _playerKillRepo = new PlayerKillRepository(MyAppDbContext);
        var _playerDeathRepo = new PlayerDeathRepository(MyAppDbContext);

        //Update missing avatars
        await _userRepo.UpdateMissingUserAvatarLinks(new CancellationToken());

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
                _worldRepo.UpdateWorldName(worldName);
            }

            if (!string.IsNullOrEmpty(worldSeed))
            {
                Console.WriteLine($"World Seed: {worldSeed}\n");
                _worldRepo.UpdateWorldSeed(worldSeed);
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
                        var timeAndDaySpawnPoint = LogHelpers.GetTimeAndDayFromString(line);
                        var locationDetails = LogHelpers.GetLocationDataFromString(line);
                        var killDetails = LogHelpers.GetKilledEntity(line);
                        var playerDeathDetails = LogHelpers.GetPlayerDeath(line);

                        var currentWorldDetails = await _worldRepo.getCurrentWorldDetails(new CancellationToken());

                        if (timeAndDaySpawnPoint != null && timeAndDaySpawnPoint.Day != -1)
                        {
                            await _worldRepo.UpdateWorldTableData(timeAndDaySpawnPoint, new CancellationToken());
                        }

                        if (locationDetails != null)
                        {
                            // Ensure spawn coordinates are set
                            if (locationDetails.SpawnX == null || locationDetails.SpawnY == null || locationDetails.SpawnZ == null)
                            {
                                locationDetails.SpawnX = currentWorldDetails?.SpawnX;
                                locationDetails.SpawnY = currentWorldDetails?.SpawnY;
                                locationDetails.SpawnZ = currentWorldDetails?.SpawnZ;
                            }

                            await _userRepo.UpdateUserLocationAsync(locationDetails, new CancellationToken());
                        }

                        if (killDetails != null)
                        {
                            var userDetails = await _userRepo.getCurrentUserDetails(killDetails.PlayerName, new CancellationToken());

                            if (userDetails != null && currentWorldDetails != null)
                            {
                                await _playerKillRepo.AddKillEventToDbAsync(killDetails, userDetails, currentWorldDetails, new CancellationToken());
                            }
                        }

                        if (playerDeathDetails != null)
                        {
                            var userDetails = await _userRepo.getCurrentUserDetails(playerDeathDetails.PlayerName, new CancellationToken());
                            if (userDetails != null && currentWorldDetails != null)
                            {
                                await _playerDeathRepo.AddPlayerDeathEventToDbAsync(playerDeathDetails, userDetails, currentWorldDetails, new CancellationToken());
                            }

                        }
                    }
                    else if (line != null && line.Contains(RealmLogString))
                    {
                        _realmRepo.addRealmEventToDb(LogHelpers.getDateTimeFromLogLine(line), LogHelpers.GetRealmStoryDataFromLogLine(line));
                    }
                    else if (line != null && line.Contains(PlayerConnectedString))
                    {
                        _userRepo.addUserToDb(LogHelpers.GetPlayerNameFromLogLine(line), LogHelpers.GetXuidFromLogLine(line), null);
                        _loginRepo.addUserLoginToDb(LogHelpers.GetXuidFromLogLine(line), LogHelpers.getDateTimeFromLogLine(line));
                    }

                    else if (line != null && line.Contains(PlayerSpawnString))
                    {
                        _userRepo.updateuserPfid(LogHelpers.GetXuidFromLogLine(line), LogHelpers.getPfidFromLogLine(line));
                        _loginRepo.updateUserLoginSpawnTime(LogHelpers.GetXuidFromLogLine(line), LogHelpers.getDateTimeFromLogLine(line));
                    }

                    else if (line != null && line.Contains(PlayerDisconnectedString))
                    {
                        _loginRepo.updateUserLoginLogoutTime(LogHelpers.GetXuidFromLogLine(line), LogHelpers.getDateTimeFromLogLine(line));
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
