using SaveManagerCLI.OptionTree;
using System.Text;
using SaveManagerCLI.SaveManipulation;
using SaveManagerCLI;

Console.OutputEncoding = Encoding.UTF8;
Console.Title = "Save Manager CLI";


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
