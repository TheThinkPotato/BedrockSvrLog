using BedrockSvrLog.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BedrockSvrLog;

class Program
{
    public const string Version = "0.2d";
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
        
        underlinedText( SplashText, '=', 1);

        ServerApiBridgeScript.checkSetupServerApiBridgeScript();
        
        underline(SplashText.Length, '=',1,2);

        var udpBridge = new UdpBridge();
        var udpBridgeThread = new Thread(() => udpBridge.startUdpListner());
        udpBridgeThread.Start();

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


            FileHelpers.CheckCreateLogFolder();

            using var process = new Process { StartInfo = psi };
            process.Start();

            // Task to read and log stdout
            var outputTask = Task.Run(async () =>
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = await process.StandardOutput.ReadLineAsync();
                    Console.WriteLine(line);
                    // Pseudocode:
                    // - The code currently prints the last message in udpBridge.MessageBuffer if the buffer is not empty.
                    // - However, this does not clear the buffer, so the same message may be printed repeatedly.
                    // - To avoid this, consume and print all new messages, then clear the buffer after printing.
                    // - Replace the code block with one that prints and clears all messages in the buffer.

                    //if (udpBridge.MessageBuffer.Count > 0)
                    //{
                    //    foreach (var msg in udpBridge.MessageBuffer)
                    //    {
                    //        Console.WriteLine(msg);
                    //    }
                    //    udpBridge.MessageBuffer.Clear();
                    //}
                    var messageBuffur = udpBridge.GetMessages();
                    messageBuffur.ForEach(msg => Console.WriteLine(msg));


                    await File.AppendAllTextAsync($"{LogFolder}\\{ServerLogFile}", line + Environment.NewLine);

                    if (line != null && LogHelpers.containtPlayerIgnored(line));
                    else if (line != null && line.Contains(RealmLogString))
                    {
                        dbHelpers.addRealmEventToDb(LogHelpers.getDateTimeFromLogLine(line), LogHelpers.GetRealmStoryDataFromLogLine(line));
                    }
                    else if (line != null && line.Contains(PlayerConnectedString))
                    {
                        dbHelpers.addUserToDb(LogHelpers.getPlayerNameFromLogLine(line), LogHelpers.getXuidFromLogLine(line), null);
                        dbHelpers.addUserLoginToDb(LogHelpers.getXuidFromLogLine(line), LogHelpers.getDateTimeFromLogLine(line));
                    }

                    else if (line != null && line.Contains(PlayerSpawnString))
                    {
                        dbHelpers.updateuserPfid(LogHelpers.getXuidFromLogLine(line), LogHelpers.getPfidFromLogLine(line));
                        dbHelpers.updateUserLoginSpawnTime(LogHelpers.getXuidFromLogLine(line), LogHelpers.getDateTimeFromLogLine(line));
                    }

                    else if (line != null && line.Contains(PlayerDisconnectedString))
                    {
                        dbHelpers.updateUserLoginLogoutTime(LogHelpers.getXuidFromLogLine(line), LogHelpers.getDateTimeFromLogLine(line));
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

            var udpReadTask = Task.Run(async () =>
            {
                while (!process.HasExited)
                {
                    var messages = udpBridge.GetMessages();
                    if (messages.Count > 0)
                    {
                        foreach (var msg in messages)
                        {
                            Console.WriteLine(msg);
                        }
                    }
                    //await Task.Delay(100); // Adjust delay as needed
                }
            });

            await Task.WhenAny(outputTask, errorTask, inputTask, udpReadTask);

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
