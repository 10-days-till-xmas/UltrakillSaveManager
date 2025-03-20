namespace SaveManagerCLI.MenuTools.OptionTree;

internal class Leaf(string name, Action onExecute) : Node(name)
{
    public readonly Action onExecute = onExecute;
}
