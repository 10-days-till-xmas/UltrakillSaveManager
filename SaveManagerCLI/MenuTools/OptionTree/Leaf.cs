namespace SaveManagerCLI.MenuTools.OptionTree;

public interface ILeaf;

public class Leaf(string name, Delegate onExecute) : Leaf<Delegate>(name, onExecute), ILeaf
{
    public Delegate OnExecute => Value;
}

public class Leaf<T>(string name, T value) : Node<T>(name, value), ILeaf;
