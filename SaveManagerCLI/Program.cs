using SaveManagerCLI.OptionTree;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Branch MainMenu = new("Main Menu",
    new Leaf("Modify Saves", () => Console.WriteLine("Go Into save navigator!!")),
    new Branch("Settings",
        new Leaf("Edit Save folder", () => Console.WriteLine("editing save folder placeholder")),
        new Leaf("Edit Assembly Path", () => Console.WriteLine("editing assembly path placeholder")),
        new Leaf("Edit Theme", () => Console.WriteLine("editing theme placeholder"))
        ),
    new Leaf("Exit", () => Environment.Exit(0))
    );

OptionSelector baseSelector = new(new Option(MainMenu));
Delegate onExecute = baseSelector.PrintOptionSelector();
if (onExecute is Action act)
{
    act();
}
else
{
    ConsoleUtils.Warn("onExecute is not an Action");
}
