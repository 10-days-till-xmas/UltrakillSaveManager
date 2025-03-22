using SaveManagerCLI.MenuTools.OptionTree;

namespace SaveManagerCLI.MenuTools;

internal class MainMenu
{
    private static MainMenu? _instance = null;
    internal static MainMenu Instance
    {
        get
        {
            _instance ??= new MainMenu();
            return _instance;
        }
    }

    internal void LoadMenu()
    {
        ProgramMenu programMenu = new();
        Option Root = new(programMenu.OptionsTree);
        OptionSelector selector = new(Root);
        Console.Clear();
        Console.WriteLine("Select one of the following Options to proceed:");
        selector.PrintOptionSelector();
        // TODO: Implement the rest of the program. Handle executing the leaves
    }
}
