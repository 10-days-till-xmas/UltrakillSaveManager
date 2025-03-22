using SaveManagerCLI;


Console.WriteLine("Enter the save folder directory:");

string saveFolderDir;
do
{
    saveFolderDir = @"C:/Users/10_days_till_xmas/Documents/coding/C#/UltraModding/ultrakill+bepinex/Saves";
    //Console.ReadLine()!;

} while (string.IsNullOrWhiteSpace(saveFolderDir));
Console.WriteLine("SaveData Folder Directory: " + saveFolderDir);


ProgramMenu programMenu = new(saveFolderDir);
ProgramMenu.DisplayOptions(programMenu.OptionsTree);

//Slot Slot1 = new(1);

//var generalProgressData = TypedSaveFile.GetSaveFile(saveFolderDir + Slot1.GeneralProgress);

//var fields = generalProgressData.GetFields();
//foreach (var field in fields)
//{
//    Console.WriteLine($"{field.Name}: {field.GetValue(generalProgressData.SaveData)}");
//}
