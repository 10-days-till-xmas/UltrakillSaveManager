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

    private readonly Dictionary<string, Action> optionFuncs = new()
    {
        { "Exit", () => Environment.Exit(0) }
    };
    private OptionSelector? selector = null;
    internal void LoadMenu()
    {
        Console.Clear();
        Console.WriteLine("Select one of the following options to proceed:");
        selector = new([.. optionFuncs.Keys]);
        selector.PrintOptionSelector(true, out string selectedOption);
        Console.WriteLine("You selected: " + selectedOption);
        optionFuncs[selectedOption]();
    }
}
