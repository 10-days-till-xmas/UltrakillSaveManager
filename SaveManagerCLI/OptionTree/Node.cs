namespace SaveManagerCLI.OptionTree;

/// <summary>
/// Base class for all nodes in the tree.
/// </summary>
/// <param name="name">The name of this node</param>
public abstract class Node(string name)
{
    /// <summary>
    /// The name of this node
    /// </summary>
    public string Name { get; init; } = name;
}

public abstract class NodeBase(string name, object? value) : Node(name)
{
    /// <summary>
    /// The value within this node, stored as <see cref="object"/>
    /// </summary>
    public virtual object? Value { get; set; } = value;
}

/// <summary>
/// Generic node class that can be used to create a tree structure.
/// </summary>
/// <remarks>Be wary when casting this since <see cref="NodeBase.Value"/> would likely be different to <see cref="Node{T}.Value"/></remarks>
/// <typeparam name="T">The type of <paramref name="value"/></typeparam>
/// <param name="name">The name of this node</param>
/// <param name="value">An arbitrary stored value</param>
public abstract class Node<T>(string name, T value) : NodeBase(name, value)
{
    /// <summary>
    /// The value within this node, stored as <typeparamref name="T"/>
    /// </summary>
    public new T Value { get; set; } = value;
}