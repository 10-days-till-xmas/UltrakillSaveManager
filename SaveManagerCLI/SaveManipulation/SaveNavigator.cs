using UltrakillSaveManager.SaveFile;

namespace SaveManagerCLI.SaveManipulation;

public class SaveNavigator
{
    private readonly FileInfo saveFileInfo;
    private readonly TypedSaveFile saveFile;
    public SaveNavigator(FileInfo saveFile)
    {
        saveFileInfo = saveFile;
        this.saveFile = LoadSave();
    }

    internal TypedSaveFile LoadSave()
    {
        return TypedSaveFile.GetSaveFile(saveFileInfo.FullName);
    }

    public void PrintSaveData()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(saveFileInfo.Name);
        Console.ResetColor();
        foreach (var field in saveFile.GetFields())
        {
            Console.WriteLine($"{field.Name}: {field.GetValue(saveFile.SaveData)}");
        }
    }
}
