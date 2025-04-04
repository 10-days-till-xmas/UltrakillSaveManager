namespace SaveManagerCLI.OptionTree;

public abstract class Node(string name)
{
    public string Name { get; init; } = name;
}

public abstract class NodeBase(string name, object? value) : Node(name)
{
    public virtual object? Value { get; set; } = value;
}

public abstract class Node<T>(string name, T value) : NodeBase(name, value)
{
    public new T Value { get; set; } = value;
}