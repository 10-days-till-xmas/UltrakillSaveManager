namespace SaveManagerCLI.OptionTree;

public interface IBranch
{
    public Node[] Children { get; }
}

public class Branch(string name, params Node[] children) : Node(name), IBranch
{
    public Node[] Children { get; } = children;
}

public class Branch<T>(string name, T value, params Node[] children) : Node<T>(name, value), IBranch
{
    public Node[] Children { get; } = children;
}