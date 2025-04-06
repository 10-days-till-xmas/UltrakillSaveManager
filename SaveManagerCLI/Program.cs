using SaveManagerCLI.OptionTree;
using SaveManagerCLI.OptionTree.ConsoleInterface;
using SaveManagerCLI.SaveManipulation;
using System.Text;

namespace SaveManagerCLI;
internal class Program
{
    internal readonly static Branch MainMenu = new("Main SaveMenu",
                                                   new Leaf<Action>("Modify Saves", SimpleDirectoryExplorer.PrintDirectoryTree),
                                                   ProgramSettings.Branch,
                                                   new Leaf<Action>("Exit", () => Environment.Exit(0)));

    internal readonly static OptionSelector mainMenuSelector = new(new Option(MainMenu));
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "Save Manager CLI";

        MenuLoop();
    }

    private static void MenuLoop()
    {
        while (true)
        {
            Console.Clear();
            Action onExecute = ConsoleOptionSelector.PrintOptionSelector<Action>(mainMenuSelector);
            onExecute();
        }
    }
}