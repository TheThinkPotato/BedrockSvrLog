namespace BedrockSvrLog;

public static class FileHelpers
{
    public const string LogFolder = Program.LogFolder;
    public const string DebugFile = Program.DebugFile;
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
}
