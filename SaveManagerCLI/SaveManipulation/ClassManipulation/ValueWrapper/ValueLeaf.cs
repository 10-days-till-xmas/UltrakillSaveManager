using SaveManagerCLI.OptionTree;

namespace SaveManagerCLI.SaveManipulation.ClassManipulation.ValueWrapper;

internal class ValueLeaf(string name, Wrapper value) : ValueNode(name, value), ILeaf
{
}