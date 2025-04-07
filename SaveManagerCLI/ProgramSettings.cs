using SaveManagerCLI.OptionTree;
using System.Drawing;

namespace SaveManagerCLI;

internal static class ProgramSettings
{
    public static Branch Branch = new("Settings",
        new Leaf<Action>("Change Save Directory", PromptToChangeSaveDirectory),
        new Leaf<Action>("Change Assembly Path", PromptToChangeAssemblyPath),
        new Leaf<Action>("Change Game Directory", PromptToChangeGameDirectory)
    );

    public static void GameDir(string gameDir)
    {
        SaveDirectory = Path.Join(gameDir, "Saves");
        AssemblyPath = Path.Join(gameDir, "ULTRAKILL_Data", "Managed", "Assembly-CSharp.dll");
    }

    private static string _saveDirectory = @"C:\Program Files (x86)\Steam\steamapps\common\ULTRAKILL\Saves";

    public static string SaveDirectory
    {
        get
        {
            return _saveDirectory;
        }

        set
        {
            if (Directory.Exists(value))
            {
                _saveDirectory = value;
            }
            else
            {
                ConsoleUtils.Error($"The directory \"{value}\" does not exist.");
                ConsoleUtils.WaitForKeyPress();
            }
        }
    }

    private static string _assemblyPath = @"C:\Program Files (x86)\Steam\steamapps\common\ULTRAKILL\ULTRAKILL_Data\Managed\Assembly-CSharp.dll";

    /// <summary>
    /// A path to Ultrakill's Assembly-CSharp.dll. <br/>
    /// Used in case the assembly path can't be resolved on its own
    /// </summary>
    public static string AssemblyPath
    {
        get
        {
            return _assemblyPath;
        }
        set
        {
            if (!File.Exists(value))
            {
                ConsoleUtils.Error($"The file \"{value}\" does not exist.");
                ConsoleUtils.WaitForKeyPress();
                return;
            }
            _assemblyPath = value;
        }
    }

    internal static void PromptToChangeGameDirectory()
    {
        string gameDir = "";
        if (PromptToChangeString(ref gameDir, "Game Directory", StringType.DirectoryPath))
        {
            GameDir(gameDir);
        }
    }

    internal static void PromptToChangeSaveDirectory()
    {
        string saveFolderDir = SaveDirectory;
        PromptToChangeString(ref saveFolderDir, "SaveData Folder Directory", StringType.DirectoryPath);
        SaveDirectory = saveFolderDir;
    }

    internal static void PromptToChangeAssemblyPath()
    {
        string assemblyPath = AssemblyPath;
        PromptToChangeString(ref assemblyPath, "Assembly-CSharp.dll Path", StringType.FilePath);
        AssemblyPath = assemblyPath;
    }

    private enum StringType
    {
        String,
        FilePath,
        DirectoryPath
    }

    private static bool PromptToChangeString(ref string target, string name, StringType stringType = StringType.String)
    {
        if (!string.IsNullOrWhiteSpace(target))
        {
            Console.WriteLine($"Old value: {target}");
        }
        Console.WriteLine($"Enter the {name}:");
        string? input;
        do
        {
            input = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(input));
        switch (stringType)
        {
            case StringType.String:
                break;

            case StringType.FilePath:
                if (!File.Exists(input))
                {
                    ConsoleUtils.Error($"The file \"{input}\" does not exist.");
                    ConsoleUtils.WaitForKeyPress();
                    return false;
                }
                break;

            case StringType.DirectoryPath:
                if (!Directory.Exists(input))
                {
                    ConsoleUtils.Error($"The directory \"{input}\" does not exist.");
                    ConsoleUtils.WaitForKeyPress();
                    return false;
                }
                break;
        }
        target = input!;
        Console.Write($"{name}: ");
        Color stringColor = Color.FromArgb(0xd6, 0x9d, 0x85); // a peach colour
        string coloredInput = ConsoleUtils.ColoredString($"\"{input}\"", stringColor);
        Console.WriteLine(coloredInput);
        return true;
    }
}