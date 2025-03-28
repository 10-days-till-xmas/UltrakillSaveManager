﻿namespace SaveManagerCLI.OptionTree;

/// <summary>
/// Class to handle selecting options from an <see cref="Option"/> tree
/// </summary>
/// <remarks>
/// The <see cref="globalRoot"/> cannot be chosen as an option, i.e . it is not selectable and is only used as a reference.
/// However, <c>allowEscapingFromRoot</c> in <see cref="PrintOptionSelector"/> can be set to <see langword="true"/> to allow returning from the method without actually choosing a value, enabling a default way to go back.
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
            leaf.OnExecute.DynamicInvoke(); // Not sure if this is a good idea
            return;
        }
        CurrentRoot = CurrentOption;
    }
    public bool Select<T>(out T? selectedLeaf)
    {
        if (CurrentOption.Node is Leaf<T> leaf)
        {
            selectedLeaf = leaf.Value;
            return true;
        }
        CurrentRoot = CurrentOption;
        CurrentOption = CurrentRoot.Children[0];
        CurrentRoot.isRoot = true;
        selectedLeaf = default;
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
