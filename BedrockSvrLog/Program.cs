using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

class Program
{

    public const string LogFolder = "logs";
    static async Task Main(string[] args)
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
                await File.AppendAllTextAsync($"{LogFolder}\\server.log", line + Environment.NewLine);
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


    public static void CheckCreateLogFolder()
    {
        if(!Directory.Exists(LogFolder))
        {
            Directory.CreateDirectory(LogFolder);
        }
    }

}
