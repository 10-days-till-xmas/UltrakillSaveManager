using SaveManagerCLI.OptionTree;
using SaveManagerCLI.OptionTree.ConsoleInterface;
using SaveManagerCLI.SaveManipulation;
using System.Reflection;
using System.Text;

namespace SaveManagerCLI;

internal class Program
{
    internal static readonly Branch MainMenu = new("Main SaveMenu",
                                                   new Leaf<Action>("Modify Saves", SimpleDirectoryExplorer.PrintDirectoryTree),
                                                   ProgramSettings.Branch,
                                                   new Leaf<Action>("Exit", () => Environment.Exit(0)));

    internal static readonly OptionSelector mainMenuSelector = new(new Option(MainMenu));

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = "Save Manager CLI";

        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            string path = ProgramSettings.AssemblyPath;
            return File.Exists(path) ? Assembly.LoadFrom(path) : null;
        };

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