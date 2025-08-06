using BedrockSvrLog.Data;
using System.Diagnostics;

namespace BedrockSvrLog;

class Program
{
    public const string Version = "0.1b";
    public const string Title = "Bedrock Server Log Tool Wrapper";

    public const string LogFolder = "logs";
    public const string ServerLogFile = "server.log";

    public const string RealmLogsFile = "realmlogs.log";
    public const string RealmLogString = "Realms Story ::";

    public const string PlayersListFile = "playerslist.csv";
    public const string PlayerSpawnString = "Player Spawned:";

    public const string DebugFile = "debug.txt";

    protected static AppDbContext MyAppDbContext;
    public static DbHelpers dbHelpers;

    static async Task Main(string[] args)
    {
        MyAppDbContext = new AppDbContext();

        MyAppDbContext.Database.EnsureCreated();
        dbHelpers = new DbHelpers(MyAppDbContext);

        underlinedText($"\t{Title} Version {Version}\n\tBy Daniel Lopez.", '=', 2);

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "bedrock_server.exe",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            // Check if filename exists
            if (!File.Exists(psi.FileName))
            {
                Console.WriteLine($"Error:\tThe file '{psi.FileName}' does not exist.\n\tPlease ensure that the executable is in bedrock server folder.");
                return;
            }

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
                        await File.AppendAllTextAsync($"{LogFolder}\\{RealmLogsFile}", line + Environment.NewLine);
                    }
                    else if (line != null && line.Contains(PlayerSpawnString))
                    {
                        createPlayerCsvFromLog(line);
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

    // create a csv breaking up "Player Spawned: PlayerNAme xuid: 0000000000000000, pfid: a0a0a00000a000a0" into playerName, xuid, pfid
    public static void createPlayerCsvFromLog(string logLine)
    {
        createCsvFile($"{LogFolder}\\{PlayersListFile}", "PlayerName,XUID,PFID");

        logLine = (logLine.Split("]"))[1].Trim();

        logLine = logLine.Substring(PlayerSpawnString.Length).Trim();
        var parts = logLine.Split(new[] { "Player Spawned:", "xuid:", "pfid:" }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 3)
        {
            writeToDebugFile($"Debug: Player Spawned Log: {logLine.Replace(",","")} - Parts: {string.Join(", ", parts)}");

            var playerName = parts[0].Trim();
            var xuid = parts[1].Trim().Replace(",", "");
            var pfid = parts[2].Trim();

            var csvPath = $"{LogFolder}\\{PlayersListFile}";
            bool exists = File.Exists(csvPath);
            var lines = exists ? File.ReadAllLines(csvPath) : Array.Empty<string>();
            bool playerExists = false;

            foreach (var line in lines)
            {
                var cols = line.Split(',');
                if (cols.Length > 0 && string.Equals(cols[0], playerName, StringComparison.OrdinalIgnoreCase))
                {
                    playerExists = true;
                    break;
                }
            }

            dbHelpers.addUserToDb(playerName, xuid, pfid);

            if (!playerExists)
            {
                File.AppendAllText(csvPath, $"{playerName},{xuid},{pfid}{Environment.NewLine}");
            }

        }
        else
        {
            writeToDebugFile($"Debug: Player Spawned Log: {logLine} - Parts: {string.Join(", ", parts)} - Invalid format");
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


    public static void createCsvFile(string filePath, string header)
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, header + Environment.NewLine);
        }
    }

    public static void CheckCreateLogFolder()
    {
        if (!Directory.Exists(LogFolder))
        {
            Directory.CreateDirectory(LogFolder);
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
}
