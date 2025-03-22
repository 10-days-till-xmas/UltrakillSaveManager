namespace SaveManagerCLI.MenuTools.OptionTree;

/// <summary>
/// Class to handle selecting options from an <see cref="Option"/> tree
/// </summary>
/// <remarks>
/// Calls the <see cref="Leaf.onExecute"/> method when a leaf is selected.
/// The <see cref="globalRoot"/> cannot be chosen as an option, i.e . it is not selectable and is only used as a reference.
/// </remarks>
public partial class OptionSelector
{
    private readonly Option globalRoot;
    public Option CurrentRoot { get; private set; }
    public Option CurrentOption { get; private set; }

    public Option[] LocalOptions => CurrentRoot.Children.ToArray();

    public Option[] VisibleOptions => GetVisibleOptions(globalRoot);

    private static Option[] GetVisibleOptions(Option root)
    {
        List<Option> visibleOptions = [];
        foreach (var option in root.Children)
        {
            visibleOptions.Add(option);
            if (option.isRoot)
            {
                visibleOptions.AddRange(GetVisibleOptions(option));
            }
        }
        return visibleOptions.ToArray();
    }

    public int LocalCurrentIndex => Array.IndexOf(LocalOptions, CurrentOption);

    public OptionSelector(Option globalRoot)
    {
        CurrentRoot = globalRoot;
        this.globalRoot = globalRoot;
        CurrentOption = globalRoot.Children[0];
    }

    public void SelectAndExecute()
    {
        if (CurrentOption.Node is Leaf leaf)
        {
            leaf.onExecute.DynamicInvoke(); // Not sure if this is a good idea
            return;
        }
        CurrentRoot = CurrentOption;
    }
    public bool Select(out Delegate? leaf_onExecute)
    {
        if (CurrentOption.Node is Leaf leaf)
        {
            leaf_onExecute = leaf.onExecute;
            return true;
        }
        CurrentRoot = CurrentOption;
        CurrentOption = CurrentRoot.Children[0];
        CurrentRoot.isRoot = true;
        leaf_onExecute = null;
        return false;
    }

    public bool GoBack()
    {
        if (CurrentRoot != globalRoot)
        {
            CurrentRoot.isRoot = false;
            CurrentOption = CurrentRoot;
            CurrentRoot = CurrentOption.Parent!;
            return true;
        }
        return false;
    }

    public void GoToOption(int index)
    {
        if (index < 0 || index >= LocalOptions.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        CurrentOption = LocalOptions[index];
    }

    public void MoveUp()
    {
        if (LocalCurrentIndex == 0)
        {
            CurrentOption = LocalOptions[^1];
        }
        else
        {
            CurrentOption = LocalOptions[LocalCurrentIndex - 1];
        }
    }

    public void MoveDown()
    {
        if (LocalCurrentIndex == LocalOptions.Length - 1)
        {
            CurrentOption = LocalOptions[0];
        }
        else
        {
            CurrentOption = LocalOptions[LocalCurrentIndex + 1];
        }
    }
}
