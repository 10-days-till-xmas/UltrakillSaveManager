namespace SaveManagerCLI.MenuTools.OptionTree;

public class Option
{
    private readonly struct OptionName(Option option)
    {
        internal readonly Option option = option;
        public override string ToString()
        {
            string indentation = new(' ', (option.depth * 4) - 2);
            string prefix;
            if (!option.HasChildren)
            {
                prefix = "  ";
            }
            else if (option.isRoot)
            {
                prefix = "↓ ";
            }
            else
            {
                prefix = "→ ";
            }
            return indentation + prefix + option.Node.Name;
        }
    }
    public Node Node { get; init; }

    private readonly OptionName optionName;
    public string Name => optionName.ToString();

    /// <summary>
    /// The depth of the option in the tree. 
    /// </summary>
    /// <remarks>
    /// 0 is the root, 1 means 1 level deep, etc.
    /// </remarks>
    public int depth;

    public readonly List<Option> Children = [];

    public Option? Parent { get; private init; }

    public bool isRoot = false;
    public bool HasChildren => Children.Count > 0;

    public Option(Node node, Option? Parent = null)
    {
        Node = node;
        this.Parent = Parent;
        optionName = new(this);

        if (Parent is not null)
        {
            depth = Parent.depth + 1;
        }
        else
        {
            depth = 0;
        }

        Children = node switch
        {
            Branch branch => (from child in branch.children
                              select new Option(child, this)).ToList(),
            Leaf => [],
            _ => throw new ArgumentException("Node must be a Branch or a Leaf"),
        };
    }

    public Option[] InstantiateChildren()
    {
        if (Node is Branch branch)
        {
            return branch.children.Select(child => new Option(child, this)).ToArray();
        }
        else
        {
            return [];
        }
    }
}
