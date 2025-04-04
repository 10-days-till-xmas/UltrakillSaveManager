using SaveManagerCLI;

namespace SaveManagerCLI.OptionTree;

public class Option
{
    public class OptionCollection : List<Option>
    {
        public int? selectedIndex = null;

        public Option? SelectedOption
        {
            get
            {
                return (selectedIndex == null) ? null : this[(int)selectedIndex];
            }
            set
            {
                if (value is null)
                {
                    selectedIndex = null;
                    return;
                }
                if (!this.TryGetIndexOf(value!, out int temp))
                {
                    throw new InvalidOperationException("Option not found in collection");
                }
                selectedIndex = temp;
            }
        }

        public OptionCollection() : base([])
        {
        }

        public OptionCollection(IEnumerable<Option> options) : base(options)
        {
        }

        /// <summary>
        /// Gets the range of visible options, and whether or not the upper and lower bounds are collapsed.
        /// </summary>
        /// <param name="range"></param>
        /// <returns>Whether or not the upper or lower ranges have been collapsed</returns>
        /// <exception cref="InvalidOperationException">Throws if <see cref="selectedIndex"/> is <see langword="null"/></exception>
        public (bool IsLowerCollapsed, bool IsUpperCollapsed) GetVisibleOptionRange(out Range range, int radius)
        {
            if (selectedIndex is null)
            {
                throw new InvalidOperationException("No option selected");
            }
            range = MathUtils.BoundedRange(Count, (int)selectedIndex, radius);
            return (range.Start.Value > 0, range.End.Value < Count);
        }

        public int[] GetVisibleIndices(int size, int index)
        {
            return Enumerable.Range(0, base.Count).ToArray().Limit(index, size);
        }

        public Option[] GetVisibleOptions(int radius)
        {
            if (selectedIndex is null)
            {
                throw new InvalidOperationException("No option selected");
            }
            var (IsLowerCollapsed, IsUpperCollapsed) = GetVisibleOptionRange(out Range range, radius);

            return base.ToArray()[range];
        }
    }

    public Node Node { get; init; }

    public string Name
    {
        get
        {
            string indentation = new(' ', depth * 4);
            string prefix;
            if (!HasChildren)
            {
                prefix = "  ";
            }
            else if (IsRoot)
            {
                prefix = "↓ ";
            }
            else
            {
                prefix = "→ ";
            }
            return indentation + prefix + Node.Name;
        }
    }

    /// <summary>
    /// The depth of the option in the tree.
    /// </summary>
    /// <remarks>
    /// 0 is the root, 1 means 1 level deep, etc.
    /// </remarks>
    public int depth;

    public readonly OptionCollection Children = [];

    public Option?[] Roots
    {
        get
        {
            Option? parent = Parent;
            List<Option> parents = [];
            while (parent is not null)
            {
                parents.Add(parent);
                parent = parent.Parent;
            }
            parents.Reverse();
            return [.. parents];
        }
    }

    public Option? Parent { get; private init; }

    private bool _isRoot = false;

    public bool IsRoot
    {
        get
        { return _isRoot; }
        private set
        {
            _isRoot = value;
            if (Parent is null)
                return;

            if (value)
            {
                Parent.Children.SelectedOption = this;
            }
            else if (Parent.Children.SelectedOption == this)
            {
                Parent.Children.selectedIndex = null;
            }
        }
    }

    public bool HasChildren => Children.Count > 0;

    public Option? ChildRoot
    {
        get => Children.SelectedOption;
        set => Children.SelectedOption = value;
    }

    public Option(Node node, Option? Parent = null)
    {
        Node = node;
        this.Parent = Parent;

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
            IBranch branch => [.. branch.Children.Select(child => new Option(child, this))],
            ILeaf => [],
            _ => throw new ArgumentException("Node must be a Branch or a Leaf"),
        };
    }

    public void MakeRoot()
    {
        if (!Parent?.IsRoot ?? true)
        {
            throw new InvalidOperationException("Parent must be a root to make this a root");
        }
        IsRoot = true;
        if (Parent is null)
            return;
        Parent.ChildRoot = this;
        foreach (var sibling in Parent.Children)
        {
            sibling.MakeNotRoot();
        }
    }

    public void MakeRootForce()
    {
        IsRoot = true;
        if (Parent is null)
            return;
        Parent.MakeRootForce();
        Parent.ChildRoot = this;
        Parent.Children.SelectedOption = this;
        foreach (var sibling in Parent.Children.Where(sibling => sibling != this))
        {
            if (sibling == this)
                continue;
            sibling.MakeNotRoot();
        }
    }

    public void MakeNotRoot()
    {
        IsRoot = false;
        Children.selectedIndex = null;
        foreach (var child in Children)
        {
            child.MakeNotRoot();
        }
        if (Parent is null)
            return;
        if (Parent.ChildRoot == this)
            Parent.ChildRoot = null;
    }

    public bool IsVisible
    {
        get
        {
            if (Parent is null)
                return false;
            if (!Parent.IsRoot)
                return false;

            return Parent.Children.GetVisibleOptions(3).Contains(this);
        }
    }

    public override string ToString()
    {
        return Name;
    }
}