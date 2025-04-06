namespace SaveManagerCLI.OptionTree;

/// <summary>
/// Class to handle selecting options from an <see cref="Option"/> tree
/// </summary>
/// <remarks>
/// The <see cref="globalRoot"/> is the reference <see cref="Option"/> used by this instance, i.e . it is (usually) not selectable.
/// </remarks>
public class OptionSelector
{
    private readonly Option globalRoot;
    public Option CurrentRoot { get; private set; }
    public Option CurrentOption { get; private set; }

    public Option[] LocalOptions => CurrentRoot.Children.ToArray();

    // TODO: Use this and provide it for each option, so it can all remain collapsible even when going deeper
    public Option[] VisibleLocalOptions => LocalOptions.Limit(LocalCurrentIndex, 4).ToArray();

    public (Option Option, int depth)[] VisibleOptions => GetVisibleOptions(globalRoot, 0);

    private static (Option Option, int depth)[] GetVisibleOptions(Option root, int depth)
    {
        List<(Option Option, int depth)> visibleOptions = [];
        foreach (var (option, index) in root.Children.Select((item, index) => (item, index))) // use LINQ to only select the middle few options
        {
            if (root.Children.selectedIndex is not null)
            {
            }
            visibleOptions.Add((option, depth));
            if (option.IsRoot)
            {
                visibleOptions.AddRange(GetVisibleOptions(option, depth + 1));
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

    /// <summary>
    /// Selects the current Option, where if its a branch, itll expand, if not itll return the value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selectedLeaf"></param>
    /// <returns></returns>
    public bool Select<T>(out T? selectedLeaf)
    {
        dynamic currentOptionNode = CurrentOption.Node;

        if (currentOptionNode is ILeaf)
        {
            selectedLeaf = currentOptionNode.Value;

            return true;
        }
        if (CurrentOption.Children.Count == 0)
        {
            selectedLeaf = default;
            // Add some sort of error message here
            return false;
        }
        CurrentOption.Parent!.ChildRoot = CurrentOption;
        CurrentRoot = CurrentOption;
        CurrentOption = CurrentRoot.Children[0];
        CurrentRoot.MakeRootForce();
        CurrentRoot.Parent.Children.SelectedOption = CurrentRoot;
        selectedLeaf = default;
        return false;
    }

    /// <summary>
    /// Returns to the previous option in the tree, if possible.
    /// </summary>
    /// <returns>Whether or not it moved back successfully</returns>
    public bool GoBack()
    {
        if (CurrentRoot != globalRoot)
        {
            CurrentRoot.MakeNotRoot();
            CurrentOption = CurrentRoot;
            CurrentRoot = CurrentOption.Parent!;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Moves to the option at the given index in the current options list.
    /// </summary>
    /// <param name="index">The desired index to move to</param>
    /// <exception cref="ArgumentOutOfRangeException">If the index is greater than the length of <see cref="LocalOptions"/></exception>
    public void GoToOption(int index)
    {
        if (index < 0 || index >= LocalOptions.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index)); // Do I even have to bother with this?
        }
        CurrentOption = LocalOptions[index];
    }

    /// <summary>
    /// Move up in the list of options, wrapping around to the last option if at the top.
    /// </summary>
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

    /// <summary>
    /// Move down in the list of options, wrapping around to the first option if at the bottom.
    /// </summary>
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