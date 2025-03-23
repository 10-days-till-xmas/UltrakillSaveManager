using SaveManagerCLI.OptionTree;
using System.Drawing;

namespace SaveManagerCLI;

internal static class ProgramSettings
{
    public static Branch Branch = new("Settings",
        new Leaf("Change Save Directory", PromptToChangeSaveDirectory),
        new Leaf("Change Assembly Path", PromptToChangeAssemblyPath),
        new Leaf("Change Game Directory", PromptToChangeGameDirectory)
    );

    public static void GameDir(string gameDir)
    {
        SaveDirectory = Path.Join(gameDir, "Saves");
        AssemblyPath = Path.Join(gameDir, "ULTRAKILL_Data", "Managed", "Assembly-CSharp.dll");
    }
        public static string SaveDirectory { get; set; } = @"C:\Program Files (x86)\Steam\steamapps\common\ULTRAKILL\Saves";
    /// <summary>
    /// Used just in case the assembly path can't be resolved on its own
    /// </summary>
    public static string AssemblyPath { get; set; } = @"C:\Program Files (x86)\Steam\steamapps\common\ULTRAKILL\ULTRAKILL_Data\Managed\Assembly-CSharp.dll";

    internal static void PromptToChangeGameDirectory()
    {
        string gameDir = "";
        PromptToChangeString(ref gameDir, "Game Directory");
        GameDir(gameDir);
    }
    internal static void PromptToChangeSaveDirectory()
    {
        string saveFolderDir = SaveDirectory;
        PromptToChangeString(ref saveFolderDir, "SaveData Folder Directory");
        SaveDirectory = saveFolderDir;
    }
    internal static void PromptToChangeAssemblyPath()
    {
        string assemblyPath = AssemblyPath;
        PromptToChangeString(ref assemblyPath, "Assembly-CSharp.dll Path");
        AssemblyPath = assemblyPath;
    }

    internal static void PromptToChangeString(ref string target, string name)
    {
        Console.WriteLine($"Enter the {name}:");
        string? input;
        do
        {
            input = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(input));
        Console.Write($"{name}: ");
        Color stringColor = Color.FromArgb(0xd6, 0x9d, 0x85);
        ConsoleUtils.ColoredString($"\"{input}\"", stringColor);
        target = input!;
    }
}
