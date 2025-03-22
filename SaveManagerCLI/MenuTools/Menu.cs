using SaveManagerCLI.MenuTools.OptionTree;

namespace SaveManagerCLI.MenuTools;

internal class Menu
{
    Branch MenuRoot { get; init; }

    internal void LoadMenu()
    {
        ProgramMenu programMenu = new();
        Branch optionRoot = new("LocalOptions",
            programMenu.SlotSaves(1),
            programMenu.SlotSaves(2),
            programMenu.SlotSaves(3),
            programMenu.SlotSaves(4),
            programMenu.SlotSaves(5)
        );

        Option Root = new(programMenu.OptionsTree);
        OptionSelector selector = new(Root);
        Console.Clear();
        Console.WriteLine("Select one of the following LocalOptions to proceed:");
        selector.PrintOptionSelector();
        // TODO: Implement the rest of the program. Handle executing the leaves
    }
}
