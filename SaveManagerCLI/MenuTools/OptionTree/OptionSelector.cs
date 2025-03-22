namespace SaveManagerCLI.MenuTools.OptionTree;

internal class OptionSelector
{
    internal Option CurrentOption 
    { 
        get
        {
            return options[CurrentIndex];
        }
        private set
        {
            CurrentIndex = Array.IndexOf(options, value);
        }
    }

    private int _currentIndex = 0;
    internal int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            if (IsIndexValid(value))
            {
                _currentIndex = value;
            }
            else
            {
                _currentIndex = 0;
            }
        }
    }

    internal Option[] options;
    internal Option? Parent => CurrentOption.Parent;

    internal OptionSelector(Option[] root)
    {
        CurrentOption = root[0];
        options = root;
    }

    private bool IsIndexValid(int index)
    {
        return 0 <= index && index < CurrentOption.Children.Count;
    }

    internal void Select(int index)
    {
        if (CurrentOption.Node is Leaf leaf)
        {
            leaf.onExecute(); // Not sure if this is a good idea
        }
        if (CurrentOption.HasChildren)
        {
            if (IsIndexValid(index))
            {
                CurrentOption = CurrentOption.Children[index];
            }
        }
    }
    internal void GoBack()
    {
        if (CurrentOption.Parent is not null)
        {
            CurrentOption = CurrentOption.Parent;
        }
    }
}
