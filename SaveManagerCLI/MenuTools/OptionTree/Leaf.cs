namespace SaveManagerCLI.MenuTools.OptionTree;

internal class Leaf(string name, Delegate onExecute) : Node(name)
{
    public readonly Delegate onExecute = onExecute;
}