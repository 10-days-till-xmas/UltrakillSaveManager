namespace SaveManagerCLI.OptionTree;

public abstract class Node(string name)
{
    public string Name { get; init; } = name;
}

public abstract class Node<T>(string name, T value) : Node(name)
{
    public T Value { get; set; } = value;
}
