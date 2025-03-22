namespace SaveManagerCLI.MenuTools.OptionTree;

internal class Branch(string name, params Node[] children) : Node(name)
{
    public readonly Node[] children = children;
}
