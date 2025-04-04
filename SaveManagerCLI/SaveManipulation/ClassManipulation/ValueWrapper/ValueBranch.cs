using SaveManagerCLI.OptionTree;

namespace SaveManagerCLI.SaveManipulation.ClassManipulation.ValueWrapper;

internal class ValueBranch(string name, Wrapper value, params Node[] children) : ValueNode(name, value), IBranch
{
    public Node[] Children { get; } = children;
}