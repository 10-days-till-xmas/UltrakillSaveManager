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
    public Option[] Options => CurrentRoot.Children.ToArray();

    public Option CurrentOption { get; private set; }

    public int CurrentIndex => Array.IndexOf(Options, CurrentOption);

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
            leaf.onExecute(); // Not sure if this is a good idea
            return;
        }
        CurrentRoot = CurrentOption;
    }
    public bool Select(out Action? leaf_onExecute)
    {
        if (CurrentOption.Node is Leaf leaf)
        {
            leaf_onExecute = leaf.onExecute;
            return true;
        }
        CurrentRoot = CurrentOption;
        leaf_onExecute = null;
        return false;
    }

    public bool GoBack()
    {
        if (CurrentRoot != globalRoot)
        {
            CurrentOption = CurrentRoot;
            return true;
        }
        return false;
    }

    public void GoTo(int index)
    {
        if (index < 0 || index >= Options.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        CurrentOption = Options[index];
    }

    public void MoveUp()
    {
        if (CurrentIndex == 0)
        {
            CurrentOption = Options[^1];
        }
        else
        {
            CurrentOption = Options[CurrentIndex - 1];
        }
    }

    public void MoveDown()
    {
        if (CurrentIndex == Options.Length - 1)
        {
            CurrentOption = Options[0];
        }
        else
        {
            CurrentOption = Options[CurrentIndex + 1];
        }
    }
}
