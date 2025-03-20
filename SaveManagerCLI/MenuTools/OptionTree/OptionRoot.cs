

namespace SaveManagerCLI.MenuTools.OptionTree;

internal class OptionRoot
{
    public static IEnumerable<Branch> GetChildren(Branch branch)
    {
        return branch.children.OfType<Branch>();
    }
}
