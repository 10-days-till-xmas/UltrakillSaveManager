namespace SaveManagerCLI;

internal class ProgramSettings
{
    // display settings that'd include things like the save directory, assembly path, theme maybe? etc...

    internal static void PrintSettingsMenu()
    {
        Console.WriteLine("Settings Menu:");
        Console.WriteLine("1. Change Save Directory");
        Console.WriteLine("2. Change Assembly Path");
        Console.WriteLine("3. Change Theme");
        Console.WriteLine("4. Go Back");
    }

    internal static void PromptToChangeSaveDirectory()
    {
        Console.WriteLine("Enter the save folder directory:");

        string saveFolderDir;
        do
        {
            saveFolderDir = @"C:/Users/10_days_till_xmas/Documents/coding/C#/UltraModding/ultrakill+bepinex/Saves";
            //Console.ReadLine()!;

        } while (string.IsNullOrWhiteSpace(saveFolderDir));
        Console.WriteLine("SaveData Folder Directory: " + saveFolderDir);
    }

}
