using System;
using System.Reflection;

namespace BedrockSvrLog;

public record RequiredFile
{
    public string Path { get; init; }
    public string FilePath { get; init; }
    public string ResourceName { get; init; }
    public RequiredFile(string path, string filePath, string resourceName)
    {
        Path = path;
        FilePath = filePath;
        ResourceName = resourceName;
    }
}


public static class ServerApiBridgeScript
{
    public const string BH_Pack_Folder = @"development_behavior_packs\";
    public const string BH_Pack_Name = @"BedrockSvrLog\";
    
    public static string Worlds_Folder = Program.bedrockServerFolderLocation + @"worlds\";
    public static string BH_Pack_FullPath = Program.bedrockServerFolderLocation + BH_Pack_Folder + BH_Pack_Name;
    public static string BH_Pack_FullPath_Scripts = BH_Pack_FullPath + @"scripts\";

    public static List<RequiredFile> requiredFiles = new List<RequiredFile> {
        new RequiredFile(BH_Pack_FullPath, BH_Pack_FullPath + "manifest.json", "BedrockSvrLog.ServerApiBridgeScript.manifest.json"),
        new RequiredFile(BH_Pack_FullPath_Scripts, BH_Pack_FullPath_Scripts + "main.js", "BedrockSvrLog.ServerApiBridgeScript.main.js")
    };

    public static void checkSetupServerApiBridgeScript()
    {

        var worldName = getWorldNameFromConfig();

        if (string.IsNullOrEmpty(worldName))
        {
            Console.WriteLine("Error: World name not found in server.properties. Please check your server configuration.");
            return;
        }

        requiredFiles.Add(new RequiredFile(
            Worlds_Folder,
            Worlds_Folder + worldName + @"\world_behavior_packs.json",
            "BedrockSvrLog.ServerApiBridgeScript.world_behavior_packs.json"
        ));


        // Check if the required files exist
        if (!checkFilesExist())
        {
            createFiles();
            return;
        }
    }

    public static bool checkFilesExist()
    {
        // Check if the files exist in the current directory
        foreach (var file in requiredFiles)
        {
            if (!File.Exists(file.FilePath))
            {
                Console.WriteLine($"Error: The file '{file.FilePath.Replace(Program.bedrockServerFolderLocation, "")}' does not exist.");
                return false;
            }

            Console.WriteLine($"File {stripFileNameFromResourceName(file.ResourceName)} exists \u2713");
        }
        return true;
    }

    public static string stripFileNameFromResourceName(string resourceName)
    {
        return resourceName.Split(".")[2] + "." + resourceName.Split(".")[3];
    }

    public static void createFiles()
    {
        createFolders();

        var assembly = Assembly.GetExecutingAssembly();

        foreach (var file in requiredFiles)
        {
            try
            {
                CopyFiles(file.ResourceName, file.FilePath);
                Console.WriteLine($"Copied resource '{stripFileNameFromResourceName(file.ResourceName)}' to '{file.FilePath.Replace(Program.bedrockServerFolderLocation, "")}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying resource '{stripFileNameFromResourceName(file.ResourceName)}': {ex.Message}");
            }
        }
    }


    public static void createFolders()
    {

        foreach (var file in requiredFiles)
        {
            var directory = Path.GetDirectoryName(file.FilePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Console.WriteLine($"Created folder: {directory.Replace(Program.bedrockServerFolderLocation, "")}");
            }
        }
    }

    public static void CopyFiles(string resourseName, string destinationFile)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using (Stream stream = assembly.GetManifestResourceStream(resourseName))
        {
            if (stream == null)
                throw new Exception($"Resource '{resourseName}' not found.");
            using (FileStream fileStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
    }

    public static string getWorldNameFromConfig()
    {
        string configFilePath = Path.Combine(Program.bedrockServerFolderLocation, "server.properties");
        if (!File.Exists(configFilePath))
        {
            Console.WriteLine("Error: server.properties file not found.");
            return string.Empty;
        }

        var lines = File.ReadAllLines(configFilePath);

        foreach (var line in lines)
        {
            if (line.StartsWith("level-name="))
            {
                return line.Substring("level-name=".Length).Trim();
            }
        }
        Console.WriteLine("Error: level-name not found in server.properties.");
        return string.Empty;
    }

}