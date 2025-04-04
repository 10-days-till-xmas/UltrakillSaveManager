using SaveManagerCLI.SaveManipulation.ClassManipulation;
using UltrakillSaveManager.SaveFile;

namespace SaveManagerCLI.SaveManipulation;

public class SaveNavigator
{
    private readonly FileInfo saveFileInfo;
    internal readonly TypedSaveFile saveFile;

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
        var saveViewer = new ClassViewer(saveFileInfo.Name, saveFile.SaveData, saveFile.Type);
        bool repeat = true;
        do
        {
            var output = saveViewer.PrintOptions();
            Console.Clear();
            if (output is Wrapper outWrapper)
            {
                var getOut = outWrapper.Getter();
            }
            else
                repeat = false;
        } while (repeat);
    }
}