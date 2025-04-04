using SaveManagerCLI;
using SaveManagerCLI.OptionTree;
using SaveManagerCLI.SaveManipulation;
using SaveManagerCLI.SaveManipulation.ClassManipulation;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.Title = "Save Manager CLI";

SaveNavigator navigator = new(new FileInfo(@"C:\Program Files (x86)\Steam\steamapps\common\ULTRAKILL\Saves\Slot1\generalprogress.bepis"));

ClassViewer viewer = new("difficulty0progress", navigator.saveFile.SaveData, navigator.saveFile.Type);
viewer.PrintOptions();

ConsoleUtils.WaitForEnterPress();

Branch MainMenu = new("Main SaveMenu",
    new Leaf<Action>("Modify Saves", SimpleDirectoryExplorer.PrintDirectoryTree),
    ProgramSettings.Branch,
    new Leaf<Action>("Exit", () => Environment.Exit(0))
    );
OptionSelector baseSelector = new(new Option(MainMenu));

while (true)
{
    Console.Clear();
    Action onExecute = baseSelector.PrintOptionSelector<Action>();
    onExecute();
}