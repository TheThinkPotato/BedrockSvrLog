namespace BedrockSvrLog.Helpers;

public static class FileHelpers
{
    public const string LogFolder = Program.LogFolder;
    public const string DebugFile = Program.DebugFile;

    public const string ServerConfigFile = "server.properties";
    public static void WriteToDebugFile(string message)
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


    public static string GetWorldSeedFromConfig(string configFilePath)
    {
        configFilePath = Path.Combine(configFilePath, ServerConfigFile);
        if (File.Exists(configFilePath))
        {
            var lines = File.ReadAllLines(configFilePath);
            foreach (var line in lines)
            {
                if (line.StartsWith("level-seed="))
                {
                    return line.Substring("level-seed=".Length).Trim();
                }
            }
        }
        return "Unknown"; // Default world seed if not found
    }

    public static string GetWorldNameFromConfig(string configFilePath)
    {
        configFilePath = Path.Combine(configFilePath, ServerConfigFile);

        if (File.Exists(configFilePath))
        {
            var lines = File.ReadAllLines(configFilePath);
            foreach (var line in lines)
            {
                if (line.StartsWith("level-name="))
                {
                    return line.Substring("level-name=".Length).Trim();
                }
            }
        }
        return "Bedrock level"; // Default world name if not found
    }
}
